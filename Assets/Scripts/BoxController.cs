using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour
{
    public GameController game;
    public MoverController leftMover;
    public MoverController rightMover;
    private ShatterableCubeController shatterScript;

    public float area = 3f;
    public float minWidth = 1f;
    private float width = 1f;
    private float length = 3f;
    public float dotIncrement = 0.01f;

    [System.NonSerialized]
    public float squareSide;
    [System.NonSerialized]
    public float maxLength;

    void Awake()
    {
        squareSide = Mathf.Sqrt(area);
        maxLength = area / minWidth;

        Physics.IgnoreCollision(GetComponent<Collider>(), leftMover.GetComponent<Collider>());
        Physics.IgnoreCollision(GetComponent<Collider>(), rightMover.GetComponent<Collider>());

        shatterScript = GetComponent<ShatterableCubeController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        length = (leftMover.transform.position - rightMover.transform.position).magnitude;
        width = area / length;
      
        if (width < minWidth)
        {
            width = minWidth;
            length = maxLength;
        }
        else if (length < width)
        {
            length = squareSide;
            width = squareSide;
        }
        transform.localScale = new Vector3(length, transform.localScale.y, width);

        Vector3 gap = leftMover.transform.position - rightMover.transform.position;

        float angle = Vector3.SignedAngle(Vector3.right, gap, Vector3.up);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.position = leftMover.transform.position - gap / 2f;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        /*for (int i = 0; i < collision.contactCount; i++)
        {
            Debug.Log("CONTACT");
            Debug.Log(collision.GetContact(i).point);
            Debug.Log(collision.GetContact(i).normal);
        }*/

        if (other.tag == "Wall" || other.tag == "BadDot")
        {
            shatterScript.Shatter(collision.GetContact(0));
            game.GameOver();
        } else if (other.tag == "GoodDot")
        {
            area += dotIncrement;
            squareSide = Mathf.Sqrt(area);
            maxLength = area / minWidth;
            Destroy(other.gameObject);
        }
    }
}
