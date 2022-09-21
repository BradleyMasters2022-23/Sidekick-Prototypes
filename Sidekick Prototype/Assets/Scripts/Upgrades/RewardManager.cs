using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    private List<UpgradeObject> upgradeOptions = new List<UpgradeObject>();

    private List<UpgradeObject> chosenUpgrades = new List<UpgradeObject>();

    private List<UpgradeContainer> upgradeContainers = new List<UpgradeContainer>();

    public GameObject upgradeContainer;

    public Transform rewardSpawnPoint;

    public Vector2Int rewardCountRange = new Vector2Int(0, 2);

    public bool linkedUpgrades = true;

    private void Start()
    {
        // Populate the list
        if (FindObjectOfType<SpawnManager>().HasSpecialUpgrades())
            upgradeOptions = FindObjectOfType<SpawnManager>().GetSpecialUpgrades();
        else
            upgradeOptions = RewardStorage.instance.GetList();

        DetermineUpgrades();
    }

    public void DetermineUpgrades()
    {
        if(upgradeOptions.Count <= 0)
        {
            Debug.Log("Out of upgrades!");
            return;
        }


        int rewardChance = RewardStorage.instance.GetLuck();
        int i = Random.Range(0, 100);
        if(i < rewardChance)
        {
            RewardStorage.instance.DecreaseLuck();
            ChooseUpgrades();
        }
        else
        {
            Debug.Log("Unlucky!");
            RewardStorage.instance.IncreaseLuck();
        }
    }

    public void ChooseUpgrades()
    {
        int rewardCount = Random.Range(rewardCountRange.x, rewardCountRange.y);
        Debug.Log(rewardCount);
        // Choose enough upgrades to reach reward count
        UpgradeObject temp;
        while (chosenUpgrades.Count < rewardCount)
        {
            temp = upgradeOptions[Random.Range(0, upgradeOptions.Count)];
            // Dont choose more than one of each upgrade
            if (!chosenUpgrades.Contains(temp))
            {
                chosenUpgrades.Add(temp);
            }
        }
    }

    public void DisplayUpgrades()
    {
        // Spawn all upgrades cascading to the right
        for (int i = 0; i < chosenUpgrades.Count; i++)
        {
            Vector3 temp = rewardSpawnPoint.transform.position;
            temp += Vector3.right * (i * 5);
            GameObject obj = Instantiate(upgradeContainer, temp, rewardSpawnPoint.rotation);
            obj.GetComponent<UpgradeContainer>().SetUp(chosenUpgrades[i]);
            upgradeContainers.Add(obj.GetComponent<UpgradeContainer>());
        }

        // Link all spawned upgrades if linked
        if(linkedUpgrades && upgradeContainers.Count > 1)
        {
            foreach(UpgradeContainer linkingUpgrade in upgradeContainers)
            {
                foreach(UpgradeContainer refUpgrade in upgradeContainers)
                {
                    if(linkingUpgrade != refUpgrade)
                    {
                        linkingUpgrade.AddLink(refUpgrade.gameObject);
                    }
                }
            }
        }

        // Enable the spawned upgrades all at once after setup
        foreach (UpgradeContainer refUpgrade in upgradeContainers)
        {
            refUpgrade.GetComponent<Collider>().enabled = true;
        }
    }
}
