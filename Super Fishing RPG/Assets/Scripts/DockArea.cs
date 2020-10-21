using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockArea : MonoBehaviour, IInteractable
{
    PlayerController interactingPlayer = null;
    IVehicle dockingVehicle = null;
    bool isHighlighted;
    public PlayerController InteractingPlayer { get => interactingPlayer; set => interactingPlayer = value; }
    public bool IsHighlighted
    {
        get => isHighlighted;
        set
        {
            if (isHighlighted == value) { return; }
            isHighlighted = value;
            Highlight(isHighlighted);
        }
    }

    private void Highlight(bool isSelected)
    {
        //TODO Not implemented meaningfully yet.
    }

    public void Interact()
    {
        if (interactingPlayer != null)
        {
            dockingVehicle = interactingPlayer.GetComponentInChildren<IVehicle>();
            if (dockingVehicle != null)
            {
                dockingVehicle.Disembark(gameObject.transform.position);
            } 
        }
    }
}
