using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Data")]
public class UpgradeObject : ScriptableObject
{
    public enum UpgradeType
    {
        Player, 
        Gun, 
        Time
    }

    public UpgradeType upgradeType;

    public GameObject upgradePrefab;
}
