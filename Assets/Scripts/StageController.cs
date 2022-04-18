using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StageController : MonoBehaviour
{
    public static string map = File.ReadAllText("Assets/Maps/close-in-the-distance.txt");

    
    public static AudioClip musicClip;

    void Start() {
        Object.DontDestroyOnLoad(this);
    }

}
