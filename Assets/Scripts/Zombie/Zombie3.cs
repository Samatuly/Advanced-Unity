using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Zombie3 : MonoBehaviour
{
    [Header("Zombie Health and Damage")]
    private float zombieHealth = 100f;
    private float presentHealth;
    public float giveDamage = 5f;

    [Header("Zombie Things")]
    public NavMeshAgent zombieAgent;
    public Transform LookPoint;
    public Camera AttackingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    bool previouslyAttack;

    [Header("Zombie Guarding Var")]
    public GameObject[] walkPoints;
    int currentZombiePosition = 0;
    public float zombieSpeed = 0.4f;
    float walkingPointRadius = 2;

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
        zombieAgent = GetComponent<NavMeshAgent>();
        anim.SetBool("Walking", true);
    }

    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if(!playerInVisionRadius && !playerInAttackingRadius)
        {
            Guard();
        }

        if(playerInVisionRadius && !playerInAttackingRadius)
        {
            PursuePlayer();
        }

        if(playerInVisionRadius && playerInAttackingRadius)
        {
            AttackPlayer();
        }
    }

    private void Guard()
    {
        if(Vector3.Distance(walkPoints[currentZombiePosition].transform.position, transform.position) < walkingPointRadius)
        {
            currentZombiePosition = Random.Range(0, walkPoints.Length);
            if(currentZombiePosition >= walkPoints.Length)
            {
                currentZombiePosition = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        transform.LookAt(walkPoints[currentZombiePosition].transform.position);
    }

    private void PursuePlayer()
    {
        if(zombieAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Die", false);
        }
        else
        {
            anim.SetBool("Walking", false);
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
                anim.SetBool("Walking", false);
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

        if(presentHealth <= 0)
        {
            anim.SetBool("Walking", false);
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
