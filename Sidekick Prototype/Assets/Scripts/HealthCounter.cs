using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public IDamagable healthbar;

    public void UpdateCounter()
    {
        int i = (int)healthbar.GetHealth();

        if(i == 100)
        {
            text.text = "100";
        }
        else if(i >= 10)
        {
            text.text = "0" + i.ToString();
        }
        else if (i >= 0)
        {
            text.text = "00" + i.ToString();
        }
        else
            text.text = "000";

    }
}
