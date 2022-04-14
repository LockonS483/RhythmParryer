using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public int score;
    //0:perfect, 1:great, 2:ok, 3:bad, 4:miss
    public int[] hitCounts = {0,0,0,0,0};
    public int highestCombo;
    void Awake()
    {
        Object.DontDestroyOnLoad(this); //:)        
    }

}
