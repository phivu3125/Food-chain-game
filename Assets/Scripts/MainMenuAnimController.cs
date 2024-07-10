using UnityEngine;

public class MainMenuAnimController : MonoBehaviour
{
    public Animator animator;
    private float pauseTime;

    public void PauseAndPlayOtherAnimation()
    {
        // Lưu trữ thời điểm hiện tại của Animation 1
        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        pauseTime = animStateInfo.normalizedTime;

        // Dừng Animation 1 và bắt đầu Animation 2
        animator.Play("MainMenuIdle");
    }

    public void ResumeAnimation()
    {
        animator.Play("MenuAnimation", 0, pauseTime + 0.1f);
    }
}
