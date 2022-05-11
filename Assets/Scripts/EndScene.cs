using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

// <a href="https://www.freepik.com/vectors/hex-background">Hex background vector created by coolvector - www.freepik.com</a>
public class EndScene : MonoBehaviour
{
    public bool firstLoad = true;
     public Text perfect_count;
    public Text great_count;
    public Text ok_count;
    public Text bad_count;
    public Text miss_count;
    public Text max_combo;
     public Text accuracy;
     public Text score;
    public GameObject endSceneUI;
    public GameObject Manager;

    string next_map_path = "Assets/Maps/NewTest.txt";
    string next_song_path = "Music/THE-PRIMALS";

    // Update is called once per frame
    
    void Update()
    {
        if (firstLoad == true && endSceneUI.GetComponent<Canvas>().enabled) {
            endSceneUI.GetComponent<Animator>().Play("level_transition_anim_anim", -1, 0f);
            firstLoad = false;
        }
        if (endSceneUI.GetComponent<Canvas>().enabled) {
            var c = GameObject.Find("Manager").GetComponent<Conductor>();
            if (c.score < c.targetScore)
            {
                var g = GameObject.Find("NextButton");
                if(g)
                    GameObject.Find("NextButton").SetActive(false);
            }
            perfect_count.text = "Perfect: " + Conductor.instance.rStats.hitCounts[0].ToString();
            great_count.text = "Great: " + Conductor.instance.rStats.hitCounts[1].ToString();
            ok_count.text = "Ok: " + Conductor.instance.rStats.hitCounts[2].ToString();
            bad_count.text = "Bad: " + Conductor.instance.rStats.hitCounts[3].ToString();
            miss_count.text = "Miss: " + Conductor.instance.rStats.hitCounts[4].ToString();
            score.text = "Total Score: " + Conductor.instance.score.ToString();
            accuracy.text = "Target Score: " + Conductor.instance.targetScore.ToString();
            max_combo.text = "Maximum Combo: " + Conductor.instance.rStats.highestCombo.ToString(); // max combo
        }
    }    
    // Start is called before the first frame update
    void Start()
    {
        // endSceneUI = GameObject.Find("Canvas");
        // endSceneUI.GetComponent<Canvas>().enabled = false;
        // endSceneUI.GetComponent<Animator>().Play("level_transition_anim_anim", -1, 0f);
        // GameObject.Find("EndScene").GetComponent<Animator>().Play("level_transition_anim_anim", -1, 0f);

        //0:perfect, 1:great, 2:ok, 3:bad, 4:miss
        
    }
    public void Restart() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        firstLoad = true;
    }
    public void NextScene()
    {

        /*string map = File.ReadAllText(next_map_path);
        StageController.map = map;

        AudioClip audioClip = Resources.Load<AudioClip>(next_song_path);
        StageController.musicClip = audioClip;
        */
        int ind = SceneManager.GetActiveScene().buildIndex;
        switch (ind) {
            case 2:
                SceneManager.LoadSceneAsync(3);
                break;
            case 3:
                SceneManager.LoadSceneAsync(4);
                break;
            case 4:
                SceneManager.LoadSceneAsync(5);
                break;
            case 5:
                SceneManager.LoadSceneAsync(6);
                break;
            case 6:
                SceneManager.LoadSceneAsync(0);
                break;
        }
        //GameStateManager.Instance.SetState(GameState.Gameplay);
        firstLoad = true;
    }
    public void Resume()
    {
        // GameStateManager.Instance.SetState(GameState.Gameplay);
    }
}
