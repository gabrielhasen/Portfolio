using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponRockHandler : CharacterHandleWeapon
{
    private RockHolder rockHolder;

    protected override void Start()
    {
        base.Start();
        rockHolder = gameObject.GetComponent<RockHolder>();
    }

    protected override void HandleInput()
    {
        if(rockHolder.rockOnPlayer)
            base.HandleInput();
    }
}
