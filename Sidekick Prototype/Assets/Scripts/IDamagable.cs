using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class IDamagable : MonoBehaviour
{
    public int maxHealth;
    protected int health;
    public bool invulnerable;

    public float comboTimer;

    public Slider healthSlider;

    public TextMeshProUGUI comboHit;
    public int comboCount;
    private bool inCombo;

    private float t;
    private bool killed;

    protected virtual void Awake()
    {
        health = maxHealth;
        killed = false;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        t = 0;
        inCombo = false;

        // If invulnerable, make target gold
        if (invulnerable)
            GetComponent<Renderer>().material.color = Color.yellow;
    }

    public virtual void TakeDamage(int dmg)
    {
        comboCount++;
        inCombo = true;
        t = 0;

        if (health - dmg <= 0 && !invulnerable)
        {
            health = 0;
            if(!invulnerable && !killed)
            {
                killed = true;
                Die();
            }
        }
        else
        {
            health -= dmg;
        }

        if(healthSlider != null)
        {
            healthSlider.value = health;
        }
    }

    protected virtual void FixedUpdate()
    {
        // Don't track combo system if not applicable
        if(comboHit != null)
        {
            if(inCombo && t >= comboTimer)
            {
                inCombo = false;
                t = 0;
                StopCombo();
            }
            else if(inCombo)
            {
                comboHit.color = Color.red;
                t += Time.deltaTime * TimeManager.worldTime;
                UpdateCombo();
            }
        }
    }

    public void StopCombo()
    {
        comboCount = 0;
        comboHit.color = Color.blue;

        if(invulnerable)
        {
            health = maxHealth;
            if (healthSlider != null)
            {
                healthSlider.value = maxHealth;
            }
        }
    }

    public void UpdateCombo()
    {
        comboHit.text = "Hits: " + comboCount;
    }

    public virtual void Die()
    {
        //Debug.Log(this.gameObject.name + " has died! Oh no!");
        Destroy(this.gameObject);
    }

    public void UpdateSlider()
    {
        healthSlider.value = health;
    }

}
