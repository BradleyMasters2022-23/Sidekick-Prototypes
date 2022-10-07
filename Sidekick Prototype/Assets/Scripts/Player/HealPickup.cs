using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : MonoBehaviour
{
    public int healAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealingSystem temp;
            if(other.TryGetComponent<HealingSystem>(out temp))
            {
                bool r = temp.PickupHeal(healAmount);
                if(r)
                    Destroy(gameObject);
            }
        }
    }
}
