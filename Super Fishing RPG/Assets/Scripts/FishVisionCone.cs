using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishVisionCone : MonoBehaviour
{
    FishController fishController;
    // Start is called before the first frame update
    void Start()
    {
        fishController = GetComponentInParent<FishController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bobber"))
        {
            Debug.Log(collision.gameObject.name + " in collider");
            fishController.BobberDetected(collision);
        }
    }

}
