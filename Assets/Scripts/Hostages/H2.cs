using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class H2 : MonoBehaviour
{
    public NavMeshAgent hostageAgent;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public float stopRunningRadius;
    public bool waitingRadius;
    public float hostageSpeed = 1.7f;
    public Animator anim;

    public static H2 h2;

    public void Awake()
    {
        hostageAgent = GetComponent<NavMeshAgent>();
        h2 = this;
        //anim.SetBool("Arguing", true);
    }

    private void Update()
    {
        waitingRadius = Physics.CheckSphere(transform.position, stopRunningRadius, PlayerLayer);

        if(!Objective2.objective2.isCompleted2)
        {
            Arguing();
        }

        if(Objective2.objective2.isCompleted2 && !waitingRadius)
        {
            FollowPlayer();
            Debug.Log("follow");
        }

        if(Objective2.objective2.isCompleted2 && waitingRadius)
        {
            WaitPlayer();
            Debug.Log("Waiting radius");
        }
    }

    private void Arguing()
    {
        anim.SetBool("Arguing", true);
    }

    private void FollowPlayer()
    {
        if(hostageAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Running", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Arguing", false);
        }
        if (VehicleController.instance.isOpened)
        {
            hostageAgent.SetDestination(VehicleController.instance.vehicleDoor3.position);
        }
    }

    private void WaitPlayer()
    {
        hostageAgent.SetDestination(transform.position);
        anim.SetBool("Arguing", false);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", true);
    }
}
