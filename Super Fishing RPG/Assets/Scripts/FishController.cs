using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class FishController : MonoBehaviour
{
    public Hatchery LocalHatchery { get => localHatchery; set => localHatchery = value; }

    [SerializeField] double acceptableDistance = .1f;
    [SerializeField] float moveSpeed = 1.0f;
    public float fishTimeout = 2;

    GameObject playerBobber = null;
    Rigidbody2D myRigidBody;
    Vector3 startPosition;
    Vector3 goalPosition;
    Hatchery localHatchery;
    Bounds hatcheryBounds;
    private bool isOnBobber = false;


    private void Start()
    {
        startPosition = transform.position;
        myRigidBody = GetComponent<Rigidbody2D>();
        goalPosition = GetNextGoalPosition();
        //Debug.Log("Starting Goal Position: " + goalPosition);
    }

    private void Update()
    {
        //GetNextGoalPosition();
    }

    private void FixedUpdate()
    {
        //Movement here.
        if((Vector3.Distance(transform.position, goalPosition) <= acceptableDistance) && playerBobber == null)
        {
            goalPosition = GetNextGoalPosition();
            //Debug.Log("Goal Position: " + goalPosition);
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
                playerAnimator.SetBool("HitBobber", true); 
            }
        }
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, goalPosition, step);
    }

    private Vector3 GetNextGoalPosition()
    {
        float randX = Random.Range
            (
                localHatchery.bounds.min.x,
                localHatchery.bounds.max.x
            );
        float randY = Random.Range
            (
                localHatchery.bounds.min.y,
                localHatchery.bounds.max.y
            );
        Vector3 nextGoalPosition = new Vector3(randX, randY, 0);
        Debug.DrawLine(nextGoalPosition, nextGoalPosition + Vector3.right/6.0f, Color.white, 3.0f);
        return nextGoalPosition;
    }

    public void BobberDetected(Collider2D collider)
    {
        playerBobber = collider.gameObject;
        goalPosition = playerBobber.transform.position;
    }

    public void Escape()
    {
        Animator playerAnimator = playerBobber.GetComponentInParent<Animator>();
        PlayerController playerController = playerBobber.GetComponentInParent<PlayerController>();
        playerAnimator.SetBool("HitBobber", false);
        playerController.FishOnLine = false;
        playerController.HookedFish = null;
        isOnBobber = false;
        playerBobber = null;
        StartCoroutine(CollisionCooldown());
        Debug.Log("Fish got away!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Change direction if fish collides with ground.
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Fish hit ground");
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
    private IEnumerator CollisionCooldown()
    {
        GameObject visionCone = GetComponentInChildren<FishVisionCone>().gameObject;
        visionCone.SetActive(false);
        yield return new WaitForSeconds(5.0f);
        visionCone.gameObject.SetActive(true);
    }
}
