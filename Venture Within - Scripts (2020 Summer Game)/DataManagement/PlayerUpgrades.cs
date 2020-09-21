using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using DataSystem;


using Debug = UnityEngine.Debug;

public class PlayerUpgrades :   MMPersistentSingleton<PlayerUpgrades>,
                                MMEventListener<CorgiEngineEvent>
{
    public UpgradeList upgradeList;

    public List<Upgrade> UpgradeList { get { return _upgrades; } }

    public List<Upgrade> _upgrades;

    private GameObject player;
    private CharacterDash dashUpgrade;
    private CharacterJump jumpUpgrade;
    private CharacterHorizontalMovement runUpgrade;
    private CharacterWalljump wallJumpUpgrade;
    private CharacterGlide glideUpgrade;
    private CharacterSwim swimUpgrade;
    private RockHolder rockUpgrade;
    private Health healthUpgrade;
    public ProjectileWeapon rockWeapon;
    public MMSimpleObjectPooler projectilePool;
    private WeaponLaserSight laserSight;


    /// <summary>
    /// Data Management
    /// </summary>
    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";
    protected const string _saveFileNameUpgrades = "PermUpgrades";
    protected const string _saveFileExtensionUpgrades = ".upgrade";

    public int GetCurrentHP()
    {
        return healthUpgrade.CurrentHealth;
    }

    public int GetMaxHP()
    {
        return healthUpgrade.MaximumHealth;
    }

    /// <summary>
    /// Increases the players current health by some value
    /// </summary>
    /// <param name="healthValue">Amount the players health increases by</param>
    /// <returns>bool to check if current health is already max</returns>
    public bool CurrentHealthUp(int healthValue)
    {
        if(healthUpgrade.CurrentHealth == healthUpgrade.MaximumHealth) {
            return false;
        }
        else {
            healthUpgrade.GetHealth(healthValue, gameObject);
            return true;
        }
    }

    /// <summary>
    /// Decreases the players current health by some value
    /// </summary>
    /// <param name="healthValue">Amount the players health decreases by</param>
    public void CurrentHealthDown(int healthValue)
    {
        healthUpgrade.GetHealth(-healthValue, gameObject);
    }

    /// <summary>
    /// Increases the players maximum health by some value
    /// Will also increase your current health by the maximum health value increase
    /// </summary>
    /// <param name="healthValue">Amount the players max health increases by</param>
    public void Upgrade_MaxHealthUp(int healthValue)
    {
        Debug.Log("Before: " + healthUpgrade.MaximumHealth);
        healthUpgrade.MaximumHealth += healthValue;
        healthUpgrade.GetHealth(healthValue, gameObject);
        Debug.Log("After: " + healthUpgrade.MaximumHealth);
    }

    /// <summary>
    /// Decreases the players maximum health by some value
    /// Sets current health to maximum health if current health becomes greater than maximum.
    /// </summary>
    /// <param name="healthValue">Amount the players max health decreases by</param>
    public void Upgrade_MaxHealthDown(int healthValue)
    {
        healthUpgrade.MaximumHealth -= healthValue;
        if(healthUpgrade.CurrentHealth > healthUpgrade.MaximumHealth) {
            healthUpgrade.CurrentHealth = healthUpgrade.MaximumHealth;
        }
    }

    public void Upgrade_JumpsUp(int jumpValue)
    {
        Debug.Log("Before: " + jumpUpgrade.NumberOfJumps);
        jumpUpgrade.NumberOfJumps += jumpValue;
        if (jumpUpgrade.NumberOfJumps < 1) {
            jumpUpgrade.NumberOfJumps = 1;
        }
        Debug.Log("After: " + jumpUpgrade.NumberOfJumps);
    }

    public void Upgrade_JumpHeight(float jumpValue)
    {
        Debug.Log("Before: " + jumpUpgrade.JumpHeight);
        jumpUpgrade.JumpHeight += jumpValue;
        if (jumpUpgrade.JumpHeight < 1) {
            jumpUpgrade.JumpHeight = 1;
        }
        Debug.Log("After: " + jumpUpgrade.JumpHeight);
    }

    public void Upgrade_RunSpeed(float runValue)
    {
        Debug.Log("Before: " + runUpgrade.WalkSpeed);
        runUpgrade.WalkSpeed += runValue;
        if (runUpgrade.WalkSpeed < 1) {
            runUpgrade.WalkSpeed = 1;
        }
        runUpgrade.ResetHorizontalSpeed();
        Debug.Log("After: " + runUpgrade.WalkSpeed);
    }

    public void Upgrade_WallJumpDistance(Vector2 wallJumpForce)
    {
        Debug.Log("Before: " + wallJumpUpgrade.WallJumpForce);
        wallJumpUpgrade.WallJumpForce += wallJumpForce;
        Debug.Log("After: " + wallJumpUpgrade.WallJumpForce);
    }

    public void Upgrade_EnableGlide()
    {
        Debug.Log("Before: " + glideUpgrade.enabled);
        glideUpgrade.enabled = true;
        Debug.Log("After: " + glideUpgrade.enabled);
    }

    public void Upgrade_DisableGlide()
    {
        Debug.Log("Before: " + glideUpgrade.enabled);
        glideUpgrade.enabled = false;
        Debug.Log("After: " + glideUpgrade.enabled);
    }

    public void Upgrade_DashDistance(float dashDistance)
    {
        Debug.Log("Before: " + dashUpgrade.DashDistance);
        dashUpgrade.DashDistance += dashDistance;
        if(dashUpgrade.DashDistance < 0) {
            dashUpgrade.DashDistance = 0;
        }
        Debug.Log("After: " + dashUpgrade.DashDistance);
    }

    public void Upgrade_DashCooldown(float dashCooldown)
    {
        Debug.Log("Before: " + dashUpgrade.DashCooldown);
        dashUpgrade.DashCooldown += dashCooldown;
        if (dashUpgrade.DashCooldown < 0) {
            dashUpgrade.DashCooldown = 0;
        }
        Debug.Log("After: " + dashUpgrade.DashCooldown);
    }

    public void Upgrade_DashForce(float dashForce)
    {
        Debug.Log("Before: " + dashUpgrade.DashForce);
        dashUpgrade.DashForce += dashForce;
        Debug.Log("After: " + dashUpgrade.DashForce);
    }

    public void Upgrade_RockDamage(int damage)
    {
        rockUpgrade.DamageUp(damage);
    }

    public void Upgrade_RockKnockback(Vector2 knockback)
    {
        rockUpgrade.Knockback(knockback);
    }

    public void Upgrade_RockMoveToSpeedk(float knockback)
    {
        rockUpgrade.MoveToSpeed(knockback);
    }

    public void Upgrade_RockReturnSpeed(float knockback)
    {
        rockUpgrade.ReturnSpeed(knockback);
    }

    //Weapon Upgrades
    public void Upgrade_WeaponShootSpeed(float timeBetween)
    {
        Debug.Log("Before: " + rockWeapon.TimeBetweenUses);
        rockWeapon.TimeBetweenUses += timeBetween;
        if (rockWeapon.TimeBetweenUses < 0) {
            rockWeapon.TimeBetweenUses = 0;
        }
        Debug.Log("After: " + rockWeapon.TimeBetweenUses);
    }

    public void Upgrade_WeaponAutomatic()
    {
        Debug.Log("Before: " + rockWeapon.TriggerMode);
        Debug.Log(rockWeapon);
        rockWeapon.TriggerMode = Weapon.TriggerModes.Auto;
        Debug.Log("After: " + rockWeapon.TriggerMode);
    }

    public void Upgrade_WeaponSemiAutomatic()
    {
        Debug.Log("Before: " + rockWeapon.TriggerMode);
        rockWeapon.TriggerMode = Weapon.TriggerModes.SemiAuto;
        Debug.Log("After: " + rockWeapon.TriggerMode);
    }

    public void Upgrade_WeaponMagazineSize(int magSizeIncrease)
    {
        Debug.Log("Before: " + rockWeapon.MagazineSize);
        rockWeapon.MagazineSize += magSizeIncrease;
        if(rockWeapon.MagazineSize < 0) {
            rockWeapon.MagazineSize = 1;
        }
        rockWeapon.CurrentAmmoLoaded = 0;
        rockWeapon.InitiateReloadWeapon();
        Debug.Log("After: " + rockWeapon.MagazineSize);
    }

    public void Upgrade_WeaponReloadTime(float reloadTimeIncrease)
    {
        Debug.Log("Before: " + rockWeapon.ReloadTime);
        rockWeapon.ReloadTime += reloadTimeIncrease;
        if(rockWeapon.ReloadTime < 0) {
            rockWeapon.ReloadTime = 0;
        }
        Debug.Log("After: " + rockWeapon.ReloadTime);
    }

    public void Upgrade_WeaponLaserSightEnable()
    {
        Debug.Log("Before: " + laserSight.enabled);
        laserSight.enabled = true;
        Debug.Log("After: " + laserSight.enabled);
    }

    public void Upgrade_WeaponLaserSightDisable()
    {
        Debug.Log("Before: " + laserSight.enabled);
        laserSight.enabled = false;
        Debug.Log("After: " + laserSight.enabled);
    }

    public void Upgrade_ProjectilesPerShot(int projectileIncrease)
    {
        Debug.Log("Before: " + rockWeapon.ProjectilesPerShot);
        rockWeapon.ProjectilesPerShot += projectileIncrease;
        if (rockWeapon.ProjectilesPerShot < 1) {
            rockWeapon.ProjectilesPerShot = 1;
        }
        Debug.Log("After: " + rockWeapon.ProjectilesPerShot);
    }

    public void Upgrade_ProjectileSpreadSet(Vector3 spread)
    {
        Debug.Log("Before: " + rockWeapon.Spread);
        rockWeapon.Spread = spread;
        Debug.Log("Before: " + rockWeapon.Spread);
    }

    public void Upgrade_ProjectileRandomSpreadEnable()
    {
        Debug.Log("Before: " + rockWeapon.RandomSpread);
        rockWeapon.RandomSpread = true;
        Debug.Log("After: " + rockWeapon.RandomSpread);
    }
    public void Upgrade_ProjectileRandomSpreadDisable()
    {
        Debug.Log("Before: " + rockWeapon.RandomSpread);
        rockWeapon.RandomSpread = false;
        Debug.Log("After: " + rockWeapon.RandomSpread);
    }

    //Will probably need an upgrade to specify a certain spread amount

    public void Upgrade_ProjectileSpeed(float speedIncrease)
    {
        Debug.Log("Before: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed);
        projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed += speedIncrease;
        if (projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed < 1) {
            projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed = 1;
        }
        RebuildPool();
        Debug.Log("After: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed);
    }
    private void SetSpeedToDefault()
    {
        projectilePool.GameObjectToPool.GetComponent<Projectile>().Speed = 200;
    }

    public void Upgrade_ProjectileAcceleration(float accelerationIncrease)
    {
        Debug.Log("Before: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration);
        projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration += accelerationIncrease;
        if (projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration < 0) {
            projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration = 0;
        }
        RebuildPool();
        Debug.Log("After: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration);
    }
    private void SetAccelerationToDefault()
    {
        projectilePool.GameObjectToPool.GetComponent<Projectile>().Acceleration = 0;
    }

    public void Upgrade_ProjectileLifetime(float lifetimeIncrease)
    {
        Debug.Log("Before: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime);
        projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime += lifetimeIncrease;
        if (projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime < 1) {
            projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime = 1;
        }
        RebuildPool();
        Debug.Log("After: " + projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime);
    }
    private void SetLifetimeToDefault()
    {
        projectilePool.GameObjectToPool.GetComponent<Projectile>().LifeTime = 5;
    }

    public void Upgrade_ProjectileDamage(int damageIncrease)
    {
        Debug.Log("Before: " + projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused);
        projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused += damageIncrease;
        if (projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused < 1) {
            projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused = 1;
        }
        RebuildPool();
        Debug.Log("After: " + projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused);
    }
    private void SetDamageToDefault()
    {
        projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCaused = 1;
        RebuildPool();
    }

    public void Upgrade_ProjectileKnockback(Vector2 knockbackIncrease)
    {
        Debug.Log("Before: " + projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCausedKnockbackForce);
        projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCausedKnockbackForce += knockbackIncrease;
        RebuildPool();
        Debug.Log("After: " + projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCausedKnockbackForce);
    }
    private void SetKnockbackToDefault()
    {
        projectilePool.GameObjectToPool.GetComponent<DamageOnTouch>().DamageCausedKnockbackForce = new Vector2(10, 2);
    }

    private void RebuildPool()
    {
        projectilePool.RefillPool();
        //projectilePool.FillObjectPool();
    }

    private void SetWeaponProjectileToDefaults()
    {
        SetDamageToDefault();
        SetSpeedToDefault();
        SetKnockbackToDefault();
        SetLifetimeToDefault();
        SetAccelerationToDefault();
        Upgrade_WeaponLaserSightDisable();
        Upgrade_ProjectileRandomSpreadDisable();
        RebuildPool();
    }


    public bool ApplyUpgrade(UpgradeType tempType)
    {
        Upgrade toChange = FindUpgrade(tempType);
        
        if(toChange == null) {
            Debug.LogError("Could not find upgrade");
            return false;
        }

        toChange.ProgressCurrent++;
        return true;
    }

    public Upgrade FindUpgrade(UpgradeType tempType)
    {
        for (int i = 0; i < _upgrades.Count; i++) {
            if(_upgrades[i].Type == tempType) {
                return _upgrades[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Called when a new level gets started to load all the scripts for upgrades from the current
    /// players GameObject.
    /// </summary>
    private void GetUpgradeInfo()
    {
        dashUpgrade = player.GetComponent<CharacterDash>();
        jumpUpgrade = player.GetComponent<CharacterJump>();
        runUpgrade = player.GetComponent<CharacterHorizontalMovement>();
        wallJumpUpgrade = player.GetComponent<CharacterWalljump>();
        glideUpgrade = player.GetComponent<CharacterGlide>();
        swimUpgrade = player.GetComponent<CharacterSwim>();
        healthUpgrade = player.GetComponent<Health>();
        rockUpgrade = player.GetComponent<RockHolder>();
        rockWeapon = player.GetComponent<CharacterWeaponRockHandler>().CurrentWeapon.GetComponent<ProjectileWeapon>();
        projectilePool = player.GetComponent<CharacterWeaponRockHandler>().CurrentWeapon.GetComponent<MMSimpleObjectPooler>();
        laserSight = player.GetComponent<CharacterWeaponRockHandler>().CurrentWeapon.GetComponent<WeaponLaserSight>();
        DebugUpgrades();

        laserSight.enabled = false;
    }

    /// <summary>
    /// Checks if all scripts are attached to the current players GameObject
    /// </summary>
    private void DebugUpgrades()
    {
        //MMDebug.DebugLogTime("Upgrade Started");
        if (dashUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterDash' not attached to player");
        }
        if (jumpUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterJump' not attached to player");
        }
        if (runUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterRun' not attached to player");
        }
        if (wallJumpUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterWalljump' not attached to player");
        }
        if (glideUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterGlide' not attached to player");
        }
        if (swimUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'CharacterSwim' not attached to player");
        }
        if (healthUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'Health' not attached to player");
        }
        if (rockUpgrade == null) {
            Debug.LogError("[PlayerUpgrades]: 'RockHolder' not attached to player");
        }
        if(rockWeapon == null) {
            Debug.LogError("[PlayerUpgrades]: 'ProjectileWeapon' not attached to CharacterWeaponRockHandler");
        }
        if (projectilePool == null) {
            Debug.LogError("[PlayerUpgrades]: 'MMSimpleObjectPooler' not attached to CharacterWeaponRockHandler");
        }
        if (laserSight == null) {
            Debug.LogError("[PlayerUpgrades]: 'WeaponLaserSight' not attached to CharacterWeaponRockHandler");
        }
    }

    private void LoadInPermUpgrades()
    {
        _upgrades = new List<Upgrade>();

        // the Upgrade List scriptable object must be in a Resources folder inside your project, like so : Resources/Upgrades/PermUpgrades/PUT_SCRIPTABLE_OBJECT_HERE
        upgradeList = (UpgradeList)Resources.Load("Upgrades/PermUpgrades");
        if(upgradeList == null) {
            Debug.LogError("Could not find UpgradeList in 'Resources/Upgrades/PermUpgrades'");
            return;
        }

        foreach (Upgrade upgrade in upgradeList.Upgrades) {
            _upgrades.Add(upgrade.Copy());
        }
    }

    /// <summary>
    /// Saves the achievements current status to a file on disk
    /// </summary>
    public void SaveUpgrades()
    {
        SerializedUpgradeManager serializedUpgrade = new SerializedUpgradeManager();
        FillSerializedUpgrades(serializedUpgrade);
        MMSaveLoadManager.Save(serializedUpgrade, _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
    }

    private void FillSerializedUpgrades(SerializedUpgradeManager serializedUpgrade)
    {
        serializedUpgrade.upgrades = new SerializedUpgrades[_upgrades.Count];
        for (int i = 0; i < _upgrades.Count; i++) {
            SerializedUpgrades newUpgrade = new SerializedUpgrades(_upgrades[i].Type, _upgrades[i].UnlockedStatus, _upgrades[i].ProgressCurrent, _upgrades[i].ProgressMax);
            serializedUpgrade.upgrades[i] = newUpgrade;
        }
    }

    public void LoadUpgrades()
    {
        SerializedUpgradeManager serializedUpgrade = (SerializedUpgradeManager)MMSaveLoadManager.Load(typeof(SerializedUpgradeManager), _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
        ExtractSerializedUpgrades(serializedUpgrade);
    }

    private void ExtractSerializedUpgrades(SerializedUpgradeManager serializedUpgrade)
    {
        if (serializedUpgrade == null) {
            return;
        }

        for (int i = 0; i < _upgrades.Count; i++) {
            _upgrades[i].Type = serializedUpgrade.upgrades[i].Type;
            _upgrades[i].UnlockedStatus = serializedUpgrade.upgrades[i].UnlockStatus;
            _upgrades[i].ProgressMax = serializedUpgrade.upgrades[i].MaxUpgradeLevel;
            _upgrades[i].ProgressCurrent = serializedUpgrade.upgrades[i].CurrentUpgradeLevel;
        }
    }

    //HERE IS WHERE YOU CHANGE THE AMOUNT EACH UPGRADE CHANGES TO UPGRADE VARIABLES
    private void ApplyUpgrades()
    {
        for (int i = 0; i < _upgrades.Count; i++) {
            UpgradeType tempType = _upgrades[i].Type;
            switch (tempType) {
                case UpgradeType.MaxHealth:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 1 per upgrade
                        Upgrade_MaxHealthUp((int)_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.JumpHeight:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 0.5f per upgrade
                        Upgrade_JumpHeight(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.RunSpeed:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 0.5f per upgrade
                        Upgrade_RunSpeed(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.WallJumpDistance:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to (0.5f, 1f) per upgrade
                        Upgrade_WallJumpDistance(new Vector2(_upgrades[i].StatIncreaseAmount, _upgrades[i].StatIncreaseAmountY_IfVector));
                    }
                    break;
                case UpgradeType.DashDistance:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 0.5f per upgrade
                        Upgrade_DashDistance(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.DashCooldown:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 0.2f per upgrade
                        Upgrade_DashCooldown(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.DashForce:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 0.2f per upgrade
                        Upgrade_DashForce(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.RockDamage:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 1 per upgrade
                        Upgrade_RockDamage((int)_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.RockKnockback:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to (1, 0.5f) per upgrade
                        Upgrade_RockKnockback(new Vector2(_upgrades[i].StatIncreaseAmount, _upgrades[i].StatIncreaseAmountY_IfVector));
                    }
                    break;
                case UpgradeType.RockMoveToSpeed:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 1 per upgrade
                        Upgrade_RockMoveToSpeedk(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                case UpgradeType.RockReturnSpeed:
                    for (int j = 0; j < _upgrades[i].ProgressCurrent; j++) {
                        //Default set to 1 per upgrade
                        Upgrade_RockReturnSpeed(_upgrades[i].StatIncreaseAmount);
                    }
                    break;
                default:
                    break;
            }   
        }
    }


    //Event Listeners
    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }
    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        switch (engineEvent.EventType) {
            case CorgiEngineEventTypes.LevelStart:
                StartCoroutine(Wait());
                break;
            case CorgiEngineEventTypes.LevelEnd:
                EndLevel();
                break;
            case CorgiEngineEventTypes.PlayerDeath:
                EndLevel();
                break;
            case CorgiEngineEventTypes.GameOver:
                EndLevel();
                break;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();
        StartLevel();
    }

    private void StartLevel()
    {
        player = LevelManager.Instance.Players[0].gameObject;
        GetUpgradeInfo();
        if (upgradeList == null) {
            LoadInPermUpgrades();
        }
        //Save upgrades here if you want to reset all upgrades back to the inital setting
        // - 3 upgrades availiable, and all are set to 0 -
        //SaveUpgrades();
        
        LoadUpgrades();
        ApplyUpgrades();
        DisplayUpgrades();
        SetWeaponProjectileToDefaults();
    }

    private void EndLevel()
    {
        SaveUpgrades();
    }

    private void DisplayUpgrades()
    {
        for (int i = 0; i < _upgrades.Count; i++) {
            //Debug.Log(_upgrades[i].UnlockedStatus);
        }
    }
}
