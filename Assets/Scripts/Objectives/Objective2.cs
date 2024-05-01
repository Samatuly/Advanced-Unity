using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    public static Objective2 objective2;
    public bool isCompleted2 = false;

    private void Awake()
    {
        objective2 = this;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            ObjectivesComplete.occurence.GetObjectivesDone2(true);
            isCompleted2 = true;

            Destroy(gameObject, 2f);
        }
    }
}
