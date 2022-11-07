using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // For optimization   
    private readonly int MOVE_AMOUNT_HASH = Animator.StringToHash("moveAmount");
    private const float ANIM_DAMP_TIME = 0.2f;

    [Header("Player Move Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;
   
    [field: SerializeField] public InputReader InputReader { get; private set; }

    private CameraController cameraController;
    private Quaternion targetRotation;

    private Animator animator;
    private CharacterController characterController;

    private bool isGrounded;
    private bool hasControl = true;

    private float ySpeed;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        #region Old Input System
        /*    
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        */
        #endregion

        float h = InputReader.MovementValue.x;
        float v = InputReader.MovementValue.y;
    
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        Vector3 moveInput = (new Vector3(h, 0, v)).normalized;
        Vector3 moveDir = cameraController.PlanarRotation * moveInput;

        if (!hasControl)
            return;        

        GroundCheck();    

        if (isGrounded)        
            ySpeed = -0.5f;        
        else        
            ySpeed += Physics.gravity.y * Time.deltaTime;
        
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        //transform.position += moveDir * moveSpeed * Time.deltaTime;  
        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {                            
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat(MOVE_AMOUNT_HASH, moveAmount, ANIM_DAMP_TIME, Time.deltaTime);
    }

    // To alternate a bug of chracterController.isGrounded
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public void SetControl(bool hasControl)
    {
        this.hasControl = hasControl;
        characterController.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat(MOVE_AMOUNT_HASH, 0f);
            targetRotation = transform.rotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.8f); // Green + alpha value
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
