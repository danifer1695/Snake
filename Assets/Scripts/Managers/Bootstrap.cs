using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public GameObject gameManager;

    public void Awake()
    {
        //GameManager needs to be decoupled from each individual scene before initializing it like this
        //InitializeGameManager();
        StartCoroutine(LaunchMenu());
    }
    
    private void InitializeGameManager()
    {
        DontDestroyOnLoad(gameManager);
    }

    IEnumerator LaunchMenu()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
        yield return load;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Title"));
        
        //Unload bootstrapper scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("mainScene"));
    }

}
