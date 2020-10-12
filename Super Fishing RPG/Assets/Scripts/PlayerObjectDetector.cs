using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObjectDetector : MonoBehaviour
{
    IInteractable nearbyInteractable;

    public IInteractable NearbyInteractable { get => nearbyInteractable; set => nearbyInteractable = value; }

    public void Disable()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void Enable()
    {
        GetComponent<CircleCollider2D>().enabled = true;
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
