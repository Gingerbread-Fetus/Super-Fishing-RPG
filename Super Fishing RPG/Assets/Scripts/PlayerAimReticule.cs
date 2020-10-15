using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimReticule : MonoBehaviour
{
    bool playerIsCasting;
    [SerializeField] float reticuleMoveSpeed = 1.0f;
    
    public void MoveReticule(Vector2 moveVector)
    {
        if (isActiveAndEnabled)
        {
            transform.position = transform.position + (Vector3)(moveVector * reticuleMoveSpeed * Time.deltaTime); 
        }
    }

    public void Reset()
    {
        transform.position = transform.parent.transform.position;
    }
}
