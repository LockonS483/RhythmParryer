using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public GameObject dialogueCanvas;
    private Animator anim;
    private Queue<string> sentences;

    void Awake()
    {
        sentences = new Queue<string>();
        anim = dialogueCanvas.GetComponent<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //print("Starting conversation with " + dialogue.name);
        
        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            print("sentence: " + sentence + " enqueued");
        }
        print("sentence count: " + sentences.Count);

        DisplayNextSentence();
    }

    public void DisplayNextSentence() 
    {
        print("sentence count: " + sentences.Count);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        anim.Play("Dialogue_anim_anim",  -1, 0f);
    }

    void EndDialogue()
    {
        print("Conversation ended");
        SceneManager.LoadScene("Lv1");
    }

    
}
