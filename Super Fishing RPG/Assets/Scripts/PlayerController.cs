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
    private bool isCast = false;
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
        aimReticule.gameObject.SetActive(isCasting);
        if (isCasting)
        {
            var reticuleVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
            aimReticule.MoveReticule(reticuleVector); 
        }
    }

    private void FixedUpdate()
    {
        if (!isCasting && !isCast)
        {
            moveVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
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
        if (!isCast)
        {
            Debug.Log("Start cast");
            myAnimator.SetTrigger("StartCast");
            isCasting = true; 
        }
        else
        {
            Debug.Log("Hook and reel in");
            isCast = false;
            myAnimator.SetTrigger("Hook");
            myAnimator.SetBool("Cast", isCast);
        }
    }

    private void Cast_performed(InputAction.CallbackContext ctx)
    {
        if (isCasting)
        {
            Debug.Log("Perform Cast");
            isCasting = false;
            isCast = true;
            myAnimator.SetBool("Cast", isCast);
            aimReticule.transform.position = transform.position;
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
