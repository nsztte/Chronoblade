using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject CountBadge;
    [SerializeField] private TextMeshProUGUI CountText;
    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private InfoTooltipTrigger infoTooltip;
    [SerializeField] private GameObject equippedMarker;
    private Button button;

    [HideInInspector] public ItemData item;
    public UnityAction<InventorySlot> onClick;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Set(ItemData itemData, bool isShopSlot = false)
    {
        item = itemData;
        iconImage.sprite = item.Icon;
        nameText.text = item.itemName;
        SetSelected(false);

        if(itemData.itemType == ItemType.Equipment)
        {
            infoTooltip.gameObject.SetActive(true);
            infoTooltip.SetItem(item);
        }
        else
            infoTooltip.gameObject.SetActive(false);
            

        UpdateCountBadge(isShopSlot);

        if (isShopSlot)
            equippedMarker.SetActive(false);
        else
        {
            bool IsEquipped = InventoryManager.Instance.IsEquipped(itemData);
            equippedMarker.SetActive(IsEquipped);
        }
        
    }

    public void Bind()
    {
        button.onClick.AddListener(() => onClick?.Invoke(this));
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionFrame != null)
            selectionFrame.SetActive(isSelected);
    }

    public void RefreshEquippedMarker()
    {
        if (equippedMarker == null || item == null)
            return;

        bool isEquipped = InventoryManager.Instance.IsEquipped(item);
        equippedMarker.SetActive(isEquipped);
    }

    public void RefreshCountBadge()
    {
        UpdateCountBadge(false);
    }

    private void UpdateCountBadge(bool isShopSlot)
    {
        if (CountBadge == null || CountText == null)
            return;

        if (isShopSlot)
        {
            CountBadge.SetActive(false);
            return;
        }

        CountBadge.SetActive(true);

        // 무기 + 탄약 사용하는 경우 → 탄약 수 표시
        if (item != null &&
            item.itemType == ItemType.Equipment &&
            item.weaponData != null &&
            item.weaponData.ammoType != AmmoType.None)
        {
            int ammo = InventoryManager.Instance.GetAmmoCount(item.weaponData.ammoType);
            CountText.text = $"{ammo}";
        }
        else
        {
            // 기존 일반 아이템 수량
            int count = InventoryManager.Instance.GetItemCount(item);
            CountText.text = $"X{count}";
        }
    }
}