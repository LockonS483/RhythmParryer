using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitAccuracyText : MonoBehaviour
{
    public GameObject hitAccuracyText;
    public TMPro.TextMeshProUGUI textUI;
    public TMPro.TextMeshProUGUI comboText;
    public TMPro.TextMeshProUGUI scoreText;
    Conductor cond;

    String accText;
    float timer = 1.00f;
    float hitAccuracy;
    float hitAccuracy_new = 1;
    public Transform scaleTransform;
    public float scaleFactor;

    void Start() 
    {   
        // Get the text in the TextUI object
        textUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        cond = GameObject.Find("Manager").GetComponent<Conductor>();
        // Get the hitAccuracy from Conductor
        //print("debugging: " + accText);
        scaleTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //textUI.GetComponent<Text>().text = "yes";
        //print(textUI.GetComponent<Text>().text);
        if (hitAccuracyText.GetComponent<Conductor>().hitAccuracy != 0) {
            hitAccuracy_new = hitAccuracyText.GetComponent<Conductor>().hitAccuracy;
        }
        if (hitAccuracy_new < 0.13)
        {
            accText = "Perfect";
        }
        else if (hitAccuracy_new < 0.2)
        {

            accText = "Great";
        }
        else if (hitAccuracy_new < 0.3)
        {

            accText = "OK";
        }
        else if (hitAccuracy_new < 0.4)
        {
            accText = "Bad";
        }
        else if (hitAccuracy_new < 0.5)
        {
            accText = "Miss";
            cond.combo = 0;
        }
        else { 
            accText = "";
        }
            
        
        textUI.text = accText;
        comboText.text = "" + cond.combo;
        scoreText.text = "" + cond.score;
        if (hitAccuracy != hitAccuracy_new){
            hitAccuracy = hitAccuracy_new;
            timer = 1f;
        }
        
        if (timer > 0){
            timer -= Time.deltaTime * 4f;
            scaleTransform.gameObject.SetActive(true);
            float scaleVal = 1f + (timer * scaleFactor);//Mathf.Pow(Mathf.Clamp(timer,0,1), scaleFactor);
            Vector3 textScale = new Vector3(scaleVal, scaleVal, 1f);
            
            scaleTransform.localScale = textScale;

            //change alpha
            Color color = textUI.color;
            color.a = timer;
            textUI.color = color;
        }else{
            scaleTransform.gameObject.SetActive(false);
            /*textUI.text = "";
            accText = "";
            timer = 1f;*/
        }
        //print("debugging: " + hitAccuracy);
    }
}
