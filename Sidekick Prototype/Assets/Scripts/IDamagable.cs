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
    
    public bool sectionedHealth;
    public int healthPerSection;
    public int numOfSections;
    [SerializeField] protected int sectionIndex;
    public List<Slider> sections;

    private AudioSource s;
    public AudioClip sound;

    public GameObject deathVFX;

    protected virtual void Awake()
    {
        if(!sectionedHealth)
        {
            health = maxHealth;
            killed = false;
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = maxHealth;
            }
        }
        else
        {
            maxHealth = health = healthPerSection * numOfSections;
            if (sections[0] != null)
            {
                sections[0].maxValue = healthPerSection;
                sections[0].value = healthPerSection;
                float spacing = sections[0].gameObject.GetComponent<RectTransform>().sizeDelta.x;
                for (int i = 1; i < numOfSections; i++)
                {
                    GameObject t = Instantiate(sections[i-1].gameObject, sections[i-1].transform);
                    t.name = "HealthSection" + i;
                    t.GetComponent<RectTransform>().localPosition = Vector3.zero + Vector3.right * spacing + Vector3.right;
                    sections.Add(t.GetComponent<Slider>());
                }
                sectionIndex = sections.Count-1;
            }
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

        if(sectionedHealth)
        {
            if (sections[sectionIndex].value - dmg == 0)
            {
                health -= dmg;
                sections[sectionIndex].value = 0;

                if (sectionIndex == 0)
                {
                    killed = true;
                    Die();
                }
                else
                {
                    sectionIndex--;
                }
            }
            else if(sections[sectionIndex].value - dmg < 0)
            {
                float overflow = -1 * (sections[sectionIndex].value - dmg);
                health -= dmg;
                sections[sectionIndex].value = 0;

                if (sectionIndex == 0)
                {
                    killed = true;
                    Die();
                }
                else
                {
                    sectionIndex--;
                    sections[sectionIndex].value -= overflow;
                }
            }
            else
            {
                health -= dmg;
                sections[sectionIndex].value -= dmg;
            }

            return;
        }



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

    private void HealSection(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            sections[sectionIndex].value = healthPerSection;
            health = (sectionIndex+1) * healthPerSection;
            if (sectionIndex+1 == sections.Count)
                return;
            else
                sectionIndex++;
        }
    }
    private void HealPool(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateSlider();
    }

    public void Heal(int amount)
    {
        if (sectionedHealth)
            HealSection(amount);
        else
            HealPool(amount);
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
        if (sound != null)
            AudioSource.PlayClipAtPoint(sound, transform.position);

        if (deathVFX != null)
            Instantiate(deathVFX, transform.position + Vector3.up * (transform.localScale.y/2), transform.rotation);

        Destroy(this.gameObject);
    }

    public void UpdateSlider()
    {
        healthSlider.value = health;
    }
    public int GetHealth()
    {
        return health;
    }
}
