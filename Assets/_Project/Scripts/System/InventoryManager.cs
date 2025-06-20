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
        return 0;
    }
}

public enum AmmoType
{
    Pistol,
    Rifle,
    Shotgun
}
