using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimController : MonoBehaviour
{
    static Animator anim;
    // Update is called once per frame
    void Start()
    {
        anim = GetComponent<Animator>();
        if(anim == null){Debug.Log("Animator is null");}
    }

    public void Enable(){
        anim.SetBool("run", true);
    }

    public void Disable(){
        anim.SetBool("run", false);
    }
}
