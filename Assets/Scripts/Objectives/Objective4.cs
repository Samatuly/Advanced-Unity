using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Vehicle")
        {
            ObjectivesComplete.occurence.GetObjectivesDone4(true);

            SceneManager.LoadScene("MainMenu");
        }
    }
}
