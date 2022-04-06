using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        if (currentGameState == GameState.Gameplay)
        {
            pauseMenuUI.SetActive(false);
        }
        else 
        {
            pauseMenuUI.SetActive(true);
        }
    }

    public void Restart()
    {
        UnityEngine.Debug.Log("Restarting...");
    }

    public void Resume()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void Exit()
    {   
        UnityEngine.Debug.Log("Quitting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }

}
