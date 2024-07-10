using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDialogue : DialogueManager
{
    public GameObject scrollBtn = null;
    public MainMenuAnimController animController;
  
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
            if (animController != null)
            {
                animController.ResumeAnimation();
            }
            else
            {
                Debug.LogError("Animator not assigned!");
            }
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
            if (animController != null)
            {
                animController.ResumeAnimation();
            }
            else
            {
                Debug.LogError("Animator not assigned!");
            }
        }
    }
}
