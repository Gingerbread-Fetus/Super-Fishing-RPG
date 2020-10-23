using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    [SerializeField] double acceptableDistance = .1f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] int verticalBound = 3;
    [SerializeField] int horizontalBound = 3;
    public float fishTimeout = 2;

    GameObject playerBobber = null;
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
        if((Vector3.Distance(transform.position, goalPosition) <= acceptableDistance) && playerBobber == null)
        {
            goalPosition = GetNextGoalPosition();
            Debug.DrawLine(transform.position, goalPosition, Color.red);
        }
        else
        {
            if ((Vector3.Distance(transform.position, goalPosition) <= acceptableDistance) && !isOnBobber)
            {
                isOnBobber = true;
                playerBobber.GetComponentInParent<PlayerController>().HookedFish = this;
                Animator playerAnimator = playerBobber.GetComponentInParent<Animator>();
                //Fish has hit the hook, trigger animator.
                playerAnimator.SetTrigger("HitBobber"); 
            }
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
        playerBobber = collider.gameObject;
        goalPosition = playerBobber.transform.position;
    }

    public void Escape()
    {
        Animator playerAnimator = playerBobber.GetComponentInParent<Animator>();
        playerAnimator.SetTrigger("FishTimeout");
        isOnBobber = false;
        playerBobber = null;
        Debug.Log("Fish got away!");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Change direction if fish collides with ground.
        if (collision.gameObject.CompareTag("Ground"))
        {
            goalPosition = GetNextGoalPosition();
        }
    }

    internal void StartTimeout()
    {
        StartCoroutine(EscapeTimeout());
    }

    private IEnumerator EscapeTimeout()
    {
        yield return new WaitForSeconds(fishTimeout);
        Escape();
    }
}
