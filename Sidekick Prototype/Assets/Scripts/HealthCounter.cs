using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public Slider healthbar;

    public void UpdateCounter()
    {
        int i = (int)healthbar.value;

        if(i == 100)
        {
            text.text = "100";
        }
        else if(i >= 10)
        {
            text.text = "0" + i.ToString();
        }
        else
        {
            text.text = "00" + i.ToString();
        }

    }
}
