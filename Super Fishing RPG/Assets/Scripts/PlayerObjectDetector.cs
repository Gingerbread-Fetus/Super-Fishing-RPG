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
        Debug.Log("Object detector hit: " + collision);
        if (nearbyInteractable != null)
        {
            //Debug.Log("nearbyInteractable is: " + nearbyInteractable);
            nearbyInteractable.IsHighlighted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyInteractable != null)
        {
            //Debug.Log("nearbyInteractable is no longer: " + nearbyInteractable);
            nearbyInteractable.IsHighlighted = false;
            nearbyInteractable = null; 
        }
    }
}
