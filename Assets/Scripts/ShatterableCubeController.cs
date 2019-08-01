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
        Debug.Log("Start");
        //Shatter(new ContactPoint());
        Debug.Log("End");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shatter(ContactPoint contactPoint)
    {
        /*
        Vector3[] pts = new Vector3[10] { new Vector3(0.5f,0.5f,.5f), new Vector3(0.5f,.5f,-.5f), new Vector3(.5f,-.5f,.5f),
               new Vector3(.5f,-.5f,-.5f), new Vector3(0f,.5f,.5f), new Vector3(0f,-.5f,.5f), new Vector3(0f,.5f,-0.5f),
 new Vector3(0f,-.5f,-.5f), new Vector3(-.5f,0,-.5f), new Vector3(-.5f,0,.5f) };
        CreateShatter(pts, contactPoint.point, Vector3.zero);
        Destroy(gameObject);
        return;
        */
        Debug.Log('1');
        Debug.Log(transform.localScale);
        // construct the standard cube
        Vector3[] cubePoints = new Vector3[]
        {
            new Vector3(.5f, .5f, -.5f), new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f), new Vector3(.5f, -.5f, -.5f),
            new Vector3(-.5f, .5f, -.5f), new Vector3(-.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(-.5f, -.5f, -.5f)
        };
        Debug.Log('2');
        // rotate the cube so that the contact point is in pos x space
        Vector3 forceDir = contactPoint.normal;//(other.transform.position - contactPoint.point).normalized;
        Vector3 worldContactDir = transform.position - contactPoint.point;
        Vector3 centerToContact = Quaternion.Inverse(transform.rotation) * worldContactDir;
        centerToContact.x /= transform.localScale.x;
        centerToContact.y /= transform.localScale.y;
        centerToContact.z /= transform.localScale.z;
        Debug.Log(centerToContact.x);
        Debug.Log(centerToContact.z);

        if (centerToContact.y < -0.3f) { centerToContact.y = -0.3f; }
        if (centerToContact.y > 0.3f) { centerToContact.y = 0.3f; }
        if (centerToContact.x < -0.5f) { centerToContact.x = -0.5f;  }
        if (centerToContact.x > 0.5f) { centerToContact.x = 0.5f;  }
        if (centerToContact.z < -0.5f) { centerToContact.z = -0.5f;  }
        if (centerToContact.z > 0.5f) { centerToContact.z = 0.5f;  }

        if (0.5f - Mathf.Abs(centerToContact.x) < 0.1f && 0.5f - Mathf.Abs(centerToContact.z) < 0.1f)
        {
            Debug.Log("GIT");
            if (Mathf.Abs(centerToContact.x) < Mathf.Abs(centerToContact.z))
            {
                centerToContact.x = centerToContact.x / Mathf.Abs(centerToContact.x) * 0.0f;
            }
            else
            {
                centerToContact.z = centerToContact.z / Mathf.Abs(centerToContact.z) * 0.0f;
            }
        }

        float widthRatio = transform.localScale.x / transform.localScale.z;
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

        //Debug.Log('3');
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        
        for (int i = 0; i < cubePoints.Length; i++)
        {
            cubePoints[i] = rotation * cubePoints[i];
            if (cubePoints[i].x < 0) { cubePoints[i].x = -.5f; } else { cubePoints[i].x = .5f; }
            if (cubePoints[i].y < 0) { cubePoints[i].y = -.5f; } else { cubePoints[i].y = .5f; }
            if (cubePoints[i].z < 0) { cubePoints[i].z = -.5f; } else { cubePoints[i].z = .5f; }
            /*
            cubePoints[i].x *= transform.localScale.x;
            cubePoints[i].y *= transform.localScale.y;
            Debug.Log("))))))))");
            Debug.Log(cubePoints[i].z);
            cubePoints[i].z *= transform.localScale.z;
            Debug.Log(cubePoints[i].z);*/
        }
        
        // check the width of the cube
        float width = transform.localScale.z / 2.0f;
        float length = transform.localScale.x / 2.0f;
        //Debug.Log('4');
        centerToContact.x = -centerToContact.x;
        contactBack.x = -contactBack.x;
        //Debug.Log(Mathf.Abs(centerToContact.x) - Mathf.Abs(centerToContact.z));
 
       //Debug.Log("nnnn");
        //Debug.Log(centerToContact.z);
        //Debug.Log(centerToContact.x);

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

                //Debug.Log("XXXX");
                //Debug.Log(centerToContact.x);

                points = new Vector3[6] { cubePoints[4], cubePoints[5], side1, side2, backTopLeft, backTopRight };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[10] { cubePoints[0], cubePoints[1], cubePoints[2], cubePoints[3], backTopLeft, backTopRight, backBotLeft, backBotRight, side1, side2 };
                CreateShatter(points, contactPoint.point, forceDir);

                points = new Vector3[6] { cubePoints[6], cubePoints[7], side1, side2, backBotLeft, backBotRight };
                CreateShatter(points, contactPoint.point, forceDir);
                //Debug.Log("HELLLOO?????");
                //Debug.Log(side1);
                //Debug.Log(side2);
            }
            else
            {

                for (int i = 0; i < 8; i++)
                {
                    //Debug.Log("asdf");
                    //Debug.Log(cubePoints[i]);

                }
                //Debug.Log("cent");
                //Debug.Log(centerToContact);
                //Debug.Log(contactBack);
                //Debug.Log(transform.localScale);
                //Debug.Log("ffff");

                //Debug.Log(contactPoint.point);

                Vector3 t = new Vector3(0, 0, 0.5f);
                Vector3 r = new Vector3(0, 0, -0.5f);
                t = rotation * t;
                r = rotation * r;/*
            t.x *= transform.localScale.x;
            t.y *= transform.localScale.y;
            t.z *= transform.localScale.z;
            r.x *= transform.localScale.x;
            r.y *= transform.localScale.y;
            r.z *= transform.localScale.z;*/


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
            //Debug.Log("asdfxcvxcv");
            //Debug.Log(centerToContact);
            //Debug.Log(side1);
            //Debug.Log(side2);
            //Debug.Log(centerToContact.x);
            //Debug.Log(centerToContact.z);

            Vector3[] points = new Vector3[8] { cubePoints[0], cubePoints[1], cubePoints[4], cubePoints[5], side1, side2, backTopLeft, backTopRight };
            CreateShatter(points, contactPoint.point, forceDir);

            points = new Vector3[6] { backTopLeft, backTopRight, backBotLeft, backBotRight, side1, side2 };
            CreateShatter(points, contactPoint.point, forceDir);

            points = new Vector3[8] { cubePoints[2], cubePoints[3], cubePoints[6], cubePoints[7], side1, side2, backBotLeft, backBotRight };
            CreateShatter(points, contactPoint.point, forceDir);
            
        }
        //Debug.Log('7');
        Destroy(gameObject);
        /*
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = localToWorld * mesh.vertices[i];
        }

        ShatterController curPiece;
        Mesh curMesh;
        shatter = Instantiate(shatter, transform.position, transform.rotation);
        shatter.transform.position = transform.position;
        shatter.transform.localScale = transform.localScale;
        curMesh = new Mesh();
        curMesh.vertices = vertices;

        int[] triangles = {
            0, 2, 1, //face front
            0, 4, 2, //face right
			0, 6, 4,
			4, 6, 7, //face back
            0, 7, 6, //face bottom
			0, 1, 7,
            2, 4, 1,
            4, 7, 1
        };

        curMesh.triangles = triangles;
        //curMesh.RecalculateNormals();

        Vector3[] points = new Vector3[5]
        {
            new Vector3(.5f,.5f,.5f), new Vector3(-.5f,.5f,.5f), new Vector3(.5f,-.5f,.5f), new Vector3(-.5f,-.5f,.5f), new Vector3(-0.1f, 0.2f, -0.5f)
        };
        shatter.LoadPoints(points);
        */

        /*
        Vector3 point = transform.InverseTransformPoint(contactPoint.point);
        Vector3 norm = Quaternion.Inverse(transform.rotation) * contactPoint.normal;
        //    norm = contactPoint.normal;

        Vector3[] vertices = new Vector3[8] {
            new Vector3(.5f, .5f, .5f), new Vector3(.5f, -.5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(-.5f, .5f, .5f),
            new Vector3(.5f, .5f, -.5f), new Vector3(.5f, -.5f, -.5f), new Vector3(-.5f, -.5f, -.5f), new Vector3(-.5f, .5f, -.5f)
        };
        */
        /*
        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log("hey");
            Debug.Log(vertices[i]);
            //worldVertices[i] = transform.TransformPoint(worldVertices[i]);
            Debug.Log(transform.TransformPoint(vertices[i]));
        }

        Vector3 projectedPoint = Vector3.Project(point, norm);

        Debug.Log(transform.localToWorldMatrix);
        Debug.Log(point);
        Debug.Log(norm);
        Debug.Log("-------");

        Debug.Log("++++++ normies");
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(Vector3.Project(point - vertices[i], norm).magnitude);
        }
        
        List<Vector3> sortedVertices = new List<Vector3>();
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = 0; j < sortedVertices.Count + 1; j++)
            {
                if (j == sortedVertices.Count)
                {
                    sortedVertices.Add(vertices[i]);
                    
                    Debug.Log("added from");
                    Debug.Log(i);
                    
                    break;
                }

                if (Vector3.Project(point - vertices[i], norm).magnitude <
                    Vector3.Project(point - sortedVertices[j], norm).magnitude)
                {
                    sortedVertices.Insert(j, vertices[i]);
                    
                    Debug.Log("inserted to");
                    Debug.Log(j);
                    
                    break;
                }
            }
        }

        int posIndex = 0;
        int negIndex = 0;
        Vector3[] positives = new Vector3[2];
        Vector3[] negatives = new Vector3[2];

        for (int i = 4; i < 8; i++) // sort far plane by y
        {
            if (sortedVertices[i].y < 0)
            {
                negatives[negIndex] = sortedVertices[i];
                negIndex++;
            }
            else
            {
                positives[posIndex] = sortedVertices[i];
                posIndex++;
            }
        }
        sortedVertices[4] = positives[0];
        sortedVertices[5] = positives[1];
        sortedVertices[6] = negatives[0];
        sortedVertices[7] = negatives[1];

        posIndex = 0;
        negIndex = 0;

        for (int i = 0; i < 4; i++) // sort near plane by y
        {
            if (sortedVertices[i].y < 0)
            {
                negatives[negIndex] = sortedVertices[i];
                negIndex++;
            }
            else
            {
                positives[posIndex] = sortedVertices[i];
                posIndex++;
            }
        }
        sortedVertices[0] = positives[0];
        sortedVertices[1] = positives[1];
        sortedVertices[2] = negatives[0];
        sortedVertices[3] = negatives[1];

        
        if ((sortedVertices[0] + sortedVertices[2]).magnitude == 0) //
        {
            Vector3 temp = sortedVertices[0];
            sortedVertices[0] = sortedVertices[1];
            sortedVertices[1] = temp;
        }

        if ((sortedVertices[4] + sortedVertices[6]).magnitude == 0) //
        {
            Vector3 temp = sortedVertices[4];
            sortedVertices[4] = sortedVertices[5];
            sortedVertices[5] = temp;
        }

        for (int i = 0; i < 8; i += 2) //
        {
            if (sortedVertices[i].x < 0 || sortedVertices[i].z < 0)
            {
                Vector3 temp = sortedVertices[i];
                sortedVertices[i] = sortedVertices[i + 1];
                sortedVertices[i + 1] = temp;
            }
        }
        

        bool xAligned = sortedVertices[0].x == sortedVertices[1].x;

        if (xAligned)
        {
            for (int i = 0; i < 8; i += 2)
            {
                if (sortedVertices[i].z > 0)
                {
                    Vector3 temp = sortedVertices[i];
                    sortedVertices[i] = sortedVertices[i + 1];
                    sortedVertices[i + 1] = temp;
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i += 2)
            {
                if (sortedVertices[i].x > 0)
                {
                    Vector3 temp = sortedVertices[i];
                    sortedVertices[i] = sortedVertices[i + 1];
                    sortedVertices[i + 1] = temp;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log((point - sortedVertices[i]).magnitude);
            if ((point - sortedVertices[i]).magnitude < .000001)
            {
                Debug.Log("HIT");
                if (xAligned)
                {
                    point = sortedVertices[i];
                }
            }
        }

        // for each side of the square, generate 0-2 fracture points.
        List<Vector2> topFractures = new List<Vector2>();
        List<Vector2> rightFractures = new List<Vector2>();
        List<Vector2> bottomFractures = new List<Vector2>();
        List<Vector2> leftFractures = new List<Vector2>();
        int numFractures = Random.Range(1, 3);
        float rand = Random.Range(-0.4, 0.4);
        int m;
        for (int i = 0; i < numFractures; i++)
        {
            for (m = 0; m < topFractures.Count; m++)
            {
                if (rand < topFractures[m].x)
                {
                    topFractures.Insert(new Vector2(rand, .5f), m);
                    break;
                }
            }
            if (m == topFractures.Count)
            {
                topFractures.Add(new Vector2(rand, .5f));
            }
        }
        numFractures = Random.Range(0, 3);
        for (int i = 0; i < numFractures; i++)
        {
            rand = Random.Range(-0.4, 0.4);
            for (m = 0; m < bottomFractures.Count; m++)
            {
                if (rand > bottomFractures[m].x)
                {
                    bottomFractures.Insert(new Vector2(rand, -.5f), m);
                    break;
                }
            }
            if (m == bottomFractures.Count)
            {
                bottomFractures.Add(new Vector2(rand, -.5f));
            }
        }
        numFractures = Random.Range(0, 3);
        for (int i = 0; i < numFractures; i++)
        {
            rand = Random.Range(-0.4, 0.4);
            for (m = 0; m < rightFractures.Count; m++)
            {
                if (rand < rightFractures[m].y)
                {
                    rightFractures.Insert(new Vector2(.5f, rand), m);
                    break;
                }
            }
            if (m == rightFractures.Count)
            {
                rightFractures.Add(new Vector2(.5f, rand));
            }
        }
        numFractures = Random.Range(0, 3);
        for (int i = 0; i < numFractures; i++)
        {
            rand = Random.Range(-0.4, 0.4);
            for (m = 0; m < leftFractures.Count; j++)
            {
                if (rand > leftFractures[m].y)
                {
                    leftFractures.Insert(new Vector2(-.5f, rand), m);
                    break;
                }
            }
            if (m == leftFractures.Count)
            {
                leftFractures.Add(new Vector2(-.5f, rand));
            }
        }

        ShatterController shatterPiece;
        Vector3[] points;
        if (xAligned)
        {
            for (int i = 0; i < topFractures.Count - 1; i++)
            {
                points = new Vector3[6] { new Vector3(0.5f, point.y, point.z), new Vector3(-0.5f, point.y, point.z), new Vector3(.5f, .5f, topFractures[i].x),
                         new Vector3(-.5f, .5f. topFractures[i].x), new Vector3(.5f, .5f, topFractures[i + 1].x), new Vector3(-.5f, .5f, topFractures[i + 1].x) };
                shatterPiece = CreateShatter(points);
            }
            points = new Vector3[6] { new Vector3(0.5f, point.y, point.z), new Vector3(-0.5f, point.y, point.z), sortedVertices };
        }
 
        Debug.Log("++++++ sorted");
        for (int i = 0; i < sortedVertices.Count; i++)
        {
            Debug.Log(sortedVertices[i]);
        }

        ShatterController shatterPiece;
        Vector3[] points;
        shatterPiece = Instantiate(shatter, transform.position, transform.rotation);
        points = new Vector3[6] { sortedVertices[4], sortedVertices[5], sortedVertices[6], sortedVertices[7], new Vector3(point.x, 0.5f, point.z), new Vector3(point.x, -0.5f, point.z) };
        shatterPiece.LoadPoints(points);
        shatterPiece.transform.localScale = transform.localScale;
        shatterPiece.rigidbody.AddForce(norm * 200);

        shatterPiece = Instantiate(shatter, transform.position, transform.rotation);
        points = new Vector3[6] { sortedVertices[0], sortedVertices[2], sortedVertices[4], sortedVertices[6], new Vector3(point.x, 0.5f, point.z), new Vector3(point.x, -0.5f, point.z) };
        shatterPiece.LoadPoints(points);
        shatterPiece.transform.localScale = transform.localScale;
        shatterPiece.rigidbody.AddForce(norm * 200);

        shatterPiece = Instantiate(shatter, transform.position, transform.rotation);
        points = new Vector3[6] { sortedVertices[1], sortedVertices[3], sortedVertices[5], sortedVertices[7], new Vector3(point.x, 0.5f, point.z), new Vector3(point.x, -0.5f, point.z) };
        shatterPiece.LoadPoints(points);
        shatterPiece.transform.localScale = transform.localScale;
        shatterPiece.rigidbody.AddForce(norm * 200);

        //shatterPiece = Instantiate(shatter);
        //points = new Vector3[5] { worldVertices[4], worldVertices[5], worldVertices[6], worldVertices[7], point };
        //shatterPiece.LoadPoints(points);

        Destroy(gameObject);
        */

    }

    ShatterController CreateShatter(Vector3[] points, Vector3 contactPoint, Vector3 forceDir)
    {
        /*
        for (int i = 0; i < points.Length; i++)
        {
            //points[i] *= 0.01f;
        }
        */
        // add some noise to the force direction
        forceDir.x += Random.Range(-0.2f, 0.2f);
        forceDir.y += Random.Range(-0.2f, 0.2f);
        forceDir.z += Random.Range(-0.2f, 0.2f);

        ShatterController shatterPiece = Instantiate(shatter, transform.position, transform.rotation);
        shatterPiece.LoadPoints(points);
        Vector3 scale = transform.localScale;
        //scale.x; /= 0.01f;
        //scale.y; /= 0.01f;
        //scale.z; /= 0.01f;
        shatterPiece.transform.localScale = scale;
        shatterPiece.rigidbody.AddForceAtPosition(20 * forceDir, contactPoint);
        shatterPiece.rigidbody.AddForceAtPosition(40 * Vector3.up, contactPoint);
        return shatterPiece;
    }
}