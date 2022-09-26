using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardStorage : MonoBehaviour
{
    public static RewardStorage instance;

    [SerializeField] private List<UpgradeObject> upgradeList;

    public int currentLuck = 60;
    public Vector2Int luckRange;
    public int rewardLuckDecrease;
    public int rewardLuckIncrease;

    private void Awake()
    {
        if (instance == null)
        {
            transform.parent = null;
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// Remove an upgrade from the pool
    /// </summary>
    /// <param name="uo">upgrade to remove from pool</param>
    public void RemoveUpgrade(UpgradeObject uo)
    {
        if(upgradeList.Contains(uo))
            upgradeList.Remove(uo);
    }

    public List<UpgradeObject> GetList()
    {
        return upgradeList;
    }

    public int GetLuck()
    {
        return currentLuck;
    }

    public void IncreaseLuck()
    {
        currentLuck += rewardLuckIncrease;
        currentLuck = (int)Mathf.Clamp(currentLuck, luckRange.x, luckRange.y);
    }
    public void DecreaseLuck()
    {
        currentLuck -= rewardLuckDecrease;
        currentLuck = (int)Mathf.Clamp(currentLuck, luckRange.x, luckRange.y);
    }

    public void DestroyRS()
    {
        RewardStorage.instance = null;
        Destroy(this.gameObject);
    }
}
