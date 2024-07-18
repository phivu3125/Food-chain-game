using System.Collections;
using System.Linq;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float moveSpeed = 2; // Tốc độ di chuyển của động vật
    public string[] edibleAnimals; // Danh sách các động vật mà động vật này có thể ăn
    public string[] poisonedByAnimals; // Danh sách các động vật mà động vật này bị trúng độc

    [SerializeField] protected int pricePerKill; // Giá trị khi giết được động vật này
    [SerializeField] protected int maxHealth; // Máu tối đa của động vật này
    [SerializeField] protected Animator animator; // Animator của động vật
    [SerializeField] protected float animalPlayerBound; // Giới hạn của player animal
    [SerializeField] protected float animalEnemyBound; // Giới hạn của player animal



    protected int currentHealth; // Máu hiện tại của động vật
    protected HealthBar healthBar; // Thanh máu của động vật
    protected GameObject targetAnimal = null; // Đối tượng mục tiêu mà động vật này đang tấn công
    protected int targetAnimalPrice = 0; // Giá trị của đối tượng mục tiêu
    protected bool canRun = false; // Động vật có thể di chuyển hay không
    protected bool isBeingAttacked = false; // Động vật có đang bị tấn công hay không
    protected bool isAttacking = false; // Động vật có đang tấn công hay không
    protected string currentAttackerName = null; // Tên của động vật đang tấn công động vật này

    public bool CanRun { get => canRun; set => canRun = value; }
    public string CurrentAttackerName { get => currentAttackerName; set => currentAttackerName = value; }

    protected void Start()
    {
        InitializeHealth(); // Khởi tạo máu của động vật
    }

    protected void InitializeHealth()
    {
        currentHealth = maxHealth;
        healthBar = transform.Find("Health")?.Find("HealthBar")?.GetComponent<HealthBar>();
        healthBar?.SetMaxHealth(maxHealth);
        healthBar?.SetHealth(maxHealth);
        healthBar?.gameObject.SetActive(false); // Ẩn thanh máu khi khởi tạo
    }

    protected void Update()
    {
        HandleMovement(); // Xử lý di chuyển của động vật
        DestroyAnimalsOutOfBounds();
        CheckHealth(); // Kiểm tra tình trạng máu của động vật
    }

    protected void HandleMovement()
    {
        if (!canRun) return;

        Vector3 direction = gameObject.CompareTag("AnimalPlayer") ? Vector3.right : Vector3.left;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    protected void DestroyAnimalsOutOfBounds()
    {
        if (transform.position.x >= animalPlayerBound && transform.gameObject.CompareTag("AnimalPlayer"))
        {
            // Destroy(gameObject);
        }
        else if (transform.position.x <= animalEnemyBound && transform.gameObject.CompareTag("AnimalEnemy"))
        {
            // Destroy(gameObject);
            // GameManager.Instance.DecreaseLives(1);
        }
    }

    protected void CheckHealth()
    {
        if (currentHealth <= 0 && !isBeingAttacked)
        {
            isBeingAttacked = true;
            StartCoroutine(Die()); // Bắt đầu quá trình chết
        }
    }

    public Animal SpawnAnimal(Vector3 spawnPos)
    {
        GameObject animalObject = Instantiate(gameObject, spawnPos, Quaternion.Euler(30, 0, 0));
        animalObject.name = animalObject.name.Replace("(Clone)", "").Trim();
        Animal animalComponent = animalObject.GetComponent<Animal>();
        return animalComponent ?? animalObject.AddComponent<Animal>(); // Thêm component Animal nếu chưa có
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (!canRun || isAttacking || (isBeingAttacked && !edibleAnimals.Contains(currentAttackerName))) return;

        if (IsEnemyCollision(collision, out GameObject enemyAnimal))
        {
            HandleEnemyCollision(enemyAnimal);
        }
    }

    protected bool IsEnemyCollision(Collider collision, out GameObject enemyAnimal)
    {
        enemyAnimal = null;
        bool isEnemy = (collision.gameObject.tag == "AnimalEnemy" && gameObject.tag == "AnimalPlayer") ||
                       (collision.gameObject.tag == "AnimalPlayer" && gameObject.tag == "AnimalEnemy");

        if (isEnemy)
        {
            Animal enemyAnimalComponent = collision.gameObject.GetComponent<Animal>();
            if (enemyAnimalComponent != null && !enemyAnimalComponent.isBeingAttacked)
            {
                enemyAnimal = collision.gameObject;
                return true;
            }
        }
        return false;
    }

    protected void HandleEnemyCollision(GameObject enemyAnimal)
    {
        Animal enemyAnimalComponent = enemyAnimal.GetComponent<Animal>();
        targetAnimalPrice = enemyAnimalComponent.pricePerKill;

        if (edibleAnimals.Contains(enemyAnimal.name))
        {
            if (!enemyAnimalComponent.isBeingAttacked)
            {
                enemyAnimalComponent.isBeingAttacked = true;
                enemyAnimalComponent.CurrentAttackerName = gameObject.name;

                if (edibleAnimals.Contains(enemyAnimal.name) && enemyAnimalComponent.edibleAnimals.Contains(gameObject.name))
                {
                    PrepareForAttack(gameObject); // Tấn công qua lại
                }
                else
                {
                    PrepareForAttack(enemyAnimal); // Tấn công đơn phương
                }
            }
        }
        else if (poisonedByAnimals.Contains(enemyAnimal.name))
        {
            if (!enemyAnimalComponent.isBeingAttacked)
            {
                enemyAnimalComponent.isBeingAttacked = true;
                enemyAnimalComponent.CurrentAttackerName = gameObject.name;
                PrepareForAttack(enemyAnimal); // Tấn công khi bị trúng độc
                StartCoroutine(ReduceHealthOverTime(5, 4));
            }
        }
    }

    protected void PrepareForAttack(GameObject enemyAnimal)
    {
        canRun = false;
        isAttacking = true;
        targetAnimal = enemyAnimal;
        animator?.SetTrigger("isAttacking");
    }

    protected IEnumerator ReduceHealthOverTime(int damagePerTick, float interval)
    {
        healthBar?.gameObject.SetActive(true);
        float timeSinceLastReduceHealth = 0f;
        float totalDmg = damagePerTick * interval;

        while (totalDmg > 0)
        {
            timeSinceLastReduceHealth += Time.deltaTime;
            if (timeSinceLastReduceHealth >= 1f)
            {
                currentHealth -= damagePerTick;
                totalDmg -= damagePerTick;
                healthBar?.SetHealth(currentHealth);
                timeSinceLastReduceHealth = 0f;
            }
            yield return null;
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void DestroyTargetAnimal()
    {
        if (targetAnimal != null)
        {
            Animal targetAnimalComponent = targetAnimal.GetComponent<Animal>();
            if (targetAnimalComponent.edibleAnimals.Contains(gameObject.name))
            {
                if (targetAnimalComponent.targetAnimal != null)
                {
                    StartCoroutine(Die());
                }
            }
            else
            {
                StartCoroutine(targetAnimalComponent?.Die());
            }
            GameManager.Instance.AddMoney(targetAnimalPrice);
        }

        if (gameObject.Equals(targetAnimal)) // Nếu động vật tự gọi hàm Destroy, phải làm cho nó đứng yên khi chết.
        {
            canRun = false;
        }
        else
        {
            canRun = true;
            isAttacking = false;
            targetAnimal = null; // Xóa mục tiêu sau khi đã xử lý
        }
    }

    protected IEnumerator Die()
    {
        canRun = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        animator?.SetTrigger("isDying");
        // SetLayerForAllChildSprites(gameObject, "Dying");
        yield return null;
    }
    // private void SetLayerForAllChildSprites(GameObject parent, string sortingLayerName)
    // {
    //     SpriteRenderer[] spriteRenderers = parent.GetComponentsInChildren<SpriteRenderer>();

    //     foreach (var spriteRenderer in spriteRenderers)
    //     {
    //         spriteRenderer.sortingLayerName = sortingLayerName;
    //     }
    // }

    public void OnDyingAnimationEnd()
    {
        Destroy(gameObject);
        targetAnimalPrice = 0;
    }
}
