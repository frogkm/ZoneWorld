using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDragStanding;
    [SerializeField] private float groundDragMoving;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float groundedCooldown;
    [SerializeField] private float inAirMultiplier;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private BoxCollider bodyCollider;
    [SerializeField] private Transform orientation;
    //[SerializeField] private Animator playerAnimator;


    private bool grounded;
    private bool ready_to_jump;
    private bool ready_to_ground;


    private float horizontal_input;
    private float vertical_input;

    private Vector3 move_direction;
    private Rigidbody rb;

    private bool movementLocked = false;

    private void OnCollisionEnter(Collision other) {
        //Debug.Log(other.gameObject.tag);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ready_to_jump = true;
        ready_to_ground = true;
    }

    private void Update()
    {
   
        // ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (ready_to_ground)
            grounded = Physics.BoxCast(groundChecker.position + new Vector3(0, bodyCollider.size.y / 8, 0), new Vector3(bodyCollider.size.x / 2.2f, bodyCollider.size.y / 8, bodyCollider.size.z / 2.2f), Vector3.down, Quaternion.identity, 0.05f, whatIsGround);


        //Debug.Log(grounded);


        if (!movementLocked) {
            MyInput();
        }
        
        SpeedControl();

        // handle drag
        
        if (grounded) {
            if (horizontal_input != 0 || vertical_input != 0) {
                rb.drag = groundDragMoving;
            }
            else {
                rb.drag = groundDragStanding;
            }
        }
        else {
            rb.drag = 0;
        }      
    }


    private void FixedUpdate()
    {
        if (!movementLocked) {
            MovePlayer();  
        }

    }

    private void MyInput()
    {
        horizontal_input = Input.GetAxisRaw("Horizontal");
        vertical_input = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && ready_to_jump && grounded)
        {
            ready_to_jump = false;
            ready_to_ground = false;
            grounded = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            Invoke(nameof(ResetGrounded), groundedCooldown);
            //playerAnimator.SetBool("jumping", true);

        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        move_direction = orientation.forward * vertical_input + orientation.right * horizontal_input;

        // on ground
        if(grounded)
            rb.AddForce(move_direction.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(move_direction.normalized * moveSpeed * 10f * inAirMultiplier, ForceMode.Force);
    }

    public void LockMovement() {
        movementLocked = true;
    }

    public void UnlockMovement() {
        movementLocked = false;
    }

    private void SpeedControl()
    {
        Vector3 flat_vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flat_vel.magnitude > moveSpeed)
        {
            Vector3 limited_vel = flat_vel.normalized * moveSpeed;
            rb.velocity = new Vector3(limited_vel.x, rb.velocity.y, limited_vel.z);
        }
    }

    private void Jump()
    {
        rb.drag = 0;
        grounded = false;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    private void ResetJump()
    {
        ready_to_jump = true;
    }

    private void ResetGrounded()
    {
        ready_to_ground = true;
    }


}