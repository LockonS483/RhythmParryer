using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    string next_map_path;
    string next_song_path;
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void StartStage1() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartStage2() {
        next_map_path = "Assets/Maps/NewTest.txt";
        next_song_path = "Music/THE-PRIMALS";

        string map = File.ReadAllText(next_map_path);
        StageController.map = map;

        AudioClip audioClip = Resources.Load<AudioClip>(next_song_path);
        StageController.musicClip = audioClip;

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
