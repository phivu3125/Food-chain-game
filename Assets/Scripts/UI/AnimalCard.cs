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
    public int spawnTime;
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _nameText;
    private GameObject _overlay;
    private bool _isEnoughMoney;
    private bool _isLoading = false;
    private float _targetSliderValue;

    private void Start()
    {
        InitializeComponents();
        SetInitialValues();
        RegisterEvents();
        CardDisplay(GameManager.Instance.CurrentMoney);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void InitializeComponents()
    {
        // Tìm và gán các thành phần
        _priceText = transform.Find("Price")?.GetComponent<TextMeshProUGUI>();
        _nameText = transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        _overlay = transform.Find("Overlay").gameObject;

        // Kiểm tra các thành phần
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
        SetMaxTimeSpawn(spawnTime);
    }

    private void RegisterEvents()
    {
        // Đăng ký sự kiện thay đổi tiền
        this.Register(EventID.MONEY_CHANGED, CardDisplay);
    }

    private void UnregisterEvents()
    {
        // Hủy đăng ký sự kiện thay đổi tiền
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
        // Thiết lập thời gian tối đa cho thanh trượt
        slider.maxValue = maxTime;
        slider.value = 0;
        _targetSliderValue = maxTime;
    }

    public void SetTimeSpawn(float timeNow)
    {
        // Cập nhật thời gian hiện tại cho thanh trượt
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
            // Không đủ tiền
            _priceText.color = Color.red;
            _overlay.SetActive(true);
            IsEnoughMoney = true;
        }
        else
        {
            // Đủ tiền
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
        // Bắt đầu bộ đếm thời gian load thẻ
        IsLoading = true;
        _overlay.SetActive(true);
        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        float timeRemaining = spawnTime;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            SetTimeSpawn(timeRemaining);
            yield return null;
        }

        // Đảm bảo giá trị mục tiêu là 0 khi kết thúc
        SetTimeSpawn(0);
        slider.value = 0;

        IsLoading = false;

        // Nếu đủ tiền thì tắt overlay, ngược lại bật overlay
        _overlay.SetActive(IsEnoughMoney);
    }

    private void Update()
    {
        if (IsLoading)
        {
            // Cập nhật giá trị thanh trượt một cách mượt mà
            slider.value = Mathf.Lerp(slider.value, _targetSliderValue, Time.deltaTime * 10);
        }
    }
}
