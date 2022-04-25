using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Start() {
        TriggerDialogue();
    }

    public void TriggerDialogue ()
    {
        DialogueManager d = FindObjectOfType<DialogueManager>();
        print(d);
        d.StartDialogue(dialogue);
    }
}
