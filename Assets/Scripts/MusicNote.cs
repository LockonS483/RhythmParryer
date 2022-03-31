using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    public float startY;
    public float startX;
    public float endX;
    public float beat;
    //public bool paused;
    public int track;

    public GameObject fxPrefab;

    public void Initialize(float posY, float sX, float removeX, float beat, float track){
        this.startY = posY;
        this.startX = sX;
        this.endX = removeX;
        this.beat = beat;
        this.track = Mathf.RoundToInt(track);

        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startX + (endX - startX) * (1f - (beat - Conductor.songPosInBeats)), transform.position.y, transform.position.z);
    }

    public void Hit(){
        Instantiate(fxPrefab, new Vector3(endX, transform.position.y, transform.position.z), Quaternion.identity);
        print("HIT");
        Destroy(gameObject);
    }
}