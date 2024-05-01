using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Wheels Colliders")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform vehicleDoor;
    public Transform vehicleDoor2;
    public Transform vehicleDoor3;
    public Transform vehicleDoor4;

    [Header("Vehicle Engine")]
    public float accelerationForce = 100f;
    public float breakingForce = 200f;
    private float presentBreakForce = 0f;
    public float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    public float wheelsTorque = 20f;
    private float presentTurnAngle = 0f;

    [Header("Vehicle Security")]
    public PlayerScript player;
    public H1 hostage1;
    public H2 hostage2;
    public H5 hostage5;
    private float radius = 5f;
    public bool isOpened = false;

    [Header("Disable Things")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public GameObject PlayerCharacter;
    public GameObject Hostage1;
    public GameObject Hostage2;
    public GameObject Hostage5;

    [Header("Vehicle Hit Var")]
    public Camera cam;
    public float hitRange = 100f;
    private float giveDamageOf = 100f;
    public GameObject goreEffect;
    public GameObject DestroyEffect;

    public static VehicleController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(isOpened == false)
                {
                    isOpened = true;
                    radius = 5000f;
                    ObjectivesComplete.occurence.GetObjectivesDone3(true);
                }
                else
                {
                    player.transform.position = vehicleDoor.transform.position;
                    hostage1.transform.position = vehicleDoor2.transform.position;
                    hostage2.transform.position = vehicleDoor3.transform.position;
                    hostage5.transform.position = vehicleDoor4.transform.position;
                    isOpened = false;
                    radius = 5f;
                }
            }
        }

        if(isOpened == true)
        {
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            PlayerCharacter.SetActive(false);
            if(H1.h1.hostageAgent.remainingDistance <= H1.h1.hostageAgent.stoppingDistance)
            {
                H1.h1.anim.SetBool("Running", false);
                H1.h1.anim.SetBool("Idle", true);
                H1.h1.anim.SetBool("Yelling", false);
                Hostage1.SetActive(false);
            }
            if(H2.h2.hostageAgent.remainingDistance <= H2.h2.hostageAgent.stoppingDistance)
            {
                H2.h2.anim.SetBool("Running", false);
                H2.h2.anim.SetBool("Idle", true);
                H2.h2.anim.SetBool("Yelling", false);
                Hostage2.SetActive(false);
            }
            if(H5.h5.hostageAgent.remainingDistance <= H5.h5.hostageAgent.stoppingDistance)
            {
                H5.h5.anim.SetBool("Running", false);
                H5.h5.anim.SetBool("Idle", true);
                H5.h5.anim.SetBool("Yelling", false);
                Hostage5.SetActive(false);
            }

            MoveVehicle();
            VehicleSteering();
            ApplyBreaks();
            HitZombies();
        }
        else if(isOpened == false)
        {
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            PlayerCharacter.SetActive(true);
            Hostage1.SetActive(true);
            Hostage2.SetActive(true);
            Hostage5.SetActive(true);
        }
    }

    void MoveVehicle()
    {
        frontRightWheelCollider.motorTorque = presentAcceleration;
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;

        presentAcceleration = accelerationForce * - Input.GetAxis("Vertical");
    }

    void VehicleSteering()
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;

        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position;
        WT.rotation = rotation;
    }

    void ApplyBreaks()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            presentBreakForce = breakingForce;

        }
        else presentBreakForce = 0f;

        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
    }

    void HitZombies()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, hitRange))
        {
            Debug.Log(hitInfo.transform.name);

            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if(zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                zombie1.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if(zombie2 != null)
            {
                zombie2.zombieHitDamage(giveDamageOf);
                zombie2.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if(objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                GameObject impactGo = Instantiate(DestroyEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 1f);
            }
        }
    }
}
