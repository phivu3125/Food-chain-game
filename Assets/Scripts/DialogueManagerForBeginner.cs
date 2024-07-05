using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerForBeginner : DialogueManager
{
    private GameObject menu = null;
    protected override void Start(){
        menu = GameObject.Find("MainMenu");
        textComponent.text = string.Empty;
        nextBtn = GameObject.Find("NextBtn");
        prevBtn = GameObject.Find("PrevBtn");
        Note = transform.parent.gameObject;

        AddBtnComponentForComponents();
        LoadDialogueData();

        //DisableAllExceptDialogue();
        StartDialogue();
        //EnableAllComponents();
    }

    public void DisableAllExceptDialogue()
    {
        foreach (Transform child in menu.transform)
        {
            if (child.gameObject != Note)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void EnableAllComponents()
    {
        foreach (Transform child in menu.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
