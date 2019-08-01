using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterableCubeController : MonoBehaviour
{
    public ShatterController shatter;
    private Mesh mesh;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shatter(ContactPoint contactPoint)
    {
        // construct the standard cube
        Vector3[] cubePoints = new Vector3[]
        {
            new Vector3(.5f, .5f, -.5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f), new Vector3(.5f, -.5f, -.5f),
            new Vector3(-.5f, .5f, -.5f), new Vector3(-.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(-.5f, -.5f, -.5f)
        };
        
        // rotate the cube so that the contact point is in pos x space
        Vector3 forceDir = contactPoint.normal;
        Vector3 worldContactDir = transform.position - contactPoint.point;
        Vector3 centerToContact = Quaternion.Inverse(transform.rotation) * worldContactDir;
        centerToContact.x /= transform.localScale.x;
        centerToContact.y /= transform.localScale.y;
        centerToContact.z /= transform.localScale.z;

        if (centerToContact.y < -0.3f) { centerToContact.y = -0.3f; }
        if (centerToContact.y > 0.3f) { centerToContact.y = 0.3f; }
        if (centerToContact.x < -0.5f) { centerToContact.x = -0.5f;  }
        if (centerToContact.x > 0.5f) { centerToContact.x = 0.5f;  }
        if (centerToContact.z < -0.5f) { centerToContact.z = -0.5f;  }
        if (centerToContact.z > 0.5f) { centerToContact.z = 0.5f;  }

        if (0.5f - Mathf.Abs(centerToContact.x) < 0.1f && 0.5f - Mathf.Abs(centerToContact.z) < 0.1f)
        {
            if (Mathf.Abs(centerToContact.x) < Mathf.Abs(centerToContact.z))
            {
                centerToContact.x = centerToContact.x / Mathf.Abs(centerToContact.x) * 0.0f;
            }
            else
            {
                centerToContact.z = centerToContact.z / Mathf.Abs(centerToContact.z) * 0.0f;
            }
        }

       
        float angle;
        Vector3 contactBack = centerToContact;
        if (Mathf.Abs(centerToContact.x) > Mathf.Abs(centerToContact.z))
        {
            if (centerToContact.x > 0)
            {
                angle = 0f;
            } else
            {
                angle = 180f;
            }
            contactBack.x = -contactBack.x;
        } else
        {
            if (centerToContact.z > 0)
            {
                angle = 90f;
            } else
            {
                angle = -90f;
            }
            contactBack.z = -contactBack.z;
        }

        // rotate the standard cube points into the correct relative orientation
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);     
        for (int i = 0; i < cubePoints.Length; i++)
        {
            cubePoints[i] = rotation * cubePoints[i];
            if (cubePoints[i].x < 0) { cubePoints[i].x = -.5f; } else { cubePoints[i].x = .5f; }
            if (cubePoints[i].y < 0) { cubePoints[i].y = -.5f; } else { cubePoints[i].y = .5f; }
            if (cubePoints[i].z < 0) { cubePoints[i].z = -.5f; } else { cubePoints[i].z = .5f; }
        }

        centerToContact.x = -centerToContact.x;
        contactBack.x = -contactBack.x;

        // check the width of the cube
        float widthRatio = transform.localScale.x / transform.localScale.z;
        
        // if wide enough, spider shatter
        if (widthRatio > 3.0f)
        {
            Vector3[] points;

            if (Mathf.Abs(centerToContact.x) > Mathf.Abs(centerToContact.z))
            {
                float randTop = Random.Range(0.1f, 0.4f);
                float randBot = Random.Range(0.1f, 0.4f);

                if (centerToContact.x < 0)
                {
                    randTop = -randTop;
                    randBot = -randBot;
                }

                Vector3 side1 = new Vector3(centerToContact.x, 0, 0.5f);
                Vector3 side2 = new Vector3(centerToContact.x, 0, -0.5f);
                Vector3 backTopLeft = new Vector3(randTop, 0.5f, 0.5f);
                Vector3 backTopRight = new Vector3(randTop, 0.5f, -0.5f);
                Vector3 backBotLeft = new Vector3(randBot, -0.5f, 0.5f);
                Vector3 backBotRight = new Vector3(randBot, -0.5f, -0.5f);

                points = new Vector3[6] { cubePoints[4], cubePoints[5], side1, side2, backTopLeft, backTopRight };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[10] { cubePoints[0], cubePoints[1], cubePoints[2], cubePoints[3], backTopLeft, backTopRight, backBotLeft, backBotRight, side1, side2 };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[6] { cubePoints[6], cubePoints[7], side1, side2, backBotLeft, backBotRight };
                CreateShatter(points, contactPoint.point, forceDir);
            }
            else
            {
                points = new Vector3[6] { cubePoints[0], cubePoints[4], cubePoints[1], cubePoints[5], centerToContact, contactBack };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[6] { cubePoints[1], cubePoints[5], cubePoints[2], cubePoints[6], centerToContact, contactBack };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[6] { cubePoints[2], cubePoints[6], cubePoints[3], cubePoints[7], centerToContact, contactBack };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[6] { cubePoints[3], cubePoints[7], cubePoints[4], cubePoints[0], centerToContact, contactBack };
                CreateShatter(points, contactPoint.point, forceDir);
            }
        }
        else // stair shatter
        {
            centerToContact.z = -centerToContact.z;

            Vector3 side1;
            Vector3 side2;
            Vector3 backTopLeft;
            Vector3 backTopRight;
            Vector3 backBotLeft;
            Vector3 backBotRight;

            float randTop = Random.Range(0.1f, 0.4f);
            float randBot = Random.Range(-0.4f, -0.1f);

          
            if (Mathf.Abs(centerToContact.x) > Mathf.Abs(centerToContact.z))
            {
                side1 = new Vector3(centerToContact.x, 0, 0.5f);
                side2 = new Vector3(centerToContact.x, 0, -0.5f);
                backTopLeft = new Vector3(-centerToContact.x, randTop, 0.5f);
                backTopRight = new Vector3(-centerToContact.x, randTop, -0.5f);
                backBotLeft = new Vector3(-centerToContact.x, randBot, 0.5f);
                backBotRight = new Vector3(-centerToContact.x, randBot, -0.5f);
            }
            else
            {
                side1 = new Vector3(0.5f, 0, centerToContact.z);
                side2 = new Vector3(-0.5f, 0, centerToContact.z);
                backTopLeft = new Vector3(0.5f, randTop, -centerToContact.z);
                backTopRight = new Vector3(-0.5f, randTop, -centerToContact.z);
                backBotLeft = new Vector3(0.5f, randBot, -centerToContact.z);
                backBotRight = new Vector3(-0.5f, randBot, -centerToContact.z);
            }

            Vector3[] points = new Vector3[8] { cubePoints[0], cubePoints[1], cubePoints[4], cubePoints[5], side1, side2, backTopLeft, backTopRight };
            CreateShatter(points, contactPoint.point, forceDir);

            points = new Vector3[6] { backTopLeft, backTopRight, backBotLeft, backBotRight, side1, side2 };
            CreateShatter(points, contactPoint.point, forceDir);

            points = new Vector3[8] { cubePoints[2], cubePoints[3], cubePoints[6], cubePoints[7], side1, side2, backBotLeft, backBotRight };
            CreateShatter(points, contactPoint.point, forceDir);
            
        }

        Destroy(gameObject);
    }

    ShatterController CreateShatter(Vector3[] points, Vector3 contactPoint, Vector3 forceDir)
    {
        // add some noise to the force direction
        forceDir.x += Random.Range(-0.2f, 0.2f);
        forceDir.y += Random.Range(-0.2f, 0.2f);
        forceDir.z += Random.Range(-0.2f, 0.2f);

        ShatterController shatterPiece = Instantiate(shatter, transform.position, transform.rotation);
        shatterPiece.LoadPoints(points);
        Vector3 scale = transform.localScale;
        shatterPiece.transform.localScale = scale;
        shatterPiece.rigidbody.AddForceAtPosition(20 * forceDir, contactPoint);
        shatterPiece.rigidbody.AddForceAtPosition(40 * Vector3.up, contactPoint);
        return shatterPiece;
    }
}