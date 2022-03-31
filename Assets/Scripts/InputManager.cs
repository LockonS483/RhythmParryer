using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] track1;
    public KeyCode[] track2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<track1.Length; i++){
            if(Input.GetKeyDown(track1[i])){
                //Debug.Log("p1");
                Conductor.instance.HitKey(0);
            }
        }

        for(int i=0; i<track2.Length; i++){
            if(Input.GetKeyDown(track2[i])){
                //Debug.Log("p2");
                Conductor.instance.HitKey(1);
            }
        }
    }
}
