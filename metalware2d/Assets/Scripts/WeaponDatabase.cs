using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Game/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    public Weapon[] allWeapons;

    public Weapon GetRandom()
    {
        if (allWeapons == null || allWeapons.Length == 0) return null;
        int i = Random.Range(0, allWeapons.Length);
        return allWeapons[i];
    }
}
