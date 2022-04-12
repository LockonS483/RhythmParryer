using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitAccuracyText : MonoBehaviour
{
    public GameObject hitAccuracyText;

    private float accText;

    public GameObject textUI;

    void Start() 
    {   
        textUI.GetComponent<Text>().text = "yes";
        accText = hitAccuracyText.GetComponent<Conductor>().hitAccuracy;
        print("debugging: " + accText);
        hitAccuracyText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(true)
        {
            textUI.GetComponent<Text>().text = "yes";
            print(textUI.GetComponent<Text>().text);
            accText = hitAccuracyText.GetComponent<Conductor>().hitAccuracy;
            print("debugging: " + accText);
            hitAccuracyText.SetActive(true);
        }
        else 
        {
            hitAccuracyText.SetActive(true);
        }
    }
}
