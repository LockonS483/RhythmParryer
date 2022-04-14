using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// <a href="https://www.freepik.com/vectors/hex-background">Hex background vector created by coolvector - www.freepik.com</a>
public class EndScene : MonoBehaviour
{
     public Text perfect_count;
    public Text great_count;
    public Text ok_count;
    public Text bad_count;
    public Text miss_count;
    public Text max_combo;
     public Text accuracy;
     public Text score;
    public GameObject endSceneUI;

    // Update is called once per frame
    
    void Update()
    {
        if (endSceneUI.GetComponent<Canvas>().enabled) {
            perfect_count.text = "Perfect: " + Conductor.instance.rStats.hitCounts[0].ToString();
            great_count.text = "Great: " + Conductor.instance.rStats.hitCounts[1].ToString();
            ok_count.text = "Ok: " + Conductor.instance.rStats.hitCounts[2].ToString();
            bad_count.text = "Bad: " + Conductor.instance.rStats.hitCounts[3].ToString();
            miss_count.text = "Miss: " + Conductor.instance.rStats.hitCounts[4].ToString();
            score.text = "Score: " + Conductor.instance.score.ToString();
            accuracy.text = "Accuracy: " + Conductor.instance.hitAccuracy.ToString(); // accuracy
            max_combo.text = "Maximum Combo: " + Conductor.instance.rStats.highestCombo.ToString(); // max combo
        }
    }    
    // Start is called before the first frame update
    void Start()
    {
        // endSceneUI = GameObject.Find("Canvas");
        endSceneUI.GetComponent<Canvas>().enabled = false;
        //0:perfect, 1:great, 2:ok, 3:bad, 4:miss

    }
    public void Restart() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextScene()
    {
        print("herpaderp");
        // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //GameStateManager.Instance.SetState(GameState.Gameplay);
    }
    public void Resume()
    {
        // GameStateManager.Instance.SetState(GameState.Gameplay);
    }
}
