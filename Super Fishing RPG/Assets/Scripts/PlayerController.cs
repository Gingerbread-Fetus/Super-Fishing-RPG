using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public IInteractable NearbyInteractable
    {
        get => objectDetector.NearbyInteractable;
    }

    private void OnEnable()
    {
        objectDetector = GetComponentInChildren<PlayerObjectDetector>();

        controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Interact.performed += Interact_performed;
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
    
    private void FixedUpdate()
    {
        if (isActiveAndEnabled)
        {
            var moveVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
            myRigidBody.velocity = moveVector;

            myAnimator.SetFloat("XDir", moveVector.x);
            myAnimator.SetFloat("YDir", moveVector.y); 
        }
    }

    private void Interact_performed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Nearby interactable: " + NearbyInteractable);
        if (NearbyInteractable != null)
        {
            NearbyInteractable.InteractingPlayer = this;
            NearbyInteractable.Interact();
        }
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
