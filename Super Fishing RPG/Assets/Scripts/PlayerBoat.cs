using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoat : MonoBehaviour, IInteractable
{
    [SerializeField] Material selectedMaterial;
    [SerializeField] public float boatSpeed = 20.0f;

    PlayerControls controls;
    PlayerController interactingPlayer;
    Material defaultMaterial;
    SpriteRenderer spriteRenderer;
    Rigidbody2D myRigidBody;
    private Vector3 landPosition;
    private bool isValidDisembark;

    public PlayerController InteractingPlayer { get => interactingPlayer; set => interactingPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (controls == null)
        {
            controls = new PlayerControls(); 
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controls.Boat.enabled)
        {
            var moveVector = controls.Boat.Move.ReadValue<Vector2>() * (boatSpeed * Time.deltaTime);
            myRigidBody.velocity = moveVector; 
        }
    }
    
    public void Interact()
    {
        Debug.Log("Activate boat");
        //child it to this gameobject and undock the boat
        ActivateBoat();
    }

    public void Highlight(bool isSelected)
    {
        if (isSelected)
        {
            spriteRenderer.material = selectedMaterial;
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
        }
    }

    private void MoveToBoat()
    {
        myRigidBody = interactingPlayer.GetComponent<Rigidbody2D>();
        interactingPlayer.DisableControl();
        interactingPlayer.transform.position = gameObject.transform.position;
        gameObject.transform.parent = interactingPlayer.transform;
    }

    private void MoveToLand(Vector3 landingPosition)
    {
        interactingPlayer.EnableControl();
        gameObject.transform.parent = null;
        interactingPlayer.transform.position = landingPosition;
    }

    private void ActivateBoat()
    {
        MoveToBoat();
        EnableBoatControls();
    }
    
    private void DeactivateBoat()
    {
        DeactivateBoatControls();
    }

    private void DeactivateBoatControls()
    {
        controls.Boat.Disable();
    }

    private void EnableBoatControls()
    {
        controls.Boat.Enable();
    }

    private void Disembark_performed(InputAction.CallbackContext ctx)
    {
        DeactivateBoat();
        MoveToLand(landPosition);
        controls.Boat.Interact.performed -= Disembark_performed;
    }
            
    private void Disembark(Collider2D collision)
    {
        DeactivateBoat();
        MoveToLand(landPosition);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Disembark Area"))
        {
            controls.Boat.Interact.performed -= Disembark_performed;
            Debug.Log("exiting docking area");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Disembark Area"))
        {
            landPosition = collision.transform.position;
            controls.Boat.Interact.performed += Disembark_performed;
            Debug.Log("entering docking area");
        }
    }
}
