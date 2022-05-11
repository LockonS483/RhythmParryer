using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkParryMove : MonoBehaviour
{
    PlayerAnims anims;
    Vector3 playerPos;

    int anim = 0;
    float[] trackPos = new float[] {-2f, 2f};

    // Start is called before the first frame update
    void Start()
    {
        anims = GetComponent<PlayerAnims>();
        playerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerPos, 0.8f);
    }

    public void NoteCross(int track){
        anims.PlayAnim(anim%2);
        anim++;
        playerPos.y = trackPos[track];
    }
}