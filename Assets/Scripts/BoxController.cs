using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public GameObject leftMover;
    public GameObject rightMover;

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
        transform.localScale = new Vector3(length, 1, width);

        Vector3 gap = leftMover.transform.position - rightMover.transform.position;

        float angle = Vector3.SignedAngle(Vector3.right, gap, Vector3.up);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.position = leftMover.transform.position - gap / 2f;
    }

    void eatDot()
    {
        area += dotIncrement;
    }
}
