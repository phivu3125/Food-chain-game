using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelText;
    private void Start()
    {
        levelText.text = level.ToString();
    }
    
    public void OpenScence()
    {
        SceneManager.LoadScene("Level " + level.ToString());
    }
}
