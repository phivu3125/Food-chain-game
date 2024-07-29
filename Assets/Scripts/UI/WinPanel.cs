using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private RectTransform winPanelRect;
    [SerializeField] private float topPos, middlePos;
    [SerializeField] private float tweenDuration;
    [SerializeField] private CanvasGroup darkPanelCanvasGroup;
    [SerializeField] private Ease animEase;

    public void Win()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        WinPanelIntro();
    }

    public void Return()
    {
        SceneManager.LoadScene("Level Selector");
        Time.timeScale = 1f;
    }

    private void WinPanelIntro()
    {
        darkPanelCanvasGroup.DOFade(1, tweenDuration).SetUpdate(true).SetEase(animEase);
        winPanelRect.DOAnchorPosY(middlePos, tweenDuration).SetEase(animEase).SetUpdate(true);
    }
}
