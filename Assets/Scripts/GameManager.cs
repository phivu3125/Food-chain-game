using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int initialMoney;
    public int maxLives;

    [SerializeField] private int currentMoney;
    [SerializeField] private int currentLives;

    private bool isWinGame = false;
    private int selectedLevel;
    

    public int CurrentLives { get => currentLives; set => currentLives = value; }
    public int CurrentMoney { get => currentMoney; set => currentMoney = value; }
    public bool IsWinGame { get => isWinGame; set => isWinGame = value; }
    public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }

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
        IsWinGame = false;

        ListenerManager.Instance.Broadcast(EventID.MONEY_CHANGED, CurrentMoney);
        ListenerManager.Instance.Broadcast(EventID.LIVE_CHANGED, CurrentLives);
        ListenerManager.Instance.Broadcast(EventID.IS_WIN_GAME_CHANGED, IsWinGame);
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

    public void WinGame()
    {
        if (!IsWinGame)
        {
            IsWinGame = true;
            ListenerManager.Instance.Broadcast(EventID.IS_WIN_GAME_CHANGED, IsWinGame);
            
            if (SelectedLevel == LevelMenu.unlockedLevel)
            {
                PlayerPrefs.SetInt("UnlockedLevel", ++SelectedLevel);
            }
        }
    }
}
