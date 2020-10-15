using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObjectDetector : MonoBehaviour
{
    IInteractable nearbyInteractable;

    public IInteractable NearbyInteractable { get => nearbyInteractable; set => nearbyInteractable = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        nearbyInteractable = collision.GetComponent<IInteractable>();
        if (nearbyInteractable != null)
        {
            nearbyInteractable.IsHighlighted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyInteractable != null)
        {
            nearbyInteractable.IsHighlighted = false;
            nearbyInteractable = null; 
        }
    }
}
