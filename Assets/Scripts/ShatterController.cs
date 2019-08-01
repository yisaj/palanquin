using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterController : MonoBehaviour
{
    private MeshFilter filter;
    new private MeshCollider collider;
    new public Rigidbody rigidbody;

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
        collider = GetComponent<MeshCollider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPoints(Vector3[] points)
    {
        Mesh mesh = Util.GenerateConvexMesh(points);
        
        /*
        Debug.Log("vertices");
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Debug.Log(mesh.vertices[i]);
        }
        Debug.Log("triangles");
        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            Debug.Log(mesh.triangles[i]);
        }
        */

        filter.mesh = mesh;
        collider.sharedMesh = mesh;
    }
}
