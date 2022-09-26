using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Wave Data", fileName = "New Wave Data")]
public class WaveSO : ScriptableObject
{
    public enum WaveType
    {
        Normal,
        Miniboss,
        Boss
    }
    
    public Wave[] allWaves;

    public WaveType type;

    public int contThreshold;

    public List<UpgradeObject> specialRewards;
}

[System.Serializable]
public class Wave
{
    public GameObject[] wave;
}