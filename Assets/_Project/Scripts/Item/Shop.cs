using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    public ShopData shopData;

    public void Interact()
    {
        if(shopData == null)
        {
            Debug.LogError($"[Shop] shopData가 설정되지 않음: {name}");
            return;
        }

        Debug.Log($"[Shop] 상점 상호작용 시작: {name}");
        
        ShopManager.Instance.OpenShop(this);
    }
}
