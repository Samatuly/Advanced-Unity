using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class About : MonoBehaviour
{
    public GameObject about;
    public GameObject settings;

    public void OnBackButton()
    {
        about.SetActive(false);
        settings.SetActive(true);
    }
}
