using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode[] track1;
    public KeyCode[] track2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Get current gamestate (in-game or paused)
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        for(int i=0; i<track1.Length; i++){
            if(Input.GetKeyDown(track1[i]) && currentGameState == GameState.Gameplay){
                //Debug.Log("p1");
                Conductor.instance.HitKey(0);
            }else if(Input.GetKeyUp(track1[i]) && currentGameState == GameState.Gameplay){
                //Debug.Log("release1");
                Conductor.instance.ReleaseKey(0);
            }
        }

        for(int i=0; i<track2.Length; i++){
            if(Input.GetKeyDown(track2[i]) && currentGameState == GameState.Gameplay){
                //Debug.Log("p2");
                Conductor.instance.HitKey(1);
            }else if(Input.GetKeyUp(track2[i]) && currentGameState == GameState.Gameplay){
                //Debug.Log("release2");
                Conductor.instance.ReleaseKey(1);
            }
        }
    }
}
