using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public Animator animator;
    public string[] anims;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAnim(int ani){
        animator.Play(anims[ani], 0, 0f);
    }
}
