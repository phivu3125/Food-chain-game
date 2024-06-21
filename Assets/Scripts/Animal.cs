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
    [SerializeField]
    private int _maxHealth;
    private int _health;
    private HealthBar _healthBar;

    public bool CanRun { get => _canRun; set => _canRun = value; }
    private void Start()
    {
        _health = _maxHealth;
        _healthBar = transform.Find("Health")?.Find("HealthBar")?.GetComponent<HealthBar>();
        _healthBar?.SetMaxHealth(20);
        _healthBar?.SetHealth(20);
        _healthBar?.gameObject.SetActive(false);
    }

    // private void Start() {
    //     _healthBar = transform.Find();
    // }

    // private void OnEnable(){
    //     this.Register(EventID.ANIMAL_EAT_ENEMY, oneatanimal);
    // }

    // private void oneatanimal(object obj)
    // {
    //     throw new NotImplementedException();
    // }

    // private void OnDisable() {
    //     this.Unregister(EventID.ANIMAL_EAT_ENEMY, oneatanimal);
    // }

    // private void Start() {
    //     // _healthBar = gameObject.GetChildre
    // }

    private void Update()
    {
        if (_canRun && gameObject.CompareTag("AnimalPlayer"))
        {
            transform.position += (Vector3.right * moveSpeed) * Time.deltaTime;
            if (transform.position.x > 20)
            {
                Destroy(gameObject);
            }
        }
        if (gameObject.CompareTag("AnimalEnemy"))
        {
            transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
            if (transform.position.x < -10)
            {
                Destroy(gameObject);
            }
        }
        if (_health <= 0)
        {
            Destroy(gameObject);
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
        if (collision.gameObject.tag == "AnimalEnemy" && gameObject.tag == "AnimalPlayer")
        {
            // Check for Animal Player
            GameObject animalEnemy = collision.gameObject;

            if (_listAnimalEat.Contains(animalEnemy.name))
            {
                Destroy(animalEnemy);
            }
            else if (_listAnimalPoisonnedBy.Contains(animalEnemy.name))
            {
                Destroy(animalEnemy);
                StartCoroutine(ReduceHealthOverTime(5, 4));
            }
        }
        else if (collision.gameObject.tag == "AnimalPlayer" && gameObject.tag == "AnimalEnemy")
        {
            // Check for Animal Enemy
            GameObject animalPlayer = collision.gameObject;
            if (_listAnimalEat.Contains(animalPlayer.name))
            {
                Destroy(animalPlayer);
            }
            else if (_listAnimalPoisonnedBy.Contains(animalPlayer.name))
            {
                Destroy(animalPlayer);
                StartCoroutine(ReduceHealthOverTime(5, 4));
            }

        }
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



    // private void EatEnemy(){
    //     this.Broadcast(EventID.ANIMAL_EAT_ENEMY, this);
    // }
}
