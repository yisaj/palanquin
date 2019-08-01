using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // trivial intersection
        Triangle tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        Triangle tri2 = new Triangle(new Vector3(0f, 1f, 0f), new Vector3(0f, 2f, 1f), new Vector3(0f, 3f, 0f));
        Debug.Log("Trivial intersection: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // trivial nonintersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(1f, 1f, 0f), new Vector3(1f, 2f, 1f), new Vector3(1f, 3f, 0f));
        Debug.Log("Trivial nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // nontrivial intersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 0f), new Vector3(-1f, 1f, 0f));
        Debug.Log("Nontrivial intersection: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // almost but not intersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f), new Vector3(0f, 2f, 1f));
        Debug.Log("Almost but not intersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // contained intersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 2f, 2f), new Vector3(0f, 4f, 0f));
        Debug.Log("Contained intersection: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // indentical intersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        Debug.Log("Identical intersection: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // more complicated intersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(5f, 20f, 0f));
        tri2 = new Triangle(new Vector3(0f, 5f, 5f), new Vector3(5f, 5f, -5f), new Vector3(-5f, 5f, -5f));
        Debug.Log("More complicated intersection: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // nontrivial nonintersection
        tri1 = new Triangle(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f), new Vector3(0f, 2f, 0f));
        tri2 = new Triangle(new Vector3(0f, 2f, 0.5f), new Vector3(1f, 3f, 0.5f), new Vector3(-1f, 3f, 0.5f));
        Debug.Log("Nontrivial nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // side nonintersection
        tri1 = new Triangle(new Vector3(1f, 1f, 0f), new Vector3(1f, -1f, 0f), new Vector3(-1f, -1f, 0f));
        tri2 = new Triangle(new Vector3(1f, 1f, 0f), new Vector3(1f, -1f, 0f), new Vector3(0f, 0f, 1f));
        Debug.Log("side nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri2, tri1));

        // another side nonintersection
        tri1 = new Triangle(new Vector3(1f, 1f, 0f), new Vector3(1f, -1f, 0f), new Vector3(-1f, 1f, 0f));
        tri2 = new Triangle(new Vector3(1f, -1f, 0f), new Vector3(-1f, 1f, 0f), new Vector3(-1f, -1f, 0f));
        Debug.Log("side nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri2, tri1));

        // tricky nonintersection
        tri1 = new Triangle(new Vector3(0.5f, -0.5f, -.5f), new Vector3(.5f, 0f, .5f), new Vector3(0f, -.5f, -.5f));
        tri2 = new Triangle(new Vector3(1f, 1f, 0f), new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 1f));
        Debug.Log("tricky nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri2, tri1));

        // another tricky noninstersection
        tri1 = new Triangle(new Vector3(.5f, .5f, .5f), new Vector3(-.5f, .5f, .5f), new Vector3(-.1f, .2f, -.5f));
        tri2 = new Triangle(new Vector3(.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(-.1f, .2f, -.5f));
        Debug.Log("tricky nonintersection: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // getting real tired of your shit
        tri1 = new Triangle(new Vector3(-.5f, .5f, -.5f), new Vector3(-.5f, .5f, .5f), new Vector3(0f, -.5f, .5f));
        tri2 = new Triangle(new Vector3(-.5f, .5f, -.5f), new Vector3(-.5f, -.5f, -.5f), new Vector3(0, .5f, .5f));
        Debug.Log("why: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // 2 tired 2
        tri1 = new Triangle(new Vector3(-.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(1.5f, -.5f, -.5f));
        tri2 = new Triangle(new Vector3(-.5f, .5f, .5f), new Vector3(.5f, .5f, -.5f), new Vector3(.5f, .5f, .5f));
        Debug.Log("2 tired 2: Expecting false");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        // 333
        tri1 = new Triangle(new Vector3(-.5f, .5f, .5f), new Vector3(-.5f, -.5f, .5f), new Vector3(0, -.5f, .5f));
        tri2 = new Triangle(new Vector3(-.5f, -.5f, .5f), new Vector3(0, -.5f, .5f), new Vector3(.5f, 0, .5f));
        Debug.Log("333: Expecting true");
        Debug.Log(Util.IntersectTriangles(tri1, tri2));

        Debug.Log("---------");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
