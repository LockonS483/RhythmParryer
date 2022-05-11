using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitLine : MonoBehaviour
{
    LineRenderer lr;
    public float fadeSpeed;
    Color c;
    Vector3 sVec;
    Vector3 eVec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Vector3 start, Vector3 end){
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        sVec = start;
        eVec = end;
    }

    // Update is called once per frame
    void Update()
    {
        c = lr.material.GetColor("_Color");
        c.a -= fadeSpeed * Time.deltaTime;
        if(c.a <= 0){
            Destroy(gameObject);
        }else{
            lr.material.SetColor("_Color", c);
        }

        sVec = Vector3.Lerp(sVec, eVec, 0.065f);
        lr.SetPosition(0, sVec);
    }
}
