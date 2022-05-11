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

    float darkTriggerX = 6.8f;
    //public bool paused;
    public int track;

    public GameObject fxPrefab;

    public NoteTypes noteType;

    public bool isHeld;
    LineRenderer lr;

    public GameObject rootVisual;
    public Conductor c;
    public bool isHit;
    public SpriteRenderer[] triggerSprites;

    bool wasTriggered;

    public GameObject spawnFX;
    public HitLine hitLine;
    public void Initialize(float posY, float sX, float removeX, float beat, float track, float eBeat, NoteTypes nt){
        this.startY = posY;
        this.startX = sX;
        this.endX = removeX;
        this.beat = beat;
        this.track = Mathf.RoundToInt(track);
        this.endBeat = eBeat;
        this.noteType = nt;
        wasTriggered = false;
        
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        

        c = GameObject.Find("Manager").GetComponent<Conductor>();

    }

    void TriggerNote(){
        if(noteType == NoteTypes.hold){
            lr = GetComponent<LineRenderer>();
            lr.enabled = true;
        }
        foreach(SpriteRenderer trigger in triggerSprites){
            trigger.enabled = true;
        }
        //Debug.Log(GameObject.FindGameObjectWithTag("dparry").name);
        GameObject.FindGameObjectWithTag("dparry").GetComponent<darkParryMove>().NoteCross(this.track);
        GameObject.Instantiate(spawnFX, transform.position, Quaternion.identity);
        wasTriggered = true;
    }

    // Update is called once per frame
    void Update()
    {   
        if(transform.position.x <= darkTriggerX && !wasTriggered){
            TriggerNote();
        }
        
        if(noteType == NoteTypes.hazard){
            transform.position = new Vector3(startX + (endX - startX) * (1f - (beat - Conductor.songPosInBeats) * 2f), transform.position.y, transform.position.z);
        }else{
            transform.position = new Vector3(startX + (endX - startX) * (1f - (beat - Conductor.songPosInBeats)), transform.position.y, transform.position.z);
        }
        if (noteType == NoteTypes.single)
        {
            if(transform.position.x < endX - 3){
                // miss if beyond the end (same done for hold note start)
                Conductor.instance.hitAccuracy = 0.45f;
                Conductor.instance.rStats.hitCounts[4] += 1;
                Conductor.instance.combo = 0;
                Destroy(gameObject);
            }
            return;
        }
        if(noteType == NoteTypes.hold && wasTriggered){
            //line renderer pos
            //currently being held?
                if(transform.position.x >= endX){
                    lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f));
                }else{
                    lr.SetPosition(0, new Vector3(endX, transform.position.y, transform.position.z + 0.1f));
                }

            float xClampCalc = startX + (endX - startX) * (1f - (beat - (Conductor.songPosInBeats - (endBeat - beat))));
            xClampCalc = Mathf.Clamp(xClampCalc, endX, darkTriggerX);

            Vector3 endpos = new Vector3(xClampCalc, 
                                transform.position.y, transform.position.z + 0.1f);
            lr.SetPosition(1, endpos);

            if(transform.position.x < endX - 3){
                if(isHeld){
                    //currently being held?
                }else{
                    //GameObject.Find("Manager").GetComponent<Conductor>().hitAccuracy = 1;
                    /*
                    c.rStats.hitCounts[4] += 1;
                    c.combo = 0;
                    */
                    Destroy(gameObject);
                }
            }
            if(Conductor.songPosInBeats >= endBeat){
                Destroy(gameObject);
            }
            return;
        }
        if(noteType == NoteTypes.hazard){
            if(transform.position.x < endX - 3f){
                // miss if beyond the end (same done for hold note start)
                if (Conductor.instance.currentTrack == this.track) {
                    Conductor.instance.hitAccuracy = 0.45f;
                    Conductor.instance.rStats.hitCounts[4] += 1;
                    Conductor.instance.combo = 0;
                }
                Destroy(gameObject);
            }
            return;
        }
    }

    public void Hit(){
        isHit = true;
        Instantiate(fxPrefab, new Vector3(endX, transform.position.y, transform.position.z), Quaternion.identity);

        if(hitLine){
            HitLine hl = Instantiate(hitLine, Vector3.zero, Quaternion.identity);
            Vector3 dpPos = GameObject.FindGameObjectWithTag("dparry").transform.position;
            hl.Init(transform.position, dpPos);
        }

        if(noteType == NoteTypes.single || noteType == NoteTypes.hazard)
            Destroy(gameObject);
    }
}