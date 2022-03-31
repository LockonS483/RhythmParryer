using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Conductor : MonoBehaviour
{
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
    public float notePressWindow = 0.06f;

    void Awake(){
        instance = this;
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
        songPos = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - songPlayOffset);

        songPosInBeats = songPos / secPerBeat;
        
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

        print("songpos: " + songPosInBeats.ToString());
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
        for(int i=0; i<6; i++){
            if((spawnedNotesInd+i) >= (spawnedNotes.Count)) continue;
            if(spawnedNotes[spawnedNotesInd+i] == null) continue;

            if(spawnedNotes[spawnedNotesInd+i].track == track){
                print("right track: " + track.ToString());
                print("diff: " + Mathf.Abs(spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats).ToString());
                if(Mathf.Abs(spawnedNotes[spawnedNotesInd+i].beat - songPosInBeats) <= notePressWindow){
                    spawnedNotes[spawnedNotesInd+i].Hit();
                    break;
                }
            }
        }
    }
}
