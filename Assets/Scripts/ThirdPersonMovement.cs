using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    PlayerControls controls;

    public Transform cam;

    public float walkSpeed = 1.7f;
    public float runSpeed = 5f;
    public float turnSmoothTime = 0.1f;

    public Transform groundCheck;
    public LayerMask groundMask;
    public float gravity = -18f;
    public float groundDistance = 0.01f;
    public float jumpHeight = 1f;
    public float neutralVerticalVelocity = 0f;

    private float vertical;
    private float horizontal;
    private float speed;
    private float turnSmoothVelocity;
    private float targetAngle;
    private float angle;

    private Vector2 inputDirection;
    private Vector3 direction;
    private Vector3 moveDirection;
    private Vector3 moveDirectionNormalized;
    private Vector3 velocity; // Vertical velocity

    // Character status
    private bool running;

    // Character event actions
    private bool toJump;

    void Awake()
    {
        controls = new PlayerControls();

        // Control events binding
        controls.Gameplay.Jump.performed += ctx => toJump = ctx.ReadValueAsButton();
        controls.Gameplay.Run.performed += ctx => running = ctx.ReadValueAsButton();
        controls.Gameplay.Move.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (toJump) jump();
        toJump = false;

        move();

        applyGravity();
        setAnimation(); 
    }

    // Applies horizontal movement.
    void move()
    {
        // direction vector will determine if there is movement
        direction = new Vector3(inputDirection.x, 0f, inputDirection.y);

        if (running) speed = runSpeed;
        else speed = walkSpeed;

        if (isMoving())
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirectionNormalized = moveDirection.normalized * speed;

            controller.Move(moveDirectionNormalized * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    // Perform a jump
    void jump()
    {
        if (!isJumping() && isGrounded()) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            controller.Move(velocity * Time.deltaTime);
            animator.SetTrigger("jump");
        }
    }

    // Applies Gravity
    void applyGravity() {
        // Reset falling velocity when touch the ground.
        if (isGrounded() && velocity.y > 0)
            velocity.y = neutralVerticalVelocity;

        // Calculates and applies gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Return if character is moving.
    bool isMoving() {
        return (direction.magnitude >= 0.1f);
    }

    // Return if character is in the ground.
    bool isGrounded() {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    // Return if character is falling.
    bool isFalling() {
        return (!isGrounded() && velocity.y < neutralVerticalVelocity);
    }

    // Return if the character is jumping
    bool isJumping() {
        return (!isGrounded() && velocity.y > neutralVerticalVelocity);
    }

    // Set animation variables.
    void setAnimation()
    {
        animator.SetBool("walking", isMoving());

        animator.SetBool("running", (isMoving() && running));

        if (toJump)
            animator.SetTrigger("jump");
    }
}
