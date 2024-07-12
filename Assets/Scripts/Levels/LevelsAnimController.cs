using UnityEngine;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelsAnimController : MonoBehaviour
{
    public Animator animator;
    private float pauseTime;
    private GameObject parent = null;
    private float speed = 0;
    bool StartingDialogue = false;
    public void Start()
    {
        speed = animator.speed;
    }


    public void PauseAnimation()
    {
        if (!StartingDialogue)
        {
            // Lưu trữ thời điểm hiện tại của Animation 1
            AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            pauseTime = animStateInfo.normalizedTime;
            // Dừng Animation 1 và bắt đầu Animation 2
            animator.speed = 0;
            StartingDialogue = true;
        }
    }

    public void ResumeAnimation()
    {
        animator.Play("LevelsAnimation", 0, pauseTime);
        animator.speed = speed;
    }
}
