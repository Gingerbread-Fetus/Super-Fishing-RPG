using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAimReticule : MonoBehaviour
{
    bool playerIsCasting;
    bool validCastLocation = false;
    [SerializeField] float reticuleMoveSpeed = 1.0f;
    [SerializeField] TilemapCollider2D tileMap;

    public bool ValidCastLocation { get => validCastLocation;}

    private void Update() 
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("In collider: " + other.name);
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
}