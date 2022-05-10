using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldGhost : MonoBehaviour
{   
    public Transform spriteChild;
    public int track;
    public bool death = false;
    SpriteRenderer sr;
    Vector3 tPos = new Vector3(0,0,-1);
    // Start is called before the first frame update
    void Start()
    {
        death = false;
        sr = spriteChild.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!death){
            if(spriteChild.localPosition.x <= 0){
                spriteChild.transform.Translate((16f + Mathf.Abs(spriteChild.localPosition.x * 4f))*Time.deltaTime, 0, 0);
            }
        }else{
            Color a = sr.color;
            a.a -= 1.8f * Time.deltaTime;
            if(a.a <= 0){
                Destroy(gameObject);
            }else{
                sr.color = a;
            }
        }
        if(!Conductor.instance.heldTracks[track]){
            death = true;
        }
    }
}
