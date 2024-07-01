using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        this.Register(EventID.MONEY_CHANGED, OnMoneyChanged);

        OnMoneyChanged(GameManager.Instance.GetCurrentMoney());
    }

    private void OnDestroy()
    {
        this.Unregister(EventID.MONEY_CHANGED, OnMoneyChanged);
    }

    private void OnMoneyChanged(object money)
    {
        int newAmount = (int)money;
        moneyText.text = "Money: " + newAmount;
    }
}
