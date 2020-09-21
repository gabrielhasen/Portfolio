using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using MoreMountains.CorgiEngine;

public class RockHolder : MonoBehaviour
{
    public GameObject rock;
    public GameObject rockGO;
    public Rock rockMovement;
    public GameObject rockAttachmentPoint;
    public bool rockOnPlayer;
    private bool pauseShooting;
    private DamageOnTouch damage;

    private void Start()
    {
        pauseShooting = false;
        rockOnPlayer = true;
        rockGO = Instantiate(rock, gameObject.transform.position, Quaternion.identity);
        rockMovement = rockGO.GetComponent<Rock>();
        damage = rockGO.GetComponentInChildren<DamageOnTouch>();
    }

    public void DamageUp(int damageCaused)
    {
        damage.DamageCaused += damageCaused;
    }

    public void Knockback(Vector2 knockbackAmount)
    {
        damage.DamageCausedKnockbackForce += knockbackAmount;
    }

    public void MoveToSpeed(float moveTo)
    {
        rockMovement.MoveToTimeLerp += moveTo;
    }
    public void ReturnSpeed(float moveTo)
    {
        rockMovement.MoveToTime += moveTo;
    }

    private void Update()
    {
        if (pauseShooting)
            return;

        //When left mouse button clicked, begin logic
        if (Input.GetMouseButtonDown(1)) {
            ShootRock();
        }
        //After shooting wait one second and if right click pull rock back to you
        if (Input.GetMouseButtonDown(1)) {
            //TeleportToRock();
        }
    }

    public void PauseShooting()
    {
        pauseShooting = true;
    }

    public void UnpauseShooting()
    {
        pauseShooting = false;
    }

    private void ShootRock()
    {
        if (rockOnPlayer && rockMovement.CanShootAgain) {
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rockMovement.SetNewLocation( target );
            rockOnPlayer = false;
        }
        else if (!rockOnPlayer && rockMovement.CanShootAgain) {
            rockMovement.ReturnToPlayer();
            rockOnPlayer = true;
        }
    }

    private void TeleportToRock()
    {
        if(!rockOnPlayer && rockMovement.CanShootAgain) {
            rockMovement.TeleportPlayerToRock();
            rockOnPlayer = true;
        }
    }

    public void ReturnRock()
    {
        rockMovement.RockReturned();
        rockOnPlayer = true;
    }
}
