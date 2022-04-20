using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinFloat : MonoBehaviour
{
    public float speed = 1;
    public float magnitude = 1;
    public float offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float ypos = Mathf.Sin( (Time.time+offset) * speed ) * magnitude;
        transform.localPosition = new Vector3(0, ypos, transform.position.z);
    }
}