using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public Weapon equippedWeapon;
    public Weapon defaultWeapon;

    public event Action<Weapon> OnWeaponChanged;

    void Start()
    {
        Weapon toEquip = equippedWeapon != null ? equippedWeapon : defaultWeapon;
        if (toEquip != null)
        {
            equippedWeapon = toEquip;
            OnWeaponChanged?.Invoke(equippedWeapon);
        }
    }

    public void Equip(Weapon w)
    {
        if (w == null) return;
        equippedWeapon = w;
        OnWeaponChanged?.Invoke(w);
    }
}
