using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteTypes
{
    single, hold, hazard
}

public class MusicNote : MonoBehaviour
{
    
    public float startY;
    public float startX;
    public float endX;
    public float beat;
    public float endBeat;
    //public bool paused;
    public int track;

    public GameObject fxPrefab;

    public NoteTypes noteType;

    public bool isHeld;
    LineRenderer lr;

    public GameObject rootVisual;
    public Conductor c;
    public bool isHit;
    public void Initialize(float posY, float sX, float removeX, float beat, float track, float eBeat, NoteTypes nt){
        this.startY = posY;
        this.startX = sX;
        this.endX = removeX;
        this.beat = beat;
        this.track = Mathf.RoundToInt(track);
        this.endBeat = eBeat;
        this.noteType = nt;
        
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        if(noteType == NoteTypes.hold){
            lr = GetComponent<LineRenderer>();
            lr.enabled = true;
        }

        c = GameObject.Find("Manager").GetComponent<Conductor>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(startX + (endX - startX) * (1f - (beat - Conductor.songPosInBeats)), transform.position.y, transform.position.z);
        if (noteType == NoteTypes.single)
        {
            if(transform.position.x < endX - 3){
                // miss if beyond the end (same done for hold note start)
                Conductor.instance.hitAccuracy = 0.45f;
                Conductor.instance.rStats.hitCounts[4] += 1;
                Destroy(gameObject);
            }
            return;
        }
        if(noteType == NoteTypes.hold){
            //line renderer pos
             //currently being held?
            if(transform.position.x >= endX){
                lr.SetPosition(0, transform.position);
            }else{
                lr.SetPosition(0, new Vector3(endX, transform.position.y, transform.position.z));
            }
            Vector3 endpos = new Vector3(startX + (endX - startX) * (1f - (beat - (Conductor.songPosInBeats - (endBeat - beat))) ), 
                                transform.position.y, transform.position.z);
            lr.SetPosition(1, endpos);

            if(transform.position.x < endX - 3){
                if(isHeld){
                    //currently being held?
                }else{
                    //GameObject.Find("Manager").GetComponent<Conductor>().hitAccuracy = 1;
                    c.rStats.hitCounts[4] += 1;
                    Destroy(gameObject);
                }
            }
            if(Conductor.songPosInBeats >= endBeat){
                Destroy(gameObject);
            }
            return;
        }
        if(noteType == NoteTypes.hazard){
            if(transform.position.x < endX - 3){
                // miss if beyond the end (same done for hold note start)
                if (c.playerGO.transform.position.y == transform.position.y) {
                    Conductor.instance.hitAccuracy = 0.45f;
                    Conductor.instance.rStats.hitCounts[4] += 1;
                }
                Destroy(gameObject);
            }
            return;
        }
    }

    public void Hit(){
        isHit = true;
        Instantiate(fxPrefab, new Vector3(endX, transform.position.y, transform.position.z), Quaternion.identity);

        if(noteType == NoteTypes.single || noteType == NoteTypes.hazard)
            Destroy(gameObject);
    }
}