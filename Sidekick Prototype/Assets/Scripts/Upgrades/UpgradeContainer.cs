using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeContainer : MonoBehaviour
{
    public UpgradeObject upgrade;
    public Color color;
    public TextMeshProUGUI nameText;
    private List<GameObject> linkedUpgrades = new List<GameObject>();

    private AudioSource s;
    public AudioClip sound;

    private void Start()
    {
        if (upgrade is null)
            Destroy(this.gameObject);

        if(upgrade != null)
            SetUp(upgrade);

        s = gameObject.AddComponent<AudioSource>();
    }

    public void SetUp(UpgradeObject obj)
    {
        upgrade = obj;
        color = upgrade.upgradeColor;
        GetComponent<Renderer>().material.color = color;
        nameText.text = upgrade.displayName;
    }


    public void AddLink(GameObject obj)
    {
        linkedUpgrades.Add(obj);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UpgradeSelected();
            if(sound!= null)
                AudioSource.PlayClipAtPoint(sound, transform.position);
            PlayerUpgradeManager.instance.AddUpgrade(upgrade);
            Destroy(this.gameObject);
        }
    }

    private void UpgradeSelected()
    {
        for (int i = linkedUpgrades.Count - 1; i >= 0; i--)
        {
            Destroy(linkedUpgrades[i]);
        }
        linkedUpgrades.Clear();

        // If only allowed once, then remove from reward pool
        if(!upgrade.reearnable)
            RewardStorage.instance.RemoveUpgrade(upgrade);
    }
}
