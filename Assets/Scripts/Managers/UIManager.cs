using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    private void Awake()
    {
        //Singleton guard - destroy duplicate instances if there's already one
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);  //Add this object to DontDestroyOnLoad
    }

    public void ShowPauseMenu() => SetActive(pauseMenu, true);
    public void HidePauseMenu() => SetActive(pauseMenu, false);

    public void ShowGameOverMenu() => SetActive(gameOverMenu, true);
    public void HideGameOverMenu() => SetActive(gameOverMenu, false);

    private void SetActive(GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }
}
