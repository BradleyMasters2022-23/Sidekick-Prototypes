using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeContainer : MonoBehaviour
{
    public UpgradeObject upgrade;
    public Color color;

    private void Start()
    {
        if (upgrade is null)
            Destroy(this.gameObject);

        GetComponent<Renderer>().material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerUpgradeManager.instance.AddUpgrade(upgrade);
            Destroy(this.gameObject);
        }
    }
}
