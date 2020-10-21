using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAimReticule : MonoBehaviour
{
    bool playerIsCasting;
    bool validCastLocation = false;
    CircleCollider2D circleCollider = default;
    [SerializeField] float reticuleMoveSpeed = 1.0f;
    [SerializeField] TilemapCollider2D tileMap;

    public bool ValidCastLocation { get => validCastLocation;}

    void Update()
    {
        Debug.Log("Collider is touching ground? : " + circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ground Collision")));
    }

    void OnEnable()
    {
        if(circleCollider == null) { circleCollider = GetComponent<CircleCollider2D>(); }
    }
    
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

    public bool IsValidCastingLocation()
    {
        return !circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Ground Collision"));
    }
}