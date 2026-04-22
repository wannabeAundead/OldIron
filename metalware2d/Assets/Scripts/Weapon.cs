using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Game/Weapon")]
public class Weapon : ScriptableObject
{
    public string displayName = "Pistol";
    public string description = "A basic sidearm.";
    public float fireRate = 0.25f;
    public float bulletSpeed = 12f;
    public int bulletDamage = 25;
    public Color bulletColor = Color.white;
}
