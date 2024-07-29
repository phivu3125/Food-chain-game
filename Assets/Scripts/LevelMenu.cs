using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelMenu : MonoBehaviour
{
    public static int selectedLevel;
    public static int unlockedLevel;
    private void Awake()
    {
        // PlayerPrefs.DeleteKey("UnlockedLevel");

        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        List<Button> buttons = GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < unlockedLevel)
            {
                buttons[i].interactable = true;
            }
            else{
                buttons[i].interactable = false;
            }
        }
    }

    public void OpenLevel(int level)
    {
        selectedLevel = level;
        SceneManager.LoadScene("Level 1");
    }
}
