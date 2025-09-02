using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponTypeText;
    [SerializeField] private TextMeshProUGUI damageText;

    [Header("총기 전용")]
    [SerializeField] private GameObject gunStatsGroup;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI ammoTypeText;
    [SerializeField] private TextMeshProUGUI magazineSizeText;
    [SerializeField] private TextMeshProUGUI maxAmmoText;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void OnDisable() => Hide();

    public void Show(ItemData item, RectTransform slotRect)
    {
        if (item == null || item.itemType != ItemType.Equipment || item.weaponData == null)
        {
            Hide();
            return;
        }

        gameObject.SetActive(true);

        WeaponData weapon = item.weaponData;

        // 위치 설정
        Vector3[] corners = new Vector3[4];
        slotRect.GetWorldCorners(corners);
        Vector3 rightCenter = (corners[2] + corners[3]) / 2f;
        rectTransform.position = rightCenter + new Vector3(10f, 0f, 0f);

        weaponTypeText.text = $"타입: {weapon.weaponType}";
        damageText.text = $"공격력: {weapon.damage}";

        // 총기 여부 판별
        bool isGun = weapon.weaponType == WeaponType.Pistol ||
                     weapon.weaponType == WeaponType.Rifle ||
                     weapon.weaponType == WeaponType.Shotgun;

        gunStatsGroup.SetActive(isGun);

        if (isGun)
        {
            fireRateText.text = $"연사 속도: {weapon.fireRate:F2}";
            rangeText.text = $"사정거리: {weapon.range}";
            ammoTypeText.text = $"탄환 종류: {weapon.ammoType}";
            magazineSizeText.text = $"탄창 크기: {weapon.magazineSize}";
            maxAmmoText.text = $"최대 탄약: {weapon.maxAmmo}";
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
