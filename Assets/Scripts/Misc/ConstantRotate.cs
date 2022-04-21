using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotate : MonoBehaviour
{
    public Vector3 spinVec;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinVec*Time.deltaTime);
    }
}
