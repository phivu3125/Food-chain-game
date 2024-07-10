using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDialogue : DialogueManager
{
    public GameObject scrollBtn = null;
    MainMenuAnimController mainMenuAnimCtrl = null;
    void Start()
    {
        // Assuming you have a GameObject in your scene named "MainMenuController"
        GameObject mainMenuControllerObject = GameObject.Find("MainMenu");
        if (mainMenuControllerObject != null)
        {
            mainMenuAnimCtrl = mainMenuControllerObject.AddComponent<MainMenuAnimController>();
        }
        else
        {
            Debug.LogError("MainMenuController GameObject not found!");
        }
    }

    protected override void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Note.SetActive(false);
            index = 0;
            textComponent.text = lines[index];
            scrollBtn.SetActive(true);
            mainMenuAnimCtrl.Enable(); 
        }
    }

    protected override void PrevLine()
    {
        if (index > 0)
        {
            index--;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Note.SetActive(false);
            index = 0;
            textComponent.text = lines[index];
            scrollBtn.SetActive(true);
            if(mainMenuAnimCtrl != null){mainMenuAnimCtrl.Disable();}else{Debug.Log("anim is null");}
        }
    }
}
