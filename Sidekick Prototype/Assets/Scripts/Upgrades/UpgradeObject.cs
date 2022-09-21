using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Upgrade Data")]
public class UpgradeObject : ScriptableObject
{
    public enum UpgradeType
    {
        Player,
        Gun,
        Time
    }

    public string displayName;



    public UpgradeType upgradeType;

    public GameObject upgradePrefab;

    public Color upgradeColor;
}
