using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimationGuideTrigger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnButtonPress()
    {
        animator.SetTrigger("ButtonPressed");
    }
}
