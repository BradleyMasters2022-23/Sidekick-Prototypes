using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public enum HitboxState
    {
        Searching,
        Damaging
    }
    private HitboxState currentState;
    private bool playerInRange = false;
    private bool hitPlayer = false;
    private int damage;

    public void AssignDamage(int dmg)
    {
        damage = dmg;
    }

    public bool PlayerInRange()
    {
        return playerInRange;
    }

    public void Attack()
    {
        currentState = HitboxState.Damaging;
        hitPlayer = false;
    }

    public void AttackEnd()
    {
        currentState = HitboxState.Searching;
        hitPlayer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (TimeManager.worldTime == 0)
            return;

        if (!hitPlayer && currentState == HitboxState.Damaging && other.CompareTag("Player"))
        {
            hitPlayer = true;
            other.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
