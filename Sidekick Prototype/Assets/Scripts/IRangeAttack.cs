using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRangeAttack : MonoBehaviour
{
    public enum AttackType
    {
        Projectile,
        Hitscan
    }
    [SerializeField] protected AttackType attackType;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTime;

    public AttackType GetAttackType()
    {
        return attackType;
    }
}
