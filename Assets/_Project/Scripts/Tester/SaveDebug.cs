using UnityEngine;

public class SaveDebug : MonoBehaviour
{
    [SerializeField] private int slot = 99;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveManager.Instance.Save(slot);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            SaveManager.Instance.Load(slot);
        }
    }
}
