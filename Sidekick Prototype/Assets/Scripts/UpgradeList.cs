using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeList : MonoBehaviour
{
    List<UpgradeObject> list;
    public TextMeshProUGUI upgradeText;
    private int c = 0;
    private int lastC = 0;


    private void Update()
    {
        list = FindObjectOfType<PlayerUpgradeManager>().GetUpgrades();

        if (list == null )
            return;

        lastC = c;
        c = list.Count;

        if(lastC != c)
        {
            Debug.Log("Calling upgrade list to update");
            UpdateList();
        }
    }

    public void UpdateList()
    {
        string temp = "";
        foreach(UpgradeObject up in list)
        {
            temp += up.displayName + "\n";
        }
        upgradeText.text = temp;
    }
}
