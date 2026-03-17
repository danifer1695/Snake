using System;
using UnityEngine;
using UnityEngine.UI;

public class LoaderMenuButton : MonoBehaviour
{
    public void StartGame(string mapName)
    {
        LoadManager.Instance.StartGame(mapName);
    }

    public void ReturnToMainMenu()
    {
        LoadManager.Instance.GoToMainMenu();
    }
}
