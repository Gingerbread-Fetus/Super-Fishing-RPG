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
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    LayerMask interactableLayerMask;
    IInteractable nearbyInteractable;

    float horizontal;
    float vertical;

    [SerializeField] float moveSpeed = 20.0f;

    public IInteractable NearbyInteractable { get => nearbyInteractable; set => nearbyInteractable = value; }

    private void OnEnable()
    {
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
        interactableLayerMask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
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
        if (nearbyInteractable != null)
        {
            nearbyInteractable.InteractingPlayer = this;
            nearbyInteractable.Interact();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if (nearbyInteractable == null)
            {
                nearbyInteractable = collision.GetComponent<IInteractable>();
                nearbyInteractable.Highlight(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            nearbyInteractable.Highlight(false);
            nearbyInteractable = null;
        }
    }
}
