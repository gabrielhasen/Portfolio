using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class HealthTile : Health 
{
    public Material lightMaterial;

    //check if correct projectile then do damage
    public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration) {
        if (instigator.tag != "DamageTile") return;
        base.Damage(damage, instigator, flickerDuration, invincibilityDuration);

    }

    public override void Kill()
    {
        TileDestroy();
        base.Kill();
    }

    private void TileDestroy()
    {
        //Handles material and lighting when tiles get destroyed
        gameObject.layer = LayerMask.NameToLayer("VisibleArea");
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = WorldController.Instance.LightArea;
        Material temp = WorldController.Instance.UnlitTileMaterial;
        renderer.material = temp;
        renderer.material.SetColor(0, Color.black);
        renderer.color = Color.black;

        //Handles destroying colliders
        Destroy(GetComponent<BoxCollider2D>());
    }
}
