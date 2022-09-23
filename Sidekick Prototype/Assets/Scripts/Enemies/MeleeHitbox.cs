using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private bool hitPlayer;
    private int damage;

    public void AssignDamage(int dmg)
    {
        damage = dmg;
    }

    public void Activate()
    {
        hitPlayer = false;
        GetComponent<Collider>().enabled = true;
    }

    public void Deactivate()
    {
        hitPlayer = true;
        GetComponent <Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !hitPlayer)
        {
            Deactivate();
            other.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
}
