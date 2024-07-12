using UnityEngine;

public class MainMenuAnimController : MonoBehaviour
{
    public Animator animator;
    private float pauseTime;

    public void PauseAndPlayOtherAnimation(string animationClipName)
    {
        if (!animator.GetBool(animationClipName + "Runned")){
            // Lưu trữ thời điểm hiện tại của Animation 1
            AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            pauseTime = animStateInfo.normalizedTime;
            // Đánh dấu đã chạy
            animator.SetBool(animationClipName + "Runned", true);
            // Dừng Animation 1 và bắt đầu Animation 2
            animator.Play(animationClipName);
        }
    }

    public void ResumeAnimation()
    {
        animator.Play("MenuAnimation", 0, pauseTime);
    }
}
