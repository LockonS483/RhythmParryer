using System.Resources;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Conductor : MonoBehaviour
{
    public GameObject playerGO;
    public PlayerAnims playerAnims;
    public CameraShake camshake;
    public AudioSource hitAudio;
    public GameObject recorder;
    public Recorder rStats;
    public StageController sController;
    //-------------------
    public float songBpm;  //170 for a long fall
    public float secPerBeat;
    public float firstBeatOffset;
    public static float songPos;
    public static float songPosInBeats;
    public float dspSongTime;

    public AudioSource musicSource;

    public List<Vector2> notes; //y is beat, x is position
    public List<Vector3> otherNoteInfo;
    public int nextIndex;
    public float beatsSpawned;
    public float songPlayOffset;

    public float hitAccuracy;

    public static Conductor instance;


    public MusicNote notePrefab;
    public MusicNote hazardPrefab;

    public float laneY1;
    public float laneY2;
    public float startX;
    public float endX;


    public TextAsset map;
    public Transform spawnpoint;

    [HideInInspector]
    List<MusicNote> spawnedNotes;
    int spawnedNotesInd;
    public int currentTrack = 0;
    public bool[] heldTracks= new bool[]{false, false};
    MusicNote[] heldNotes = new MusicNote[2];
    public GameObject[] holdParticles;
    int lastanim;
    public float notePressWindow = 0.06f;
    bool done;
    public int combo;
    public int score;
    public int targetScore;
    int scoreAmnt = 100;
    float endTimer = 0.5f;
    float endMarker;
    float hzMod = 1; // modifier for hitting hazards

    float[] holdTimer = new float[]{0f, 0f};
    float holdInterval = 0.07f;

    public HoldGhost holdGhost;
 Animation anim;
    void Awake(){
        instance = this;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.SetState(GameState.Gameplay);
        var rObj = Instantiate(recorder);
        rStats = rObj.GetComponent<Recorder>();
        done = false;
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("BGLazer").GetComponent<Animation>();
        /*print(StageController.musicClip);
        if (!StageController.musicClip) {
            //AudioSource musicSource = gameObject.AddComponent <AudioSource>() as AudioSource;
            AudioClip audioClip = Resources.Load<AudioClip>("Music/close-in-the-distance"); // change name to testaudio for quick debugging
            musicSource.clip = audioClip;
        } else {
            //AudioSource musicSource = gameObject.AddComponent <AudioSource>() as AudioSource;
            AudioClip audioClip = StageController.musicClip;
            musicSource.clip = audioClip;
        }*/
        
        dspSongTime = (float)AudioSettings.dspTime;
        spawnedNotes = new List<MusicNote>();
        Invoke("StartMusic", songPlayOffset);
        GenerateNotes();
        secPerBeat = 60f / songBpm;
        nextIndex = 0;
        spawnedNotesInd = 0;
        score = 0;
        combo = 0;
        GameObject.Find("EndScene").GetComponent<Canvas>().enabled = false;
    }

    void StartMusic(){
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameObject.Find("EndScene").GetComponent<Canvas>().enabled == false) { // for testing
        //     GameObject.Find("EndScene").GetComponent<Canvas>().enabled = true;
        //     GameObject.Find("EndScene").GetComponent<Animator>().Play("level_transition_anim_anim", -1, 0f);
        //     print(GameObject.Find("EndScene").GetComponent<Canvas>().enabled);
        // }
        
        //moving the player
        Vector3 playerPos = new Vector3(-5.5f, transform.position.y, 0);
        if(currentTrack == 0){
            playerPos.y = -2;
        }else if (currentTrack == 1){
            playerPos.y = 2;
        }
        playerGO.transform.position = Vector3.Lerp(playerGO.transform.position, playerPos, 0.75f);

        //print("songpos: " + songPosInBeats.ToString());

        //if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        songPos = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - songPlayOffset);
        
        songPosInBeats = songPos / secPerBeat;
        
        // SPAAAAAAAAAAAAAWN NOTES
        if(nextIndex < notes.Count && notes[nextIndex].y < songPosInBeats + beatsSpawned){
            //print(spawnpoint.position + " " + Quaternion.identity);
            MusicNote m;
            NoteTypes nt = NoteTypes.single;
            if(Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 0 || Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 1){ //Note is not a hazard
                m = Instantiate(notePrefab, spawnpoint.position, Quaternion.identity);
                if(Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 1){
                    nt = NoteTypes.hold;
                }
            }else{ // note is a hazard
                m = Instantiate(hazardPrefab, spawnpoint.position, Quaternion.identity);
                nt = NoteTypes.hazard;
            }

            float ty = notes[nextIndex].x == 0 ? laneY1 : laneY2;

            m.Initialize(ty, startX, endX, notes[nextIndex].y, notes[nextIndex].x, otherNoteInfo[nextIndex].y, nt);
            spawnedNotes.Add(m);
            nextIndex++;
        }
        
        for(int i = 0; i<2; i++){
            if(heldTracks[i]){
                if(heldNotes[i]){
                    holdParticles[i].SetActive(true);
                    if(holdTimer[i] <= 0){
                        holdTimer[i] = holdInterval;
                        combo++;
                        hitAudio.Play();
                    }
                }else{
                    holdParticles[i].SetActive(false);
                }
                
                if(holdTimer[i] > 0)
                    holdTimer[i] -= Time.deltaTime;
            }else{
                for(int pi=i*2; pi<(i*2+2); pi++){
                    holdParticles[i].SetActive(false);
                }
            }
        }

        //print(musicSource.time);
        if (musicSource.time<=0 && spawnedNotes.Count>0) {
            if (endTimer > 0)
            {
                endTimer -= Time.deltaTime;
            }
            else {
                // GameObject.Find("EndScene").GetComponent<Animator>().Play("level_transition_anim_anim", -1, 0f
                if (!done) {
                    print(rStats.highestCombo);
                    print((float)score * ((float)rStats.highestCombo / (float)spawnedNotes.Count));
                    print(score);
                    score = score + (int)((float)score * ((float)rStats.highestCombo / (float)spawnedNotes.Count));
                    print(score);
                    GameObject.Find("EndScene").GetComponent<Canvas>().enabled = true;
                    done = true;
                }
            }
        }


        if (spawnedNotes.Count < 1 || spawnedNotesInd >= spawnedNotes.Count) {
            return;
        }

        if(spawnedNotes[spawnedNotesInd].beat < songPosInBeats - notePressWindow){
            spawnedNotesInd++;
        }

        
    }

    public float PosFromBeat(float beat){ //I actually don't know what this does but it is never called. Leaving this in just because
        return (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
    }

    void GenerateNotes(){
        notes = new List<Vector2>();
        otherNoteInfo = new List<Vector3>();
        string fs = map.text;
        string[] maplines = fs.Split('\n');
        var metadata = maplines[0].Split(' ');
        print(metadata[0] + " " + metadata[1] + " " + metadata[2]);
        songBpm = float.Parse(metadata[0].Substring(1));
        
        print("songBpm:" + songBpm);
        endMarker = float.Parse(metadata[2]);
        //print(endMarker);
        float offset = float.Parse(metadata[1]);
        print("songOffset:" + offset);
        for(int i=1; i<maplines.Length; i++){
            string[] nn = maplines[i].Split(' ');
            if (nn[0].Contains("/") || nn.Length==1)
                continue;
            float tx = float.Parse(nn[0]);
            float ty = float.Parse(nn[2]) + offset;
            //print(tx + " " + ty);
            notes.Add(new Vector2(tx, ty));
            
            //additional info (TYPEVAL: 0 = single, HOLD = 1, HAZARD = 2)
            int typeVal = 0;
            float endBeat = 0;
            if(nn[1] == "SINGLE"){
                typeVal = 0;
            }else if(nn[1] == "HELD"){
                typeVal = 1;
                endBeat = float.Parse(nn[3]) + offset;
            }else if(nn[1] == "HAZARD"){
                typeVal = 2;
            }

            otherNoteInfo.Add(new Vector3(typeVal, endBeat, 0));
        }
    }

    public void HitKey(int track){
        currentTrack = track;
        //basic animation test
        int canim = Random.Range(0,2);
        while(lastanim == canim){
            canim = Random.Range(0,2);
        }
        lastanim = canim;
        playerAnims.PlayAnim(lastanim);

        for(int i=0; i<6; i++){
            if((spawnedNotesInd+i) >= (spawnedNotes.Count)) continue;
            if(spawnedNotes[spawnedNotesInd+i] == null) continue;
            if(spawnedNotes[spawnedNotesInd+i].isHit) continue;

            if(spawnedNotes[spawnedNotesInd+i].track == track){
                //print("right track: " + track.ToString());
                //print("diff: " + Mathf.Abs(spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats).ToString());

                if (Mathf.Abs(spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats) <= notePressWindow){
                    spawnedNotes[spawnedNotesInd + i].Hit();
                    hitAccuracy = spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats;
                    combo += 1;
                    if (combo % 100 == 0 && combo!=0) {
                        anim.Play();
                    }
                    if (combo > rStats.highestCombo)
                        rStats.highestCombo = combo;
                    //print("hitaccuracy: " + hitAccuracy.ToString());
                    if (spawnedNotes[spawnedNotesInd + i].noteType == NoteTypes.hazard)
                        hzMod = 1.5f;
                    else
                        hzMod = 1;
                    switch (hitAccuracy) {
                        case float f when f< 0.13f:
                            //Perfect case
                            score += (int)((1 / 0.13f) * (scoreAmnt*hzMod));
                            rStats.hitCounts[0] += 1;
                            break;
                        case float f when f< 0.2f:
                            //Great case
                            score += (int)((1 / 0.2f) * (scoreAmnt*hzMod));
                            rStats.hitCounts[1] += 1;
                            break;
                        case float f when f < 0.3f:
                            //Ok case
                            score += (int)((1 / 0.3f) * (scoreAmnt * hzMod));
                            rStats.hitCounts[2] += 1;
                            break;
                        case float f when f < 0.4f:
                            //Bad case
                            score += (int)((1 / 0.4f) * (scoreAmnt * hzMod));
                            rStats.hitCounts[3] += 1;
                            break;
                    }
                    //UnityEngine.Debug.LogFormat("{0} {1} {2} {3} {4}", rStats.hitCounts[0], rStats.hitCounts[1], rStats.hitCounts[2], rStats.hitCounts[3], rStats.hitCounts[4]);
                    rStats.score = score;
                    camshake.AddShake();
                    hitAudio.Play();
                    spawnedNotes[spawnedNotesInd + i].Hit();
                    hitAccuracy = spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats;

                    if(spawnedNotes[spawnedNotesInd + i].noteType == NoteTypes.hold){
                        heldTracks[track] = true;
                        if(holdGhost != null){
                            float py = track == 0 ? laneY1 : laneY2;
                            HoldGhost tgh = Instantiate(holdGhost, new Vector3(playerGO.transform.position.x, py, 0), Quaternion.identity);
                            tgh.track = track;
                            tgh.endBeat = spawnedNotes[spawnedNotesInd + i].endBeat;
                        }
                        heldNotes[track] = spawnedNotes[spawnedNotesInd + i];
                        heldNotes[track].isHeld = true;
                        heldNotes[track].rootVisual.SetActive(false);
                    }

                    break;
                }
            }
        }
    }

    public void ReleaseKey(int track){
        //currentTrack = track;
        heldTracks[track] = false;
        if(heldNotes[track] != null){
            heldNotes[track].isHeld = false;
            if (Mathf.Abs(heldNotes[track].endBeat - songPosInBeats) > (notePressWindow * 1f)){
                //break combo
                this.hitAccuracy = 0.45f;
                this.rStats.hitCounts[4] += 1;
                combo = 0;
                Destroy(heldNotes[track].gameObject);
            }
        }
    }


    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;

        // pause and unpause game+music
        if (enabled) 
            {
                AudioListener.pause = false;
            }
            else 
            {
                AudioListener.pause = true;
            }
    }

}
