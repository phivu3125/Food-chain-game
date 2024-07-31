using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

[System.Serializable]
public class Item
{
    public string name;
    public string use;
    public string effectiveTime;
    public string cooldownTime;
    public string price;
    public string url;
    public string getName() { return name; }
    public string getUrl() { return url; }
    public string getPrice() { return price; }

    public override string ToString()
    {
        return $"Name: {name}, Use: {use}, Effective Time: {effectiveTime}, Cooldown Time: {cooldownTime}, Price: {price}";
    }
}

public class ItemList
{
    public List<Item> items;

    public override string ToString()
    {
        return $"{string.Join(", ", items.Select(d => d.ToString()))}";
    }

    public List<Item> GetItems()
    {
        return items;
    }

}

public class ItemLoader : MonoBehaviour
{
    const string jsonFilePath = "Assets/Data/Items.json";
    public GameObject nextBtn = null;
    public GameObject prevBtn = null;
    public TextMeshProUGUI numPages = null;
    public TextMeshProUGUI pageOrderCurrent = null;
    public TextMeshProUGUI use = null;
    public Transform itemContainer = null;
    public TextMeshProUGUI price = null;
    public Image icon = null;
    private ItemList itemList;
    private List<Item> items;
    private int itemIndex = 0;
    private int itemsPerPage = 7;
    void Start()
    {
        if (nextBtn != null && prevBtn != null && pageOrderCurrent != null && numPages != null && itemContainer != null && use != null)
        {
            LoadJsonData();
            DisplayItems();
            AddBtnComponent();
            AddBtnComponentForItemLabel();
        }
    }
    public void LoadJsonData()
    {
        string json = File.ReadAllText(jsonFilePath);
        itemList = JsonUtility.FromJson<ItemList>(json);
        items = itemList.GetItems();

        numPages.SetText(Math.Ceiling(items.Count * 1.0 / 7).ToString());
        pageOrderCurrent.SetText("1");
    }

    void DisplayItems()
    {
        ResetDisplayItems();
        int startIndex = itemIndex * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            if (startIndex + i >= items.Count || i >= itemContainer.childCount)
                break;

            Transform itemComponent = itemContainer.GetChild(i);
            TextMeshProUGUI nameText = itemComponent.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI quantityText = itemComponent.Find("Quantity").GetComponent<TextMeshProUGUI>();

            nameText.text = items[startIndex + i].getName();
            quantityText.text = "0";
        }
    }

    void ResetDisplayItems()
    {
        for (int i = 0; i < itemContainer.childCount; i++)
        {
            Transform itemComponent = itemContainer.GetChild(i);
            TextMeshProUGUI nameText = itemComponent.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI quantityText = itemComponent.Find("Quantity").GetComponent<TextMeshProUGUI>();

            nameText.text = "";
            quantityText.text = "";
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

    void AddBtnComponentForItemLabel()
    {
        for (int i = 0; i < itemsPerPage; i++)
        {
            Transform itemComponent = itemContainer.GetChild(i);
            GameObject itemObj = itemComponent.gameObject;

            if (itemObj != null)
            {
                Button button = itemObj.GetComponent<Button>();
                if (button == null)
                {
                    button = itemObj.AddComponent<Button>();
                    button.onClick.AddListener(() => ItemCompClicked(itemComponent));
                }
            }
        }
    }

    void EnableAllBgSelectedObject()
    {
        for (int i = 0; i < itemContainer.childCount; i++)
        {
            Transform itemComponent = itemContainer.GetChild(i);
            Transform selected = itemComponent.Find("Selected");

            if (selected)
            {
                selected.gameObject.SetActive(false);
            }
        }
    }

    void ItemCompClicked(Transform itemComponent)
    {
        TextMeshProUGUI nameText = itemComponent.Find("Name").GetComponent<TextMeshProUGUI>();
        Transform selected = itemComponent.Find("Selected");

        use.SetText(items.Find(item => item.getName() == nameText.text).use);
        icon.sprite = Resources.Load<Sprite>(items.Find(item => item.getName() == nameText.text).getUrl());
        price.SetText(items.Find(item => item.getName() == nameText.text).getPrice());
        
        EnableAllBgSelectedObject();
        selected.gameObject.SetActive(true);
    }

    void OnNextBtnClicked()
    {
        if ((itemIndex + 1) * itemsPerPage < items.Count)
        {
            itemIndex++;
            DisplayItems();
            pageOrderCurrent.SetText((itemIndex + 1).ToString());
        }
    }

    void OnPrevBtnClicked()
    {
        if (itemIndex > 0)
        {
            itemIndex--;
            DisplayItems();
            pageOrderCurrent.SetText((itemIndex + 1).ToString());
        }
    }
}