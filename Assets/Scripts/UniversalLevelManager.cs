using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NumberOfLevels
{
    public GameObject[] enemyAnimals;
    public GameObject[] animalCards;
}

public class UniversalLevelManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Transform cardContainer;

    public NumberOfLevels[] numberOfLevels;
    void Awake()
    {
        int level = LevelMenu.selectedLevel;
        setEnemyAnimals(level);
        setAnimalCards(level);
        GameManager.Instance.SelectedLevel = level;
    }

    private void setEnemyAnimals(int level)
    {
        enemySpawner.enemyAnimals = numberOfLevels[level - 1].enemyAnimals;
    }

    private void setAnimalCards(int level)
    {
        foreach (var cardPrefab in numberOfLevels[level - 1].animalCards)
        {
            Instantiate(cardPrefab, cardContainer);
        }
    }
}
