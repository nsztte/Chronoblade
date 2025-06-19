using UnityEngine;

public enum WeaponType
{
    Sword,
    Pistol,
    Shotgun,
    Rifle
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("무기 기본 정보")]
    public string weaponName;
    public WeaponType weaponType;

    [Header("무기 성능")]
    public int damage;
    public float fireRate;  // 초당 발사 횟수
    public float range; // 사정거리
    //public float coolTime; // 쿨타임

    [Header("탄약 설정")]
    public int magazineSize;    // 탄창 크기
    public int maxAmmo;         // 최대 탄약 개수

    [Header("샷건 설정")]
    public int pelletCount = 1;
    public float spreadAngle = 0f;

    [Header("조준 설정")]
    public float aimFOV = 60f;

    [Header("반동 설정")]
    public float recoilX = 2f;  // 위로 튀는 정도
    public float recoilY = 1f;  // 좌우 흔들림
    public float recoilRecoverySpeed = 10f; // 반동 복구 속도
    [Range(0f, 1f)]
    public float aimRecoilMultiplier = 0.5f; // 조준 상태 반동 감소 배율

    [Header("UI")]
    public Sprite iconSprite; // 아이콘

    [Header("이펙트 및 사운드")]
    public GameObject muzzleFlash;
    public AudioClip fireSound;
}
