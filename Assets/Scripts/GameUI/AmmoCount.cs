using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public Text ammunitionText;
    public Text magText;

    public static AmmoCount occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void UpdateAmmoCount(int presentAmmunition)
    {
        ammunitionText.text = "" + presentAmmunition;
    }

    public void UpdateMagCount(int mag)
    {
        magText.text = "" + mag;
    }
}
