using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camera;
    public float shakeDuration;
    public float shakeMagnitude;
    public float dampingSpeed;
    float shakeEnd;
    
    Vector3 camPos;

    // Start is called before the first frame update
    void Start()
    {
        camPos = camera.position;
        //shakeDuration = 60f / Conductor.instance.songBpm / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < shakeEnd){
            Vector2 camOffset = new Vector2(Random.Range(-shakeMagnitude, shakeMagnitude), Random.Range(-shakeMagnitude, shakeMagnitude));
            Vector3 newCamPos = new Vector3(camPos.x + camOffset.x, camPos.y + camOffset.y, camPos.z);
            camera.position = Vector3.Lerp(camera.position, newCamPos, dampingSpeed);
        }else{
            camera.position = camPos;
        }
    }

    public void AddShake(){
        shakeEnd = Time.time + shakeDuration;
    }
}
