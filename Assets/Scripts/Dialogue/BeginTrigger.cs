using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginTrigger : MonoBehaviour
{
    /*    public Dialogue dialogue;

        void Start() {
            TriggerDialogue();
        }

        public void TriggerDialogue ()
        {
            DialogueManager d = FindObjectOfType<DialogueManager>();
            print(d);
            d.StartDialogue(dialogue);
        }*/
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("Lv1");
        }
    }
}
