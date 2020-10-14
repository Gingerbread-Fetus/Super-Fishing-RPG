using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerControls controls;
    PlayerObjectDetector objectDetector;
    Animator myAnimator;
    Rigidbody2D myRigidBody;

    float horizontal;
    float vertical;

    [SerializeField] float moveSpeed = 20.0f;
    private bool isCast = false;
    private float lastXDir;
    private float lastYDir;

    public IInteractable NearbyInteractable
    {
        get => objectDetector.NearbyInteractable;
    }

    private void OnEnable()
    {
        objectDetector = GetComponentInChildren<PlayerObjectDetector>();

        controls = new PlayerControls();
        controls.Player.Enable();

        controls.Player.Interact.performed += ctx => Interact_performed(ctx);

        controls.Player.CastRod.started += ctx => Cast_started(ctx);
        controls.Player.CastRod.performed += ctx => Cast_performed(ctx);
    }
    
    private void OnDisable()
    {
        controls.Player.Disable();
        controls.Player.Interact.performed -= Interact_performed;
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var lastMoveVector = controls.Player.Move.ReadValue<Vector2>();
        if (controls.Player.Move.triggered)
        {
            lastXDir = lastMoveVector.x;
            lastYDir = lastMoveVector.y;
        }
    }

    private void FixedUpdate()
    {
        if (isActiveAndEnabled && !isCast)
        {
            var moveVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
            myRigidBody.velocity = moveVector;

            myAnimator.SetFloat("XDir", moveVector.x);
            myAnimator.SetFloat("YDir", moveVector.y); 
        }
    }

    private void Interact_performed(InputAction.CallbackContext ctx)
    {
        if (NearbyInteractable != null)
        {
            NearbyInteractable.InteractingPlayer = this;
            NearbyInteractable.Interact();
        }
    }

    private void Cast_started(InputAction.CallbackContext ctx)
    {
        Debug.Log("Start cast");
        if (isCast)
        {
            myAnimator.SetBool("Cast", !isCast);
            isCast = !isCast;
        }
        else
        {
            isCast = !isCast;
        }
    }

    private void Cast_performed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Perform Cast");
        //play animation
        myAnimator.SetBool("Cast", isCast);

    }

    public void DisableControl()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void EnableControl()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
    }
}
