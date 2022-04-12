using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitAccuracyText : MonoBehaviour
{
    public GameObject hitAccuracyText;

    void Start() 
    {   
        hitAccuracyText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(true)
        {
            hitAccuracyText.SetActive(false);
        }
        else 
        {
            hitAccuracyText.SetActive(true);
        }
    }
}
