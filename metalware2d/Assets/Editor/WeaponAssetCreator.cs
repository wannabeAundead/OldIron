using UnityEditor;
using UnityEngine;
using System.IO;

public static class WeaponAssetCreator
{
    [MenuItem("Game/Create Weapon Assets")]
    public static void CreateAll()
    {
        string folder = "Assets/Weapons";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        Weapon pistol = ScriptableObject.CreateInstance<Weapon>();
        pistol.displayName = "Pistol";
        pistol.description = "Reliable sidearm. Balanced damage and speed.";
        pistol.fireRate = 0.25f;
        pistol.bulletSpeed = 12f;
        pistol.bulletDamage = 25;
        pistol.bulletColor = new Color(1f, 1f, 1f, 1f);
        AssetDatabase.CreateAsset(pistol, folder + "/Pistol.asset");

        Weapon rifle = ScriptableObject.CreateInstance<Weapon>();
        rifle.displayName = "Rifle";
        rifle.description = "Rapid fire, lower damage per shot.";
        rifle.fireRate = 0.1f;
        rifle.bulletSpeed = 16f;
        rifle.bulletDamage = 12;
        rifle.bulletColor = new Color(1f, 0.85f, 0.2f, 1f);
        AssetDatabase.CreateAsset(rifle, folder + "/Rifle.asset");

        Weapon cannon = ScriptableObject.CreateInstance<Weapon>();
        cannon.displayName = "Cannon";
        cannon.description = "Slow, heavy-hitting shots.";
        cannon.fireRate = 0.8f;
        cannon.bulletSpeed = 9f;
        cannon.bulletDamage = 75;
        cannon.bulletColor = new Color(1f, 0.3f, 0.2f, 1f);
        AssetDatabase.CreateAsset(cannon, folder + "/Cannon.asset");

        Weapon smg = ScriptableObject.CreateInstance<Weapon>();
        smg.displayName = "SMG";
        smg.description = "Very fast fire rate. Low damage.";
        smg.fireRate = 0.06f;
        smg.bulletSpeed = 14f;
        smg.bulletDamage = 8;
        smg.bulletColor = new Color(0.3f, 1f, 0.9f, 1f);
        AssetDatabase.CreateAsset(smg, folder + "/SMG.asset");

        WeaponDatabase db = ScriptableObject.CreateInstance<WeaponDatabase>();
        db.allWeapons = new Weapon[] { pistol, rifle, cannon, smg };
        AssetDatabase.CreateAsset(db, folder + "/WeaponDatabase.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Weapon assets created under Assets/Weapons/");
    }
}
