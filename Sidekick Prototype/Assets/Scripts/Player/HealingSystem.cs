using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingSystem : MonoBehaviour
{
    private IDamagable player;

    private PlayerControls c;
    private InputAction heal;
    private InputAction cheatHeal;

    public int maxHeals;
    public bool autoUse;
    public int startingHeals;
    public float healDelay;
    [SerializeField] private int currHeals;
    private bool healing;
    private int healAmount = 2;

    private Coroutine healingRoutine;

    private void Awake()
    {
        c = new PlayerControls();

        heal = c.Player.HealPlayer;
        heal.performed += InputHeal;
        heal.Enable();

        cheatHeal = c.Player.HealCheat;
        cheatHeal.performed += CheatHealInput;
        cheatHeal.Enable();
    }

    private void Start()
    {
        player = GetComponent<IDamagable>();
        currHeals = startingHeals;
        healing = false;
    }

    private void OnDisable()
    {
        heal.Disable();
        cheatHeal.Disable();
        if (healingRoutine != null)
            StopCoroutine(healingRoutine);
    }

    public bool PickupHeal(int amount)
    {
        healAmount = amount;

        if (autoUse)
        {
            ApplyHeal();
            return true;
        }
            

        if(currHeals >= maxHeals)
        {
            return false;
        }
        else
        {
            currHeals++;
            return true;
        }
    }

    private void InputHeal(InputAction.CallbackContext callback)
    {
        if (healing)
            return;
        else if (currHeals <= 0)
            return;
        else if (player.GetHealth() == player.maxHealth)
            return;
        else
        {
            healing = true;
            healingRoutine = StartCoroutine(HealPlayer());
        }
    }

    private void CheatHealInput(InputAction.CallbackContext callback)
    {
        if (healing)
            return;
        else
        {
            ApplyHeal();
        }
    }

    private IEnumerator HealPlayer()
    {
        yield return new WaitForSeconds(healDelay);
        ApplyHeal();
        currHeals--;
        healingRoutine = null;
        healing = false;
    }

    private void ApplyHeal()
    {
        player.Heal(healAmount);
    }

    public int GetHealCount()
    {
        return currHeals;
    }

    public bool IsHealing()
    {
        return healing;
    }
}
