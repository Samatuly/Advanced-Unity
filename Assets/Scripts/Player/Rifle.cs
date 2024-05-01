using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class rifle : MonoBehaviour
{
    [Header("Rifle things")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    private float nextTimeToShoot = 0f;
    public Animator animator;
    public PlayerScript player;
    public Transform hand;
    public GameObject rifleUI;

    [Header("Rifle Ammunition and Shooting")]
    private int maxAmmunition = 32;
    public int mag = 10;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject WoodedEffect;
    public GameObject goreEffect;

    [Header("Sounds and UI")]
    public GameObject AmmoOutUI;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;


    private void Awake()
    {
        transform.SetParent(hand);
        rifleUI.SetActive(true);
        presentAmmunition = maxAmmunition;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(setReloading)
        {
            return;
        }

        if(presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            nextTimeToShoot = Time.time + 1f/fireCharge;
            Shoot();
        }
        else if(Input.GetButton("Fire1") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))) {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
        }
        else if(Input.GetButton("Fire1") && Input.GetButton("Fire2")) {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
        }
    }

    private void Shoot()
    {
        if(mag == 0)
        {
            StartCoroutine(AmmoOut());
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }

        AmmoCount.occurence.UpdateAmmoCount(presentAmmunition);
        AmmoCount.occurence.UpdateMagCount(mag);

        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if(objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(WoodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 1f);
            }
            else if(zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if(zombie2 != null)
            {
                zombie2.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading");
        animator.SetBool("Reloading", true);
        audioSource.PlayOneShot(reloadingSound);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maxAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;
    }

    IEnumerator AmmoOut()
    {
        AmmoOutUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        AmmoOutUI.SetActive(false);
    }
}
