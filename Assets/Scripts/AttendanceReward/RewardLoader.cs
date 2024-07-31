using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;


[System.Serializable]
public class Reward
{
    public string day;
    public string url;
    public string getDay() { return day; }
    public string getUrl() { return url; }
}

public class RewardList
{
    public List<Reward> rewards;

    public override string ToString()
    {
        return $"{string.Join(", ", rewards.Select(d => d.ToString()))}";
    }

    public List<Reward> GetRewards()
    {
        return rewards;
    }

}

public class RewardLoader : MonoBehaviour
{
    const string jsonFilePath = "Assets/Data/AttendanceReward.json";
    public Transform RewardContainer = null;
    private RewardList RewardList;
    private List<Reward> Rewards;
    int numDaysPresent = 0;
    void Start()
    {
        if (RewardContainer != null)
        {
            LoadJsonData();
            DisplayRewards();
            AddBtnComponentForRewardLabel();
        }
    }

    void Update()
    {
        AddBtnComponentForRewardLabel();
    }
    public void LoadJsonData()
    {
        string json = File.ReadAllText(jsonFilePath);
        RewardList = JsonUtility.FromJson<RewardList>(json);
        Rewards = RewardList.GetRewards();
    }

    void DisplayRewards()
    {
        for (int i = 0; i < RewardContainer.childCount; i++)
        {
            Transform RewardComponent = RewardContainer.GetChild(i);

            Reward reward = Rewards.Find(Reward => Reward.getDay() == (i + 1).ToString());

            Image icon = RewardComponent.Find("Item").GetComponent<Image>();

            if (reward == null)
            {
                System.Random random = new System.Random();
                int randomIndex = random.Next(Rewards.Count); // Lấy chỉ số ngẫu nhiên từ 0 đến số phần tử - 1

                Reward randomElement = Rewards[randomIndex]; // Lấy phần tử tại chỉ số ngẫu nhiên
            }

            icon.sprite = Resources.Load<Sprite>(reward.getUrl());
        }
    }

    void AddBtnComponentForRewardLabel()
    {
        numDaysPresent = (numDaysPresent) % RewardContainer.childCount;

        Transform RewardComponent = RewardContainer.GetChild(numDaysPresent);
        GameObject RewardObj = RewardComponent.gameObject;

        if (RewardObj != null)
        {
            Button button = RewardObj.GetComponent<Button>();
            if (button == null)
            {
                button = RewardObj.AddComponent<Button>();
                button.onClick.AddListener(() => RewardCompClicked(RewardComponent));
            }
        }
    }
    void RewardCompClicked(Transform RewardComponent)
    {
        Transform bg = RewardComponent.Find("Bg");
        Transform tick = RewardComponent.Find("Tick");

        bg.gameObject.SetActive(true);
        tick.gameObject.SetActive(true);
        ++numDaysPresent;
    }
}