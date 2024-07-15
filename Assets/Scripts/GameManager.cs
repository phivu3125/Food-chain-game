using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int initialMoney;
    public int maxLives;

    public int currentMoney;
    public int currentLives;

    public int CurrentLives { get => currentLives; set => currentLives = value; }
    public int CurrentMoney { get => currentMoney; set => currentMoney = value; }

    protected override void Awake()
    {
        base.Awake();
        ResetGameManager();
    }

    private void InitializeMoney()
    {
        CurrentMoney = initialMoney;
    }

    private void InitializePlayerLives()
    {
        CurrentLives = maxLives;
    }

    public void ResetGameManager()
    {
        InitializeMoney();
        InitializePlayerLives();
        
        ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, CurrentMoney);
        ListenerManager.Instance.Broadcast(EventID.LIVE_CHANGED, CurrentLives);
    }

    public bool SpendMoney(int amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, CurrentMoney);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, CurrentMoney);
    }

    public void AddLives(int amount)
    {
        CurrentLives += amount;
        ListenerManager.Instance.Broadcast(EventID.LIVE_CHANGED, CurrentLives);
    }

    public void DecreaseLives(int amount)
    {
        if (CurrentLives >= amount)
        {
            CurrentLives -= amount;
            ListenerManager.Instance.Broadcast(EventID.LIVE_CHANGED, CurrentLives);
        }
    }
}
