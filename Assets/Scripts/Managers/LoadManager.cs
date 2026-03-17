using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance { get; private set; }

    public string uiSceneName = "UI_Menus";
    public string titleSceneName = "Title";

    private void Awake()
    {
        //Singleton guard - destroy duplicate instances if there's already one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  //Add this object to DontDestroyOnLoad
    }

    public void StartGame(string mapName)
    {
        StartCoroutine(LoadGame(mapName));
    }

    public void Reload()
    {
        StartCoroutine(ReloadCurrentLevel());
    }

    public void GoToMainMenu()
    {
        StartCoroutine(ReturnToMainMenu());
    }

    //This co-routine is called when starting the game from the main menu ONLY.
    //It loads all UI scenes in addition to the selected map.
    IEnumerator LoadGame(string mapName)
    {
        //Load UI scenes additively
        yield return SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        //Load map
        yield return SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);

        //Unload current scene
        AsyncOperation unloadMenu = SceneManager.UnloadSceneAsync(
            SceneManager.GetSceneByName(titleSceneName)
        );
        yield return unloadMenu;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapName));

        //Show HUD if we are to implement one
    }

    IEnumerator ReloadCurrentLevel()
    {
        //Hide pause menu
        UIManager.Instance.HidePauseMenu();

        string sceneName = SceneManager.GetActiveScene().name;
        //Unload current level
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //Load it back up
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        //Set it as active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    //This co-routine is called when we want to go back to main menu from the pause menu
    //It unloads all UI scenes
    IEnumerator ReturnToMainMenu()
    {
        yield return SceneManager.UnloadSceneAsync(uiSceneName);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        SceneManager.LoadScene(titleSceneName);
    }

}
