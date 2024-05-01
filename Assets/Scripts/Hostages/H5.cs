using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class H5 : MonoBehaviour
{

    public NavMeshAgent hostageAgent;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    public float stopRunningRadius;
    public bool waitingRadius;
    public float hostageSpeed = 1.7f;
    public Animator anim;

    public static H5 h5;

    public void Awake()
    {
        hostageAgent = GetComponent<NavMeshAgent>();
        h5 = this;
        //anim.SetBool("Peeking", true);
    }

    private void Update()
    {
        waitingRadius = Physics.CheckSphere(transform.position, stopRunningRadius, PlayerLayer);

        if(!Objective2.objective2.isCompleted2)
        {
            Peeking();
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

    private void Peeking()
    {
        anim.SetBool("Peeking", true);
    }

    private void FollowPlayer()
    {
        if(hostageAgent.SetDestination(playerBody.position))
        {
            anim.SetBool("Running", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Peeking", false);
        }
        if (VehicleController.instance.isOpened)
        {
            hostageAgent.SetDestination(VehicleController.instance.vehicleDoor4.position);
        }
    }

    private void WaitPlayer()
    {
        hostageAgent.SetDestination(transform.position);
        anim.SetBool("Peeking", false);
        anim.SetBool("Running", false);
        anim.SetBool("Idle", true);
    }
}
