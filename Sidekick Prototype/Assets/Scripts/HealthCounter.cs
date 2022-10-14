using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI healthPickups;
    public IDamagable healthbar;
    public HealingSystem storedHeals;
    private Color originalColor;

    private void Awake()
    {
        originalColor = healthPickups.color;
    }

    private void Start()
    {
        if (storedHeals.autoUse)
            healthPickups.gameObject.SetActive(false);
        else
            healthPickups.text = "Heals: " + 
                storedHeals.startingHeals.ToString() + " / " + storedHeals.maxHeals.ToString();

        
    }

    public void UpdateCounter()
    {

        int i = 100;
        if(healthbar != null)
            i = (int)healthbar.GetHealth();

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

        if(healthPickups.gameObject.activeInHierarchy)
        {
            healthPickups.text = "Heals: " +
                    storedHeals.GetHealCount().ToString() + " / " + storedHeals.maxHeals.ToString();

            if(storedHeals.IsHealing())
            {
                healthPickups.color = Color.green;
            }
            else
            {
                healthPickups.color = originalColor;
            }
        }
            
    }

    private void Update()
    {
        UpdateCounter();
    }
}
