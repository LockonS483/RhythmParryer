using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{   
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObj", lifetime);   
    }

    void DestroyObj(){
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
