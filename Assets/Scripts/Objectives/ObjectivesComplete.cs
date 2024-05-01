using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesComplete : MonoBehaviour
{
    [Header("Objectives to complete")]
    public Text objective1;
    public Text objective2;
    public Text objective3;
    public Text objective4;

    public static ObjectivesComplete occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void GetObjectivesDone1(bool obj1)
    {
        if(obj1 == true)
        {
            objective1.text = "1. Completed";
            objective1.color = Color.green;
        }
        else
        {
            objective1.text = "01. Find the Rifle";
            objective1.color = Color.white;
        }
    }

    public void GetObjectivesDone2(bool obj2)
    {
        if(obj2 == true)
        {
            objective2.text = "2. Completed";
            objective2.color = Color.green;
        }
        else
        {
            objective2.text = "02. Find villagers";
            objective2.color = Color.white;
        }
    }

    public void GetObjectivesDone3(bool obj3)
    {
        if(obj3 == true)
        {
            objective3.text = "3. Completed";
            objective3.color = Color.green;
        }
        else
        {
            objective3.text = "03. Find vehicle";
            objective3.color = Color.white;
        }
    }

        public void GetObjectivesDone4(bool obj4)
    {
        if(obj4 == true)
        {
            objective4.text = "4. Completed";
            objective4.color = Color.green;
        }
        else
        {
            objective4.text = "04. Take all of the villagers into vehicle";
            objective4.color = Color.white;
        }
    }

}
