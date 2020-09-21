using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Jump,
    JumpHeight,
    HealthCurrent,
    HealthMax,
    RunSpeed,
    WallJumpDistance,
    Glide,  //Enable or disable
    DashDistance,
    DashCooldown,
    DashForce,
    RockDamage,
    RockKnockback,
    RockMoveToSpeed,
    RockReturnSpeed,
    WeaponShootSpeed,
    WeaponAutomatic,    //Enable or disable
    WeaponMagazineSize,
    WeaponReloadTime,
    WeaponLaserSight,
    WeaponProjectilesPerShot,
    WeaponProjectilesSpread,
    WeaponProjectileRandomSpread, //Random for a big blast, or not random for precision
    WeaponProjectileSpeed,
    WeaponProjectileAcceleration,
    WeaponProjectileLifetime,
    WeaponProjectileDamage,
    WeaponProjectileKnockback
}

[CreateAssetMenu(fileName = "New Stat", menuName = "Item/Stat")]
public class Stat : ScriptableObject
{
    public StatType Type;
    public float Modifier;
    public bool BoolModifier;
    public Vector3 VectorModifier;
}
