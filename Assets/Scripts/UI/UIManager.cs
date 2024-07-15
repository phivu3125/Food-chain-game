using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI livesText;
    public LosePanel losePanel;

    void Start()
    {
        this.Register(EventID.MONEY_CHANGED, OnMoneyChanged);
        this.Register(EventID.LIVE_CHANGED, OnLivesChanged);

        OnMoneyChanged(GameManager.Instance.CurrentMoney);
        OnLivesChanged(GameManager.Instance.maxLives);
    }

    private void OnMoneyChanged(object money)
    {
        int newAmount = (int)money;
        moneyText.text = "Money: " + newAmount;
    }

    private void OnLivesChanged(object lives)
    {
        int newLives = (int)lives;
        livesText.text = "Lives: " + newLives;

        if (newLives == 0)
        {
            losePanel.Lose();
        }
    }

    private void OnDestroy()
    {
        this.Unregister(EventID.LIVE_CHANGED, OnLivesChanged);
        this.Unregister(EventID.MONEY_CHANGED, OnMoneyChanged);
    }
}
