using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public GameObject playerGO;
    public PlayerAnims playerAnims;
    public CameraShake camshake;
    public AudioSource hitAudio;
    public GameObject recorder;
    public Recorder rStats;
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

    public float laneY1;
    public float laneY2;
    public float startX;
    public float endX;


    public TextAsset map;
    public Transform spawnpoint;


    List<MusicNote> spawnedNotes;
    int spawnedNotesInd;
    int currentTrack = 0;
    bool[] heldTracks= new bool[]{false, false};
    MusicNote[] heldNotes = new MusicNote[2];
    int lastanim;
    public float notePressWindow = 0.06f;

    public int combo;
    public int score;
    int scoreAmnt = 100;
    float endTimer = 2.0f;
    float endMarker;
    void Awake(){
        instance = this;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.SetState(GameState.Gameplay);
        var rObj = Instantiate(recorder);
        rStats = rObj.GetComponent<Recorder>();
    }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        spawnedNotes = new List<MusicNote>();
        GenerateNotes();
        nextIndex = 0;
        spawnedNotesInd = 0;
        score = 0;
        combo = 0;
        Invoke("StartMusic", songPlayOffset);
    }

    void StartMusic(){
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
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
            MusicNote m = Instantiate(notePrefab, spawnpoint.position, Quaternion.identity);
            float ty = notes[nextIndex].x == 0 ? laneY1 : laneY2;

            NoteTypes nt = NoteTypes.single;
            if(Mathf.RoundToInt(otherNoteInfo[nextIndex].x) == 1){
                nt = NoteTypes.hold;
            }

            m.Initialize(ty, startX, endX, notes[nextIndex].y, notes[nextIndex].x, otherNoteInfo[nextIndex].y, nt);
            spawnedNotes.Add(m);
            nextIndex++;
        }
        //print(musicSource.time);
        if (musicSource.time<=0 && spawnedNotes.Count>0) {
            if (endTimer > 0)
            {
                endTimer -= Time.deltaTime;
            }
            else {
                GameObject.Find("EndScene").GetComponent<Canvas>().enabled = true;
            }
        }

        if (spawnedNotes.Count < 1 || spawnedNotesInd >= spawnedNotes.Count) {
            return;
        }

        if(spawnedNotes[spawnedNotesInd].beat < songPosInBeats - notePressWindow){
            spawnedNotesInd++;
        }
        
    }

    public float PosFromBeat(float beat){
        return (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
    }

    void GenerateNotes(){
        notes = new List<Vector2>();
        otherNoteInfo = new List<Vector3>();
        string fs = map.text;
        string[] maplines = fs.Split('\n');
        var metadata = maplines[0].Split(' ');
        endMarker = float.Parse(metadata[2]);
        //print(endMarker);
        float offset = float.Parse(metadata[1]);
        for(int i=1; i<maplines.Length; i++){
            string[] nn = maplines[i].Split(' ');
            float tx = float.Parse(nn[0]);
            float ty = float.Parse(nn[2]) + offset;
            print(tx + " " + ty);
            notes.Add(new Vector2(tx, ty));
            
            //additional info (TYPEVAL: 0 = single, HOLD = 1)
            int typeVal = nn[1] == "SINGLE" ? 0 : 1;
            float endBeat = 0;
            if(typeVal == 1){
                endBeat = float.Parse(nn[3]) + offset;
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

            if(spawnedNotes[spawnedNotesInd+i].track == track){
                //print("right track: " + track.ToString());
                //print("diff: " + Mathf.Abs(spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats).ToString());
                if (Mathf.Abs(spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats) <= notePressWindow){
                    spawnedNotes[spawnedNotesInd + i].Hit();
                    hitAccuracy = spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats;
                    combo += 1;
                    if (combo > rStats.highestCombo)
                        rStats.highestCombo = combo;
                    //print("hitaccuracy: " + hitAccuracy.ToString());
                    switch (hitAccuracy) {
                        case float f when f< 0.13f:
                            score += (int)(1 / 0.13f) * scoreAmnt;
                            rStats.hitCounts[0] += 1;
                            break;
                        case float f when f< 0.2f:
                            score += (int)(1 / 0.2f) * scoreAmnt;
                            rStats.hitCounts[1] += 1;
                            break;
                        case float f when f < 0.3f:
                            score += (int)(1 / 0.3f) * scoreAmnt;
                            rStats.hitCounts[2] += 1;
                            break;
                        case float f when f < 0.4f:
                            score += (int)(1 / 0.4f) * scoreAmnt;
                            rStats.hitCounts[3] += 1;
                            break;
                        case float f when f < 0.5f:
                            score += (int)(1 / 0.5f) * scoreAmnt;
                            rStats.hitCounts[4] += 1;
                            break;
                    }
                    //print(score);
                    rStats.score = score;
                    camshake.AddShake();
                    hitAudio.Play();
                    spawnedNotes[spawnedNotesInd + i].Hit();
                    hitAccuracy = spawnedNotes[spawnedNotesInd + i].beat - songPosInBeats;

                    heldTracks[track] = true;
                    heldNotes[track] = spawnedNotes[spawnedNotesInd + i];
                    heldNotes[track].isHeld = true;
                    heldNotes[track].rootVisual.SetActive(false);

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
