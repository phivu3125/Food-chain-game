using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float moveSpeed;
    public String[] _listAnimalEat;
    public String[] _listAnimalPoisonnedBy;
    private bool _canRun = false;
    private bool _isBeingAttacked = false;
    private bool _isAttackedByPeer = false;

    [SerializeField] private int _maxHealth;
    [SerializeField] private Animator _animator;
    private int _health;
    private HealthBar _healthBar;
    private GameObject _targetAnimalDestroy = null;
    public bool CanRun { get => _canRun; set => _canRun = value; }

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
            _isBeingAttacked = true;
            StartCoroutine(Die());
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
            enemyAnimal = collision.gameObject;
            return true;
        }
        return false;
    }

    private void HandleEnemyCollision(GameObject enemyAnimal)
    {
        Animal enemyAnimalComponent = enemyAnimal.GetComponent<Animal>();
        if (_listAnimalEat.Contains(enemyAnimal.name) && !enemyAnimalComponent._isBeingAttacked)
        {
            // If 2 animal can eat each other
            if (enemyAnimalComponent._listAnimalEat.Contains(gameObject.name) && !enemyAnimalComponent._isAttackedByPeer)
            {
                enemyAnimalComponent._isBeingAttacked = true; // to block others enemies attack
                enemyAnimalComponent._isAttackedByPeer = true; // to block others same level enemies attack
                PrepareForAttack(gameObject);
            }
            else if (!_isBeingAttacked) // to block this animal attack others enemies when dying
            {
                enemyAnimalComponent._isBeingAttacked = true;
                PrepareForAttack(enemyAnimal);
            }
        }
        else if (_listAnimalPoisonnedBy.Contains(enemyAnimal.name) && !enemyAnimalComponent._isBeingAttacked)
        {
            enemyAnimalComponent._isBeingAttacked = true;
            PrepareForAttack(enemyAnimal);
            StartCoroutine(ReduceHealthOverTime(5, 4));
        }
    }

    private void PrepareForAttack(GameObject enemyAnimal)
    {
        _canRun = false;
        _targetAnimalDestroy = enemyAnimal;
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
            if (_targetAnimalDestroy.Equals(gameObject)) // check if target animal is gameobject, set it can't run
            {
                _canRun = false;
            }
            else
            {
                _canRun = true;
            }
            _targetAnimalDestroy = null;
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
    }
}