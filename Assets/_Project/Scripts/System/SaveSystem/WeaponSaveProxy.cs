using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class MagEntry
    {
        public int slot;   // weaponSlots 인덱스
        public int ammo;   // 해당 총기의 currentAmmo
    }

    [Serializable]
    private class Data
    {
        public int equippedIndex = -1;   //현재 장착 인덱스
        public List<MagEntry> mags = new();
    }

    private WeaponManager Weapon => WeaponManager.Instance;

    public override string CaptureStateJson()
    {
        var d = new Data
        {
            equippedIndex = Weapon.GetCurrentWeaponIndex() // 현재 장착 인덱스 저장
        };

        var slots = Weapon.GetWeaponSlots();
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i] is GunWeaponController gun)
            {
                d.mags.Add(new MagEntry
                {
                    slot = i,
                    ammo = gun.GetCurrentAmmoCount() // 현재 탄창 수 저장
                });
            }
        }

        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        var d = JsonUtility.FromJson<Data>(json);

        if (d.equippedIndex >= 0)
        {
            if (!Weapon.EquipWeapon(d.equippedIndex))
            {
                Weapon.UnEquipWeapon(); // 장착 실패 시 확실히 비장착 상태로
            }
        }
        else
        {
            Weapon.UnEquipWeapon();
        }

        var slots = Weapon.GetWeaponSlots();
        foreach (var m in d.mags)
        {
            if (m.slot < 0 || m.slot >= slots.Count) continue;
            if (slots[m.slot] is GunWeaponController gun)
            {
                gun.SetCurrentAmmo(m.ammo);
            }
        }

        if (WeaponManager.Instance.CurrentWeapon is GunWeaponController equippedGun)
        {
            equippedGun.UpdateAmmoCount();
        }
    }
}
