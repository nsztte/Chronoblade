using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("HP")]
    public int maxHP = 100;
    public int currentHP;

    [Header("MP")]
    public int maxMP = 100;
    public int currentMP;

    [Header("Stamina")]
    public int maxStamina = 100;
    public int currentStamina;

    [Header("Currency")]
    public int gold = 0;

    #region Singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        currentStamina = maxStamina;

        // UI 업데이트
        UIManager.Instance?.UpdateHP(currentHP, maxHP);
        UIManager.Instance?.UpdateMP(currentMP, maxMP);
        UIManager.Instance?.UpdateStamina(currentStamina, maxStamina);
        UIManager.Instance?.UpdateGold(gold);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // UI 업데이트
        UIManager.Instance?.UpdateHP(currentHP, maxHP);

        if(currentHP <= 0)
        {
            Die();
        }
    }

    public void HealHP(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // UI 업데이트
        UIManager.Instance?.UpdateHP(currentHP, maxHP);
    }

    public void UseMP(int amount)
    {
        currentMP -= amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        // UI 업데이트
        UIManager.Instance?.UpdateMP(currentMP, maxMP);
    }

    public void RestoreMP(int amount)
    {
        currentMP += amount;
        currentMP = Mathf.Clamp(currentMP, 0, maxMP);

        // UI 업데이트
        UIManager.Instance?.UpdateMP(currentMP, maxMP);
    }

    public void UseStamina(int amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // UI 업데이트
        UIManager.Instance?.UpdateStamina(currentStamina, maxStamina);
    }

    public void RecoverStamina(int amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // UI 업데이트
        UIManager.Instance?.UpdateStamina(currentStamina, maxStamina);
    }

    public void AddGold(int amount)
    {
        gold += amount;

        // UI 업데이트
        UIManager.Instance?.UpdateGold(gold);
    }

    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;

            // UI 업데이트
            UIManager.Instance?.UpdateGold(gold);

            return true;
        }
        return false;
    }

    private void Die()
    {
        // 게임 오버 처리
        Debug.Log("플레이어 죽음");

        GameOver();
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("게임 오버");
        //TODO: 연출, 사운드, 애니메이션, 게임 오버 UI 표시
    }
}          
