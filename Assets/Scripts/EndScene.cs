using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour
{
    // public GameObject UI;
    // [SerializeField]
    //  public TextMeshPro perfect_count;
     public Text perfect_count;
     public Text great_count;
     public Text max_combo;
     public Text accuracy;
     public Text score;
     
    // Start is called before the first frame update
    void Start()
    {
        //0:perfect, 1:great, 2:ok, 3:bad, 4:miss
        perfect_count.text = "# of Perfect: " + Conductor.instance.rStats.hitCounts[0].ToString();
        great_count.text = "# of Great: " + Conductor.instance.rStats.hitCounts[1].ToString();
        score.text = "Net Score: " + Conductor.instance.score.ToString();
        accuracy.text = "Accuracy: " + Conductor.instance.hitAccuracy.ToString(); // accuracy
        max_combo.text = "Maximum Combo: " + Conductor.instance.rStats.highestCombo.ToString(); // max combo
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
