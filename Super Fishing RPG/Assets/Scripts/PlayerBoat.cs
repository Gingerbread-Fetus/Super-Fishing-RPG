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
    bool isDocked = true;

    public PlayerController InteractingPlayer { get => interactingPlayer; set => interactingPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDocked)
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

    private void ActivateBoat()
    {
        MoveToBoat();

        SetupBoatControls();
    }
    
    private void DeactivateBoat()
    {

        isDocked = true;

        DeactivateBoatControls();
    }

    private void DeactivateBoatControls()
    {
        controls.Boat.Disable();
    }

    private void SetupBoatControls()
    {
        controls = new PlayerControls();
        controls.Boat.Enable();
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
        interactingPlayer.GetComponent<CapsuleCollider2D>().enabled = false;
        interactingPlayer.transform.position = gameObject.transform.position;
        gameObject.transform.parent = interactingPlayer.transform;
        interactingPlayer.enabled = false;
        isDocked = false;
    }

    private void MoveToLand(Vector3 landingPosition)
    {
        interactingPlayer.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.transform.parent = null;
        interactingPlayer.enabled = true;
        interactingPlayer.transform.position = landingPosition;
        isDocked = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDocked)
        {
            //Move to land?
            Vector3 landDirection = (collision.GetContact(0).point);
            Debug.DrawLine(interactingPlayer.transform.position, landDirection, Color.red, 4.0f);
            RaycastHit2D raycastHit2D = Physics2D.Raycast
                (interactingPlayer.transform.position,
                landDirection,
                .5f,
                LayerMask.GetMask("Ground"));
            var contactPoint = raycastHit2D.point;
            Debug.DrawLine(interactingPlayer.transform.position, contactPoint, Color.white, 4.0f);
            var landPosition = contactPoint;

            DeactivateBoat();

            MoveToLand(landPosition);
        }
    }
}
