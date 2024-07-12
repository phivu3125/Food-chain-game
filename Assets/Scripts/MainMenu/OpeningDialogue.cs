using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
            scrollBtn.SetActive(true);

            ReassignTextComponent();

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
            scrollBtn.SetActive(true);

            ReassignTextComponent();

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

    public void ReassignTextComponent()
    {
        GameObject parentObject = GameObject.Find("Note");
        if (parentObject != null){
            TextMeshProUGUI storyModeTransform = parentObject.transform.Find("StoryMode").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI challengeModeTransform = parentObject.transform.Find("ChallengeMode").GetComponent<TextMeshProUGUI>();
            textComponent = (textComponent == storyModeTransform) ? challengeModeTransform : storyModeTransform;
            LoadDialogueData();
            StartDialogue();
        }
    }
}
