using UnityEngine;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Levels : MonoBehaviour
{
    private GameObject parent = null;
    public LevelsAnimController animController;
    public void Start()
    {
        parent = GameObject.Find("LevelsBtn");

        AddBtnComponentForLevelBtns();
    }

    private void AddBtnComponentForLevelBtns()
    {
        foreach (Transform child in parent.transform)
        {
            if (child != null && child.gameObject.GetComponent<Button>() == null)
            {
                Button btn = child.gameObject.AddComponent<Button>();
                btn.onClick.AddListener(OnLevelBtnClicked);
            }
        }
    }

    private void OnLevelBtnClicked()
    {
        int levelIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        GameObject levelInfo = FindInactiveObjectByName("LevelInfo");
        GameObject intro = FindInactiveObjectByName("LevelIntro");

        Debug.Log(levelInfo);

        if (levelIndex == 1 && animController != null)
        {
            levelInfo.SetActive(true);
            intro.SetActive(true);
            animController.ResumeAnimation();
        }
        else if (levelIndex == 2)
        {
            Debug.Log("Level2");
        }
        else if (levelIndex == 3)
        {
            Debug.Log("Level3");
        }
        else if (levelIndex == 4)
        {
            Debug.Log("Level4");
        }
        else if (levelIndex == 5)
        {
            Debug.Log("Level5");
        }
        else if (levelIndex == 6)
        {
            Debug.Log("Level6");
        }
        else if (levelIndex == 7)
        {
            Debug.Log("Level7");
        }
        else if (levelIndex == 8)
        {
            Debug.Log("Level8");
        }
    }

    public static GameObject FindInactiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        foreach (Transform obj in objs)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == name)
            {
                return obj.gameObject;
            }
        }
        return null;
    }
}