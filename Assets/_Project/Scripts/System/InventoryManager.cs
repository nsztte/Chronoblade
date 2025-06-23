using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
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

    public int GetAmmoCount(AmmoType type)
    {
        // Placeholder
        return 100;
    }

    public bool UseAmmo(AmmoType type, int amount)
    {
        // Placeholder  
        return true;
    }
}

public enum AmmoType
{
    None,
    PistolAmmo,
    RifleAmmo,
    ShotgunAmmo
}
