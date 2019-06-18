using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private float width;
    private float height = 1;

    void Awake()
    {
        width = Camera.main.aspect;
        transform.localScale = new Vector3(width, transform.localScale.y, height);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        width = Camera.main.aspect;
        transform.localScale = new Vector3(width, transform.localScale.y, height);
    }
}
