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
    //-------------------
    public float songBpm;  //170 for a long fall
    public float secPerBeat;
    public float firstBeatOffset;
    public static float songPos;
    public static float songPosInBeats;
    public float dspSongTime;

    public AudioSource musicSource;

    public List<Vector2> notes; //y is beat, x is position
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
    int lastanim;
    public float notePressWindow = 0.06f;

    void Awake(){
        instance = this;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameStateManager.Instance.SetState(GameState.Gameplay);
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
            MusicNote m = Instantiate(notePrefab, spawnpoint.position, Quaternion.identity);
            float ty = notes[nextIndex].x == 0 ? laneY1 : laneY2;
            m.Initialize(ty, startX, endX, notes[nextIndex].y, notes[nextIndex].x);
            spawnedNotes.Add(m);
            nextIndex++;
        }

        if(spawnedNotes.Count < 1 || spawnedNotesInd >= spawnedNotes.Count) return;

        if(spawnedNotes[spawnedNotesInd].beat < songPosInBeats - notePressWindow){
            spawnedNotesInd++;
        }
    }

    public float PosFromBeat(float beat){
        return (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
    }

    void GenerateNotes(){
        notes = new List<Vector2>();
        string fs = map.text;
        string[] maplines = fs.Split('\n');
        for(int i=0; i<maplines.Length; i++){
            string[] nn = maplines[i].Split(' ');
            float tx = float.Parse(nn[0]);
            float ty = float.Parse(nn[1]);
            notes.Add(new Vector2(tx, ty));
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
                if(Mathf.Abs(spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats) <= notePressWindow){
                    spawnedNotes[spawnedNotesInd+i].Hit();
                    hitAccuracy = spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats;
                    //print("hitaccuracy: " + hitAccuracy.ToString());
                    camshake.AddShake();
                    hitAudio.Play();
                    break;
                }
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
