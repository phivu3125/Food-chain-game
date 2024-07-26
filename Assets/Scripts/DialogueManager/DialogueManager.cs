using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

[System.Serializable]
public class DialogueData
{
    public string label;
    public string[] notes;
    public override string ToString()
    {
        return $"{label}: {string.Join(", ", notes)}";
    }
}

[System.Serializable]
public class DialogueDataList
{
    public DialogueData[] dialogues;

    public override string ToString()
    {
        return $"{string.Join(", ", dialogues.Select(d => d.ToString()))}";
    }

}

[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    protected GameObject nextBtn = null;
    protected GameObject prevBtn = null;
    protected GameObject Note = null;

    //protected GameObject panel; // Panel chứa tất cả các component
    const string jsonFilePath = "Assets/Data/Dialogue.json";
    protected DialogueDataList dialogueDataList;
    protected string[] lines;
    protected const float textSpeed = 0.1f;
    protected int index;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
        AddBtnComponentForComponents();
        LoadJsonData();
        LoadDialogueData();
        StartDialogue();
    }

    public void Init(){
        textComponent.text = string.Empty;
        nextBtn = transform.Find("NextBtn").gameObject;
        prevBtn = transform.Find("PrevBtn").gameObject;
        Note = transform.parent.gameObject;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            textComponent.text = string.Empty;
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }

    protected void AddBtnComponentForComponents()
    {
        if (nextBtn != null && nextBtn.GetComponent<Button>() == null)
        {
            nextBtn.AddComponent<Button>();
        }
        if (prevBtn != null && prevBtn.GetComponent<Button>() == null)
        {
            prevBtn.AddComponent<Button>();
        }

        AddButtonListeners();
    }

    protected void AddButtonListeners()
    {
        if (nextBtn != null)
        {
            nextBtn.GetComponent<Button>().onClick.AddListener(OnNextBtnClicked);
        }
        if (prevBtn != null)
        {
            prevBtn.GetComponent<Button>().onClick.AddListener(OnPrevBtnClicked);
        }
    }

    protected void OnNextBtnClicked()
    {
        NextLine();
    }

    protected void OnPrevBtnClicked()
    {
        PrevLine();
    }

    protected void LoadJsonData(){
        string json = File.ReadAllText(jsonFilePath);
        dialogueDataList = JsonUtility.FromJson<DialogueDataList>(json);
    }

    protected void LoadDialogueData()
    {
        textComponent.text = string.Empty;
        index = 0;

        foreach (DialogueData e in dialogueDataList.dialogues)
        {
            if ((e.label.Equals(textComponent.name, StringComparison.OrdinalIgnoreCase)))
            {
                lines = e.notes;
                return;
            }
        }
    }

    protected void StartDialogue()
    {
        StartCoroutine(TypeLine());
    }

    protected IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    protected virtual void NextLine()
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
        }
    }

    protected virtual void PrevLine()
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
        }
    }
}