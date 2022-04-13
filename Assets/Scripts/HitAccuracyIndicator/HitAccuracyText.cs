using System.ComponentModel;
using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitAccuracyText : MonoBehaviour
{
    public GameObject hitAccuracyText;
    public GameObject textUI;

    String accText;
    float timer = 0.00f;
    float hitAccuracy;
    float hitAccuracy_new = 1;
    bool textShow = true;

    void Start() 
    {   
        // Get the text in the TextUI object
        textUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        // Get the hitAccuracy from Conductor
        //print("debugging: " + accText);
        hitAccuracyText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        //textUI.GetComponent<Text>().text = "yes";
        //print(textUI.GetComponent<Text>().text);
        if(hitAccuracyText.GetComponent<Conductor>().hitAccuracy != 0) {
            hitAccuracy_new = hitAccuracyText.GetComponent<Conductor>().hitAccuracy;
        }
        if (hitAccuracy_new < 0.13) {
            accText = "Perfect";
        }
        else if (hitAccuracy_new < 0.2){
            accText = "Great";
        } 
        else if (hitAccuracy_new < 0.3){
            accText = "OK";
        } 
        else if (hitAccuracy_new < 0.4){
            accText = "Bad";
        } 
        else {
            accText = "";
        }
        if (textShow == true) {
            textUI.GetComponent<TMPro.TextMeshProUGUI>().text = accText;
        }
        if(hitAccuracy != hitAccuracy_new) {
            textShow = true;
            CancelInvoke("clearText");
            hitAccuracy = hitAccuracy_new;
            Invoke("clearText", 2.0f);
            
        }
        //print("debugging: " + hitAccuracy);
        
        
    }

    void clearText(){
        textUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        accText = "";
        textShow = false;
    }
}
