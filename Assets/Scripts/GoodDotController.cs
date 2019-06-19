using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodDotController : MonoBehaviour
{
    public float speed = 0.05f;
    public float carryThrough = 0.1f;
    public Vector3 initialPos;
    public bool left = false;
    public bool entered = false;
    public int phase = 0;
    public float distanceLeft = 0f;
    public GameController game;

    void Awake()
    {
        initialPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (game.gameIsOver)
        {
            return;
        }

        transform.Translate(new Vector3(speed, 0, 0));
        if (phase == 2)
        {
            distanceLeft += speed;
            if (distanceLeft > carryThrough)
            {
                transform.position = initialPos;
                distanceLeft = 0f;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Collider other = collision.collider;
        if (other.tag == "Wall")
        {
            if (phase == 0)
            {
                phase = 1;
            } else if (phase == 2)
            {
                phase = 0;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (other.tag == "Wall" && phase == 1)
        {
            phase = 2;
        } else if (other.tag == "Box")
        {
            BoxController box = other.GetComponent<BoxController>();
            box.eatDot();
            Destroy(gameObject);
        }
    }
}
