using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class LosePanel : MonoBehaviour
{
    [SerializeField] private RectTransform losePanelRect;
    [SerializeField] private float topPos, middlePos;
    [SerializeField] private float tweenDuration;
    [SerializeField] private CanvasGroup darkPanelCanvasGroup;


    public void Lose()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        LosePanelIntro();
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

    private void LosePanelIntro()
    {
        darkPanelCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        losePanelRect.DOAnchorPosY(middlePos, tweenDuration).SetUpdate(true);
    }
}
