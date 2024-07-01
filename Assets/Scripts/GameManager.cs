using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int currentMoney;
    public int initialMoney;

    protected override void Awake()
    {
        base.Awake();
        currentMoney = initialMoney;
        ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, currentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, currentMoney);
            // Debug.Log("Money spent: " + amount + ". Current money: " + currentMoney);
            return true;
        }
        else
        {
            // Debug.Log("Not enough money. Current money: " + currentMoney);
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, currentMoney);
        // Debug.Log("Money added: " + amount + ". Current money: " + currentMoney);
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}
