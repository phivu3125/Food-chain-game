using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCard : MonoBehaviour
{
    public Animal animal;
    [SerializeField] private int _costToDrop;
    public Slider slider;
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _nameText;
    private GameObject _overlay;
    public int timeSpawn;
    public bool _isEnoughMoney;
    public bool _isLoading = false;
    public float _targetSliderValue;

    private void Start()
    {
        InitializeComponents();
        SetInitialValues();
        RegisterEvents();
        CardDisplay(GameManager.Instance.GetCurrentMoney());
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void InitializeComponents()
    {
        _priceText = transform.Find("Price")?.GetComponent<TextMeshProUGUI>();
        _nameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        _overlay = transform.Find("Overlay").gameObject;

        if (_priceText == null)
        {
            Debug.LogError("Price Text is not assigned or cannot be found.");
            return;
        }

        if (_nameText == null)
        {
            Debug.LogError("Name Text is not assigned or cannot be found.");
            return;
        }
    }

    private void SetInitialValues()
    {
        _overlay.SetActive(false);
        _priceText.text = _costToDrop.ToString();
        _nameText.text = animal.transform.name;
        SetMaxTimeSpawn(timeSpawn);
    }

    private void RegisterEvents()
    {
        this.Register(EventID.MONEY_CHANGED, CardDisplay);
    }

    private void UnregisterEvents()
    {
        this.Unregister(EventID.MONEY_CHANGED, CardDisplay);
    }

    public int CostToDrop
    {
        get => _costToDrop;
        set => _costToDrop = value;
    }

    public bool IsEnoughMoney
    {
        get => _isEnoughMoney;
        set => _isEnoughMoney = value;
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => _isLoading = value;
    }

    public void SetMaxTimeSpawn(int maxTime)
    {
        slider.maxValue = maxTime;
        slider.value = 0;
        _targetSliderValue = maxTime;
    }

    public void SetTimeSpawn(float timeNow)
    {
        _targetSliderValue = timeNow;
    }

    public void CardDisplay(object money)
    {
        if (_priceText == null)
        {
            Debug.LogError("Price Text is not assigned or cannot be found.");
            return;
        }

        int currentMoney = (int)money;

        if (currentMoney < _costToDrop)
        {
            _priceText.color = Color.red;
            _overlay.SetActive(true);
            IsEnoughMoney = true;
        }
        else
        {
            _priceText.color = Color.white;
            IsEnoughMoney = false;
            if (!IsLoading)
            {
                _overlay.SetActive(false);
            }
        }
    }

    public void StartSpawnTimer()
    {
        IsLoading = true;
        _overlay.SetActive(true);
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        float timeRemaining = timeSpawn;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            SetTimeSpawn(timeRemaining);
            yield return null;
        }

        SetTimeSpawn(0);
        slider.value = 0;

        IsLoading = false;
        _overlay.SetActive(IsEnoughMoney);
    }

    private void Update()
    {
        if (IsLoading)
        {
            slider.value = Mathf.Lerp(slider.value, _targetSliderValue, Time.deltaTime * 10);
        }
    }
}
