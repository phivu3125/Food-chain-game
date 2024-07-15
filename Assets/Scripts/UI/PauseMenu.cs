using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private RectTransform pausePanelRect, pauseButtonRect;
    [SerializeField] private float topPos, middlePos;
    [SerializeField] private float tweenDuration;
    [SerializeField] private CanvasGroup darkPanelCanvasGroup;


    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        PausePanelIntro();
    }

    public async void Resume()
    {
        await PausePanelOutro();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        GameManager.Instance.ResetGameManager();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadScene("Level Selector");
        Time.timeScale = 1f;
    }

    private void PausePanelIntro()
    {
        darkPanelCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pausePanelRect.DOAnchorPosY(middlePos, tweenDuration).SetUpdate(true);
        pauseButtonRect.DOAnchorPosX(130, tweenDuration).SetUpdate(true);
    }

    private async Task PausePanelOutro()
    {
        darkPanelCanvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await pausePanelRect.DOAnchorPosY(topPos, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        pauseButtonRect.DOAnchorPosX(-105, tweenDuration).SetUpdate(true);
    }
}
