using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie2 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    bool previouslyAttack;

    [Header("Zombie Standing Var")]
    public float zombieSpeed;

    [Header("Zombie mood/states")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInVisionRadius;
    public bool playerInAttackingRadius;

    [Header("Zombie Animation")]
    public Animator anim;

    public void Awake()
    {
        presentHealth = zombieHealth;
        healthBar.GiveFullHealth(zombieHealth);
        zombieAgent = GetComponent<NavMeshAgent>();
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }

    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if(!playerInVisionRadius && !playerInAttackingRadius) Idle();
        if(playerInVisionRadius && !playerInAttackingRadius) PursuePlayer();
        if(playerInVisionRadius && playerInAttackingRadius) AttackPlayer();
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
    }

    private void PursuePlayer()
    {
        if(zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Die", false);
        }
        else
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Die", true);
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);

        if(!previouslyAttack)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                anim.SetBool("Idle", false);
                anim.SetBool("Running", false);
                anim.SetBool("Attacking", true);
                anim.SetBool("Die", false);
            }

            previouslyAttack = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }

    private void ActiveAttacking()
    {
        previouslyAttack = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Die", true);

            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0f;
        attackingRadius = 0f;
        visionRadius = 0f;
        playerInAttackingRadius = false;
        playerInVisionRadius= false;
        Object.Destroy(gameObject, 5.0f);
    }
}
