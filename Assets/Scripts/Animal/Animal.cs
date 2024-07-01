using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float moveSpeed = 2;
    public string[] _listAnimalEat;
    public string[] _listAnimalPoisonnedBy;
    private bool _canRun = false;
    public bool _isBeingAttacked = false;
    public bool _isAttacking = false;
    [SerializeField] private int pricePerKill;
    [SerializeField] private int _maxHealth;
    [SerializeField] private Animator _animator;
    private int _health;
    private HealthBar _healthBar;
    private GameObject _targetAnimalDestroy = null;
    private int _enemyAnimalPrice = 0;
    public bool CanRun { get => _canRun; set => _canRun = value; }
    public string CurrentAnimalAttackerName { get => _currentAnimalAttackerName; set => _currentAnimalAttackerName = value; }

    private String _currentAnimalAttackerName = null;

    private void Start()
    {
        InitializeHealth();
    }

    private void InitializeHealth()
    {
        _health = _maxHealth;
        _healthBar = transform.Find("Health")?.Find("HealthBar")?.GetComponent<HealthBar>();
        _healthBar?.SetMaxHealth(_maxHealth);
        _healthBar?.SetHealth(_maxHealth);
        _healthBar?.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleMovement();
        CheckHealth();
    }

    private void HandleMovement()
    {
        if (!_canRun) return;

        Vector3 direction = gameObject.CompareTag("AnimalPlayer") ? Vector3.right : Vector3.left;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (transform.position.x > 20 || transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            if (!_isBeingAttacked)
            {
                _isBeingAttacked = true;
                StartCoroutine(Die());
            }
        }
    }

    public Animal spawnAnimal(Vector3 spawnPos)
    {
        GameObject animalObject = Instantiate(gameObject, spawnPos, Quaternion.identity);
        Animal animalComponent = animalObject.GetComponent<Animal>();
        if (animalComponent == null)
        {
            animalComponent = animalObject.AddComponent<Animal>();
        }
        return animalComponent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isAttacking) return; // Nếu đang tấn công thì không xử lý va chạm khác

        if (_isBeingAttacked && !_listAnimalEat.Contains(_currentAnimalAttackerName)) return; // Nếu đang bị tấn công bởi con lớn hơn thì ko xử lí va chạm

        if (IsEnemyCollision(collision, out GameObject enemyAnimal))
        {
            HandleEnemyCollision(enemyAnimal);
        }
    }

    private bool IsEnemyCollision(Collider2D collision, out GameObject enemyAnimal)
    {
        enemyAnimal = null;

        if ((collision.gameObject.tag == "AnimalEnemy" && gameObject.tag == "AnimalPlayer") ||
            (collision.gameObject.tag == "AnimalPlayer" && gameObject.tag == "AnimalEnemy"))
        {
            Animal enemyAnimalComponent = collision.gameObject.GetComponent<Animal>();
            if (enemyAnimalComponent != null && !enemyAnimalComponent._isBeingAttacked)
            {
                enemyAnimal = collision.gameObject;
                return true;
            }
        }
        return false;
    }

    private void HandleEnemyCollision(GameObject enemyAnimal)
    {
        Animal enemyAnimalComponent = enemyAnimal.GetComponent<Animal>();
        _enemyAnimalPrice = enemyAnimalComponent.pricePerKill;

        if (_listAnimalEat.Contains(enemyAnimal.name))
        {
            if (!enemyAnimalComponent._isBeingAttacked)
            {
                enemyAnimalComponent._isBeingAttacked = true;
                enemyAnimalComponent.CurrentAnimalAttackerName = gameObject.name;

                if (_listAnimalEat.Contains(enemyAnimal.name) && enemyAnimalComponent._listAnimalEat.Contains(gameObject.name))
                {
                    PrepareForAttack(gameObject);
                }
                else
                {
                    PrepareForAttack(enemyAnimal);
                }
            }
        }
        else if (_listAnimalPoisonnedBy.Contains(enemyAnimal.name))
        {
            if (!enemyAnimalComponent._isBeingAttacked)
            {
                enemyAnimalComponent._isBeingAttacked = true;
                enemyAnimalComponent.CurrentAnimalAttackerName = gameObject.name;
                PrepareForAttack(enemyAnimal);
                StartCoroutine(ReduceHealthOverTime(5, 4));
            }
        }
    }

    private void PrepareForAttack(GameObject enemyAnimal)
    {
        _canRun = false;
        _isAttacking = true; // Đặt trạng thái tấn công
        _targetAnimalDestroy = enemyAnimal; // Gán đối tượng mục tiêu
        _animator?.SetTrigger("isAttacking");
    }

    private IEnumerator ReduceHealthOverTime(int damagePerTick, float interval)
    {
        _healthBar?.gameObject.SetActive(true);
        float timeSinceLastReduceHealth = 0f;
        float totalDmg = damagePerTick * interval;

        while (totalDmg > 0)
        {
            timeSinceLastReduceHealth += Time.deltaTime;
            if (timeSinceLastReduceHealth >= 1f)
            {
                _health -= damagePerTick;
                totalDmg -= damagePerTick;
                _healthBar?.SetHealth(_health);
                timeSinceLastReduceHealth = 0f;
            }
            yield return null;
        }

        if (_health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void DestroyTargetAnimal()
    {
        if (_targetAnimalDestroy != null)
        {
            Animal targetAnimalComponent = _targetAnimalDestroy.GetComponent<Animal>();
            if (targetAnimalComponent._listAnimalEat.Contains(gameObject.name))
            {
                if (targetAnimalComponent._targetAnimalDestroy != null)
                {
                    StartCoroutine(Die());
                }
            }
            else
            {
                StartCoroutine(targetAnimalComponent?.Die());
            }
            GameManager.Instance.AddMoney(_enemyAnimalPrice);
        }

        if (gameObject.Equals(_targetAnimalDestroy))// Nếu con vật tự gọi hàm Destroy, phải làm cho nó đứng yên khi chết.
        {
            _canRun = false;
        }
        else
        {
            _canRun = true;
            _isAttacking = false; // Đặt lại trạng thái tấn công
            _targetAnimalDestroy = null; // Xóa mục tiêu sau khi đã xử lý
        }
    }

    private IEnumerator Die()
    {
        _canRun = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        _animator?.SetTrigger("isDying");
        transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
        _enemyAnimalPrice = 0;
    }
}
