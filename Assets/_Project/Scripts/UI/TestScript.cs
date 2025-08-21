using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            PlayerManager.Instance.AddGold(100);

        if(Input.GetKeyDown(KeyCode.O))
            PlayerManager.Instance.SpendGold(100);

    }
}
