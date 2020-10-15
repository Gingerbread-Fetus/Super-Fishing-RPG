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
    PlayerAimReticule aimReticule;
    Vector2 moveVector;
    Animator myAnimator;
    Rigidbody2D myRigidBody;

    float horizontal;
    float vertical;

    [SerializeField] float moveSpeed = 20.0f;
    private bool isCasting = false;
    private float lastXDir;
    private float lastYDir;

    public IInteractable NearbyInteractable { get => objectDetector.NearbyInteractable;}

    private void OnEnable()
    {
        objectDetector = GetComponentInChildren<PlayerObjectDetector>();
        aimReticule = GetComponentInChildren<PlayerAimReticule>(true);
        aimReticule.gameObject.SetActive(false);

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
        aimReticule.MoveReticule(moveVector);
    }

    private void FixedUpdate()
    {
        moveVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
        if (!isCasting)
        {
            myRigidBody.velocity = moveVector;
        }

        myAnimator.SetFloat("XDir", moveVector.x);
        myAnimator.SetFloat("YDir", moveVector.y);
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
        if (isCasting)
        {
            isCasting = !isCasting;
            myAnimator.SetTrigger("Hook");
        }
        else
        {
            aimReticule.gameObject.SetActive(true);
            isCasting = !isCasting;
            myAnimator.SetBool("Cast", isCasting);
        }
    }

    private void Cast_performed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Perform Cast");
        isCasting = !isCasting;
        myAnimator.SetBool("Cast", isCasting);
        aimReticule.Reset();
        aimReticule.gameObject.SetActive(false);
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
