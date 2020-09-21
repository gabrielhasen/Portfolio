using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemObject;
    public ChunkMinimapInfo minimapInfo;
    private bool hasPickedUp;

    private void Start()
    {
        hasPickedUp = false;
    }

    /// <summary>
    /// When the player collides, checks what type of Item is attached to
    /// apply logic.
    /// </summary>
    /// <param name="collider">Checks if player is colliding</param>
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (hasPickedUp) 
            return;

        if (!collider.CompareTag("Player")) {
            return;
        }

        //
        //ADD ITEM PICKUP LOGIC TO GO TO THE FIELD NOTEBOOK HERE
        //

        hasPickedUp = true;
        CorgiEngineEvent.Trigger(CorgiEngineEventTypes.ItemPickup);

        switch (itemObject.type) {
            case ItemType.currency:
                CurrencyPickup();
                break;
            case ItemType.bomb:
                BombPickup();
                break;
            case ItemType.healthPickup:
                HealthPickup();
                break;
            case ItemType.buff:
                BuffPickup();
                break;
            default:
                break;
        }
    }

    private void MinimapPickup()
    {
        if (minimapInfo == null) 
            return;

        minimapInfo.PickedUpItem();
    }

    private void CurrencyPickup()
    {
        PlayerInventory.Instance.CurrencyUp(itemObject.SciencePoints);
        MinimapPickup();
        Destroy(gameObject);
    }

    private void BombPickup()
    {
        PlayerInventory.Instance.BombPickup();
        MinimapPickup();
        Destroy(gameObject);
    }

    private void HealthPickup()
    {
        //if current health is equal to max do not pickup this item and don't destroy it
        if(PlayerUpgrades.Instance.GetCurrentHP() == PlayerUpgrades.Instance.GetMaxHP()) {
            hasPickedUp = false;
            return;
        }

        //Apply all stats from this item (Is how hp increase gets applied)
        ApplyAllStats();

        MinimapPickup();
        Destroy(gameObject);
    }

    private void BuffPickup()
    {
        ApplyAllStats();
        PlayerInventory.Instance.BuffAdded(itemObject);
        Destroy(gameObject);
    }

    private void ApplyAllStats()
    {
        foreach (Stat item in itemObject.statList) {
            StatApply(item);
        }
    }

    private void StatApply(Stat stat)
    {
        switch (stat.Type) {
            case StatType.Jump:
                PlayerUpgrades.Instance.Upgrade_JumpsUp((int)stat.Modifier);
                break;
            case StatType.JumpHeight:
                PlayerUpgrades.Instance.Upgrade_JumpHeight(stat.Modifier);
                break;
            case StatType.HealthCurrent:
                PlayerInventory.Instance.HealthCurrentUp((int)stat.Modifier);
                break;
            case StatType.HealthMax:
                PlayerInventory.Instance.HealthMaxUp((int)stat.Modifier);
                break;
            case StatType.RunSpeed:
                PlayerUpgrades.Instance.Upgrade_RunSpeed(stat.Modifier);
                break;
            case StatType.WallJumpDistance:
                PlayerUpgrades.Instance.Upgrade_WallJumpDistance(stat.VectorModifier);
                break;
            case StatType.Glide:
                if(stat.BoolModifier)
                    PlayerUpgrades.Instance.Upgrade_EnableGlide();
                else
                    PlayerUpgrades.Instance.Upgrade_DisableGlide();
                break;
            case StatType.DashDistance:
                PlayerUpgrades.Instance.Upgrade_DashDistance(stat.Modifier);
                break;
            case StatType.DashCooldown:
                PlayerUpgrades.Instance.Upgrade_DashCooldown(stat.Modifier);
                break;
            case StatType.DashForce:
                PlayerUpgrades.Instance.Upgrade_DashForce(stat.Modifier);
                break;
            case StatType.RockDamage:
                PlayerUpgrades.Instance.Upgrade_RockDamage((int)stat.Modifier);
                break;
            case StatType.RockKnockback:
                PlayerUpgrades.Instance.Upgrade_RockKnockback(stat.VectorModifier);
                break;
            case StatType.RockMoveToSpeed:
                PlayerUpgrades.Instance.Upgrade_RockMoveToSpeedk(stat.Modifier);
                break;
            case StatType.RockReturnSpeed:
                PlayerUpgrades.Instance.Upgrade_RockReturnSpeed(stat.Modifier);
                break;
            case StatType.WeaponShootSpeed:
                PlayerUpgrades.Instance.Upgrade_WeaponShootSpeed(stat.Modifier);
                break;
            case StatType.WeaponAutomatic:
                if(stat.BoolModifier)
                    PlayerUpgrades.Instance.Upgrade_WeaponAutomatic();
                else
                    PlayerUpgrades.Instance.Upgrade_WeaponSemiAutomatic();
                break;
            case StatType.WeaponMagazineSize:
                PlayerUpgrades.Instance.Upgrade_WeaponMagazineSize((int)stat.Modifier);
                break;
            case StatType.WeaponReloadTime:
                PlayerUpgrades.Instance.Upgrade_WeaponReloadTime(stat.Modifier);
                break;
            case StatType.WeaponLaserSight:
                if (stat.BoolModifier)
                    PlayerUpgrades.Instance.Upgrade_WeaponLaserSightEnable();
                else
                    PlayerUpgrades.Instance.Upgrade_WeaponLaserSightDisable();
                break;
            case StatType.WeaponProjectilesPerShot:
                PlayerUpgrades.Instance.Upgrade_ProjectilesPerShot((int)stat.Modifier);
                break;
            case StatType.WeaponProjectilesSpread:
                PlayerUpgrades.Instance.Upgrade_ProjectileSpreadSet(stat.VectorModifier);
                break;
            case StatType.WeaponProjectileRandomSpread:
                if (stat.BoolModifier)
                    PlayerUpgrades.Instance.Upgrade_ProjectileRandomSpreadEnable();
                else
                    PlayerUpgrades.Instance.Upgrade_ProjectileRandomSpreadDisable();
                break;
            case StatType.WeaponProjectileSpeed:
                PlayerUpgrades.Instance.Upgrade_ProjectileSpeed(stat.Modifier);
                break;
            case StatType.WeaponProjectileAcceleration:
                PlayerUpgrades.Instance.Upgrade_ProjectileAcceleration(stat.Modifier);
                break;
            case StatType.WeaponProjectileLifetime:
                PlayerUpgrades.Instance.Upgrade_ProjectileLifetime(stat.Modifier);
                break;
            case StatType.WeaponProjectileDamage:
                PlayerUpgrades.Instance.Upgrade_ProjectileDamage((int)stat.Modifier);
                break;
            case StatType.WeaponProjectileKnockback:
                PlayerUpgrades.Instance.Upgrade_ProjectileKnockback(stat.VectorModifier);
                break;
            default:
                break;
        }
    }
}
