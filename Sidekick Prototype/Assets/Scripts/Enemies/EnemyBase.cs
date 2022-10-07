using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is enemy lmao
/// </summary>
public abstract class EnemyBase : IDamagable
{
    // Interface Stuff
    public enum EnemyState
    {
        Idle,
        Preparing,
        Moving,
        Attacking
    }

    [Header("---General Enemy Stats---")]

    /// <summary>
    /// Player object
    /// </summary>
    protected GameObject player;

    //[Tooltip("Current state of the enemy")]
    //[SerializeField] protected EnemyState currentState;

    /// <summary>
    /// Current world time, based on time manager
    /// </summary>
    //protected float currTime;

    //[Tooltip("Maximum speed for this enemy")]
    //[SerializeField]
    //protected float maxMoveSpeed;

    //[Tooltip("Speed the enemy reaches max speed")]
    //[SerializeField]
    //protected float accelerationSpeed;

    //[Tooltip("Speed the enemy can rotate")]
    //[SerializeField]
    //protected float rotationSpeed;

    /// <summary>
    /// Prepare setup
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        //currTime = TimeManager.worldTime;
        player = FindObjectOfType<PlayerControllerRB>().gameObject;
    }

    
}
