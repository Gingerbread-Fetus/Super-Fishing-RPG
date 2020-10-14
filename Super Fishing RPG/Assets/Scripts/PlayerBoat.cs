using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//TODO: Consider making a vehicle interface for some of these methods.
public class PlayerBoat : MonoBehaviour, IInteractable, IVehicle
{
    [SerializeField] Material selectedMaterial;
    [SerializeField] public float boatSpeed = 20.0f;
    public bool isBoarded;

    PlayerController interactingPlayer;
    Material defaultMaterial;
    SpriteRenderer spriteRenderer;
    private Vector3 landPosition;
    private bool isValidDisembark;
    bool isHighlighted = false;

    IInteractable nearbyInteractable = null;
    public PlayerController InteractingPlayer { get => interactingPlayer; set => interactingPlayer = value; }
    public IInteractable NearbyInteractable { get => nearbyInteractable; set => nearbyInteractable = value; }
    public bool IsHighlighted
    {
        get => isHighlighted;
        set
        {
            if(isHighlighted == value) { return; }
            isHighlighted = value;
            Highlight(isHighlighted);
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }
        
    //This objects' 'interact with' method
    public void Interact()
    {
        MoveToBoat();
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

    public void Disembark(Vector3 landPosition)
    {
        MoveToLand(landPosition);
    }
}
