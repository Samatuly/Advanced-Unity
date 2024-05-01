using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("HealthBoost")]
    public PlayerScript player;
    private float healthToGive = 120f;
    private float radius = 2.5f;

    [Header("Sounds")]
    public AudioClip healthBoostSound;
    public AudioSource audioSource;

    [Header("HealthBox Animator")]
    public Animator animator;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if(Input.GetKeyDown("f"))
            {
                animator.SetBool("Open", true);
                player.presentHealth = healthToGive;

                audioSource.PlayOneShot(healthBoostSound);
                Object.Destroy(gameObject, 1.9f);
            }
        }
    }

}
