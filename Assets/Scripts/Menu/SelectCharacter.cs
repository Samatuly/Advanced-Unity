using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    public GameObject selectCharacter;
    public GameObject mainMenu;
    public void OnCharacter1()
    {
        SceneManager.LoadScene("UndeadHavoc");
    }
    public void OnCharacter2()
    {
        SceneManager.LoadScene("UndeadHavoc1");
    }
    public void OnCharacter3()
    {
        SceneManager.LoadScene("UndeadHavoc2");
    }

    public void OnBackButton()
    {
        selectCharacter.SetActive(false);
        mainMenu.SetActive(true);
    }
}
