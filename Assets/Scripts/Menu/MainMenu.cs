using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject settings;
    public GameObject mainMenu;

    public void OnPlayButton()
    {
        selectCharacter.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnSettingsButton()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
