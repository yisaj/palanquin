using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverController : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    public BoxController box;
    public MoverController otherMover;
    public GameController game;

    new private Rigidbody rigidbody;
    private Vector3 direction = Vector3.zero;
    public float stepForce = 0.5f;
    public float stepTime = 0.2f;
    private float sinceLastStep = 0f;
    private Vector3 moveForce = Vector3.zero;
    public float drag = 2f;
    public float maxSpeed = 1f;
    public float pullSpeed = 7f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // On keydown, immediately impulse
        Vector3 dir = Vector3.zero;
        if (Input.GetKeyDown(up))
        {
            dir += Vector3.forward;
            sinceLastStep = 0f;
        }
        if (Input.GetKeyDown(down))
        {
            dir += Vector3.back;
            sinceLastStep = 0f;
        }
        if (Input.GetKeyDown(left))
        {
            dir += Vector3.left;
            sinceLastStep = 0f;
        }
        if (Input.GetKeyDown(right))
        {
            dir += Vector3.right;
            sinceLastStep = 0f;
        }
        moveForce += Vector3.Normalize(dir) * stepForce;
        dir = Vector3.zero;

        // Check if a key is being held down
        if (Input.GetKey(up) ||
            Input.GetKey(down) ||
            Input.GetKey(left) ||
            Input.GetKey(right))
        {
            sinceLastStep += Time.deltaTime;
            if (sinceLastStep > stepTime)
            {
                sinceLastStep = 0f;
                if (Input.GetKey(up))
                {
                    dir += Vector3.forward;
                }
                if (Input.GetKey(down))
                {
                    dir += Vector3.back;
                }
                if (Input.GetKey(left))
                {
                    dir += Vector3.left;
                }
                if (Input.GetKey(right))
                {
                    dir += Vector3.right;
                }
                Vector3.Normalize(dir);
                moveForce += dir * stepForce;

                dir = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if (game.gameIsOver)
        {
            return;
        }

        if (moveForce.magnitude > maxSpeed)
        {
            moveForce = Vector3.Normalize(moveForce) * maxSpeed;
        }

        transform.position += moveForce;
        Vector3 gap = otherMover.transform.position - transform.position;

        float minDiff = gap.magnitude - box.squareSide;
        float maxDiff = box.maxLength - gap.magnitude;
        if (minDiff < 0)
        {
            gap = Vector3.Normalize(gap);
            Vector3 parallel = gap * Vector3.Dot(gap, moveForce);
            Vector3 perpendicular = moveForce - parallel;

            otherMover.push(parallel / pullSpeed);
            transform.position += gap * minDiff;
            moveForce = perpendicular + parallel / pullSpeed;
        } else if (maxDiff < 0)
        {
            gap = -Vector3.Normalize(gap);
            Vector3 parallel = gap * Vector3.Dot(gap, moveForce);
            Vector3 perpendicular = moveForce - parallel;

            otherMover.push(parallel / pullSpeed);
            transform.position += gap * maxDiff;
            moveForce = perpendicular + parallel / pullSpeed;
        }

        moveForce /= drag;
    }

    /*
    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Wall")
        {
            Vector3 normal = collision.contacts[0].normal;
            float dot = Vector3.Dot(normal, moveForce);
            if (dot < 0f)
            {
                moveForce -= dot * normal;
            }
        }
    }
    */

    void push(Vector3 force)
    {
        moveForce += force;
    }
}
