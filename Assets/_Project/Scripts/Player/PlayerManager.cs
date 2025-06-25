using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
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

    private Animator animator;

    #region Singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 플레이어 애니메이터 참조
            animator = GetComponent<Animator>();
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
        Debug.Log($"Player Take Damage: {damage}");
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

    // 애니메이터 제어 메서드
    public void SetAnimatorBool(string param, bool value)
    {
        animator.SetBool(param, value);
    }
    public void SetAnimatorTrigger(string param)
    {
        animator.SetTrigger(param);
    }
    public void SetAnimatorFloat(string param, float value)
    {
        animator.SetFloat(param, value);
    }
    public void SetAnimatorFloat(string param, float value, float dampTime, float deltaTime)
    {
        animator.SetFloat(param, value, dampTime, deltaTime);
    }

    // UpperBody 레이어에서 현재 재생 중인 애니메이션 클립의 길이 반환
    public float GetCurrentUpperBodyClipLength()
    {
        if (animator == null) return 0.3f;
        int upperBodyLayer = animator.GetLayerIndex("UpperBody");
        if (upperBodyLayer < 0) return 0.3f;
        var clips = animator.GetCurrentAnimatorClipInfo(upperBodyLayer);
        if (clips.Length > 0)
            return clips[0].clip.length;
        return 0.3f;
    }
}          
