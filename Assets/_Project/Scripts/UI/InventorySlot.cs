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

        if(isShopSlot)
            CountBadge.SetActive(false);
        else
        {
            CountBadge.SetActive(true);
            CountText.text = $"X{InventoryManager.Instance.GetItemCount(itemData)}";
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
}