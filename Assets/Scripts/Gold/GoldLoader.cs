using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

[System.Serializable]
public class Gold
{
    public int gold;
    public float money;
    public int getGold() { return gold; }
    public float getMoney() { return money; }
    public override string ToString()
    {
        return $"Gold: {gold}, Money: {money}";
    }
}

public class GoldList
{
    public List<Gold> items;

    public override string ToString()
    {
        return $"{string.Join(", ", items.Select(d => d.ToString()))}";
    }

    public List<Gold> Getitems()
    {
        return items;
    }

}

public class GoldLoader : MonoBehaviour
{
    const string jsonFilePath = "Assets/Data/Golds.json";
    public GameObject nextBtn = null;
    public GameObject prevBtn = null;
    public TextMeshProUGUI numPages = null;
    public TextMeshProUGUI pageOrderCurrent = null;
    public Transform GoldContainer = null;
    public TextMeshProUGUI price = null;
    // public Image icon = null;
    private GoldList GoldList;
    private List<Gold> Golds;
    private int GoldIndex = 0;
    private int GoldsPerPage;
    void Start()
    {
        if (nextBtn != null && prevBtn != null && pageOrderCurrent != null && numPages != null && GoldContainer != null && price != null)
        {
            LoadJsonData();
            DisplayGolds();
            AddBtnComponent();
            AddBtnComponentForGoldLabel();
        }
    }
    public void LoadJsonData()
    {
        string json = File.ReadAllText(jsonFilePath);
        GoldList = JsonUtility.FromJson<GoldList>(json);
        Golds = GoldList.Getitems();
        GoldsPerPage = Golds.Count;

        numPages.SetText(Math.Ceiling(Golds.Count * 1.0 / 7).ToString());
        pageOrderCurrent.SetText("1");
    }

    void DisplayGolds()
    {
        ResetDisplayGolds();
        int startIndex = GoldIndex * GoldsPerPage;

        for (int i = 0; i < GoldsPerPage; i++)
        {
            if (startIndex + i >= Golds.Count || i >= GoldContainer.childCount)
                break;

            Transform GoldComponent = GoldContainer.GetChild(i);
            TextMeshProUGUI nameText = GoldComponent.Find("Gold").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI moneyText = GoldComponent.Find("Money").GetComponent<TextMeshProUGUI>();


            nameText.text = "+" + Golds[startIndex + i].getGold().ToString();
            moneyText.text = Golds[startIndex + i].getMoney().ToString() + " $";
        }
    }

    void ResetDisplayGolds()
    {
        for (int i = 0; i < GoldContainer.childCount; i++)
        {
            Transform GoldComponent = GoldContainer.GetChild(i);
            TextMeshProUGUI nameText = GoldComponent.Find("Gold").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI moneyText = GoldComponent.Find("Money").GetComponent<TextMeshProUGUI>();

            nameText.text = "";
            moneyText.text = "";
        }
    }

    void AddBtnComponent()
    {
        if (nextBtn != null)
        {
            Button nextButton = nextBtn.GetComponent<Button>();
            if (nextButton == null)
                nextButton = nextBtn.AddComponent<Button>();
            nextButton.onClick.AddListener(OnNextBtnClicked);
        }
        if (prevBtn != null)
        {
            Button prevButton = prevBtn.GetComponent<Button>();
            if (prevButton == null)
                prevButton = prevBtn.AddComponent<Button>();
            prevButton.onClick.AddListener(OnPrevBtnClicked);
        }
    }

    void AddBtnComponentForGoldLabel()
    {
        for (int i = 0; i < GoldsPerPage; i++)
        {
            Transform GoldComponent = GoldContainer.GetChild(i);
            GameObject GoldObj = GoldComponent.gameObject;

            if (GoldObj != null)
            {
                Button button = GoldObj.GetComponent<Button>();
                if (button == null)
                {
                    button = GoldObj.AddComponent<Button>();
                }
                button.onClick.AddListener(() => GoldCompClicked(GoldComponent));
            }
        }

    }

    void GoldCompClicked(Transform GoldComponent)
    {
        TextMeshProUGUI nameText = GoldComponent.Find("Gold").GetComponent<TextMeshProUGUI>();

        if (nameText != null && price != null)
        {
            Gold gold = Golds.Find(Gold => "+" + Gold.getGold().ToString() == nameText.text);
            
            if (gold != null)
            {
                price.SetText(gold.getMoney().ToString());
            }
        }

    }

    void OnNextBtnClicked()
    {
        if ((GoldIndex + 1) * GoldsPerPage < Golds.Count)
        {
            GoldIndex++;
            DisplayGolds();
            pageOrderCurrent.SetText((GoldIndex + 1).ToString());
        }
    }

    void OnPrevBtnClicked()
    {
        if (GoldIndex > 0)
        {
            GoldIndex--;
            DisplayGolds();
            pageOrderCurrent.SetText((GoldIndex + 1).ToString());
        }
    }
}