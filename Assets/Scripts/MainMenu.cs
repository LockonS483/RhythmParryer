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
        SceneManager.LoadScene("Dialogue");
        //GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void StartStage1() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartStage2() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void StartStage3() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void StartStage4() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 4);
    }
}
