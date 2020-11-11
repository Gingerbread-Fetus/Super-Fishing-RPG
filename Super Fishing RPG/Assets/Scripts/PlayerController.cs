using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20.0f;
    //[SerializeField] GameObject rodObject = default;
    [SerializeField] GameObject bobberObject = default;
    [SerializeField] GameObject battleOverlay = default;
    bool fishOnLine = false;

    PlayerInput playerInput;
    PlayerControls controls;
    PlayerObjectDetector objectDetector;
    PlayerAimReticule aimReticule;
    FishController hookedFish;
    Vector3 castHeading;
    Vector2 moveVector;
    Animator myAnimator;
    Rigidbody2D myRigidBody;

    private bool isCasting = false;
    private bool isCast = false;
    private float lastXDir;
    private float lastYDir;

    public IInteractable NearbyInteractable { get => objectDetector.NearbyInteractable;}
    public FishController HookedFish { get => hookedFish; set => hookedFish = value; }
    public bool FishOnLine { get => fishOnLine; set => fishOnLine = value; }

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
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        if (battleOverlay.activeInHierarchy) { battleOverlay.SetActive(false); }
    }

    private void Update()
    {
        CheckCasting();
    }
        
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!isCasting && !isCast)
        {
            moveVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
            myRigidBody.velocity = moveVector;

            myAnimator.SetFloat("XDir", moveVector.x);
            myAnimator.SetFloat("YDir", moveVector.y);
        }
    }

    private void CheckCasting()
    {
        aimReticule.gameObject.SetActive(isCasting);
        if (isCasting)
        {
            myRigidBody.velocity = new Vector2();
            var reticuleVector = controls.Player.Move.ReadValue<Vector2>() * (moveSpeed * Time.deltaTime);
            aimReticule.MoveReticule(reticuleVector);
            SetCastFacing();
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
        //Start cast
        if (!isCast)
        {
            myAnimator.SetTrigger("StartCast");
            isCasting = true; 
        }
        //Hook and reel in.
        else
        {
            //Reset the animator states.
            isCast = false;
            myAnimator.SetTrigger("Hook");
            myAnimator.SetBool("HitBobber", false);
            myAnimator.SetBool("Cast", isCast);
            CatchFish();
            bobberObject.transform.position = transform.position;
            bobberObject.SetActive(false);
        }
    }
    
    private void Cast_performed(InputAction.CallbackContext ctx)
    {
        if (isCasting)
        {
            isCasting = false;
            isCast = true;
            myAnimator.SetFloat("CastHeadingX", castHeading.x);
            myAnimator.SetFloat("CastHeadingY", castHeading.y);
            if(aimReticule.IsValidCastingLocation())
            {
                myAnimator.SetBool("Cast", isCast);
                bobberObject.SetActive(true);
                bobberObject.transform.position = aimReticule.transform.position;
            }
            else
            {
                myAnimator.SetTrigger("CancelCast");
                isCast = false;
            }
            aimReticule.Reset();
        }
    }

    private void SetCastFacing()
    {
        castHeading = aimReticule.transform.position - transform.position;
        myAnimator.SetFloat("CastHeadingX", castHeading.x);
        myAnimator.SetFloat("CastHeadingY", castHeading.y);
    }

    private void CatchFish()
    {
        if(hookedFish != null)
        {
            //TODO Handle putting fish in inventory here?
            Debug.Log("fish caught!");
            hookedFish.LocalHatchery.RemoveFish(hookedFish);
            fishOnLine = false;
            Destroy(hookedFish.gameObject);
            hookedFish = null;
            StartBattle();
        }
    }

    private void StartBattle()
    {
        Debug.Log("Starting Battle");
        battleOverlay.SetActive(true);
        //TODO Animations for polish
        //TODO Make sure control is disabled
    }

    public void DisableControl()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void EnableControl()
    {
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public void StartFishTimeout()
    {
        Debug.Log("Starting fish timeout");
        Debug.Log("Hooked fish: " + hookedFish.name);
        if (hookedFish != null)
        {
            hookedFish.StartTimeout(); 
        }
    }
}
