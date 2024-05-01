using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 2.7f;
    public float playerSprint = 4.2f;

    [Header("Player Health Things")]
    private float playerHealth = 120f;
    public float presentHealth;
    public GameObject playerDamage;
    public HealthBar HealthBar;

    [Header("Player Script Cameras")]
    public Transform playerCamera;
    public GameObject EndGameMenuUI;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    
    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    bool isRunning = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        HealthBar.GiveFullHealth(playerHealth);
    }

    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y < 0) velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

        playerMove();
        Jump();
        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        {
            Vector3 cameraForward = playerCamera.forward;
            Vector3 cameraUp = playerCamera.up;

            float pitchRotationWeight = 0.4f;

            Vector3 blendedDirection = Vector3.Slerp(transform.forward, cameraForward.normalized, pitchRotationWeight);
            Vector3 blendedUp = Vector3.Slerp(transform.up, cameraUp.normalized, pitchRotationWeight);

            transform.LookAt(transform.position + blendedDirection, blendedUp);
        }

        if(direction.magnitude >= 0.1f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("IdleAim", false);
            animator.SetBool("Jump", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) *  Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurface && !isRunning)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Running", false);
            animator.SetTrigger("Jump");

            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if(Input.GetButton("Sprint") && onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");
            isRunning = true;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if(direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) *  Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);

                if(Input.GetButtonDown("Jump") && isRunning)
                {
                    animator.SetTrigger("Running Jumping");
                    velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
                }
                else
                {
                    animator.ResetTrigger("Running Jumping");
                    animator.SetBool("Running", true);
                }
            }
            else
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
            }
        }
        else
        {
            isRunning = false;
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        StartCoroutine(PlayerDamage());

        HealthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        EndGameMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }

    IEnumerator PlayerDamage()
    {
        playerDamage.SetActive(true);
        yield return new WaitForSeconds(2f);
        playerDamage.SetActive(false);
    }
}