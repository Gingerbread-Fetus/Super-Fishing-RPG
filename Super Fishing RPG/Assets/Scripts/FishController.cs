using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    [SerializeField] double acceptableDistance = .1f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] int verticalBound = 3;
    [SerializeField] int horizontalBound = 3;

    Rigidbody2D myRigidBody;
    Tilemap waterTileMap;
    Vector3 startPosition;
    Vector3 goalPosition;
    private bool isOnBobber = false;

    private void Start()
    {
        startPosition = transform.position;
        myRigidBody = GetComponent<Rigidbody2D>();
        waterTileMap = GetComponentInParent<Tilemap>();
        goalPosition = GetNextGoalPosition();
    }

    private void FixedUpdate()
    {
        //Movement here.
        if((Vector3.Distance(transform.position, goalPosition) <= acceptableDistance) && !isOnBobber)
        {
            goalPosition = GetNextGoalPosition();
            Debug.DrawLine(transform.position, goalPosition, Color.red);
        }
        float step = moveSpeed * Time.deltaTime;
        myRigidBody.position = Vector3.MoveTowards(myRigidBody.position, goalPosition, step);

        //Debug.Log("Goal Position: " + goalPosition);
    }

    private Vector3 GetNextGoalPosition()
    {
        Vector3Int startAsInt = waterTileMap.WorldToCell(startPosition);
        int randX = Random.Range(startAsInt.x - horizontalBound, startAsInt.x + horizontalBound);
        int randY = Random.Range(startAsInt.y - verticalBound, startAsInt.y + verticalBound);
        Vector3Int GoalAsInt = new Vector3Int(randX, randY, 0);
        return waterTileMap.CellToLocal(GoalAsInt);
    }

    public void BobberDetected(Collider2D collider)
    {
        goalPosition = collider.transform.position;
        isOnBobber = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Fish collided with ground");
            goalPosition = GetNextGoalPosition();
        }
    }
}
