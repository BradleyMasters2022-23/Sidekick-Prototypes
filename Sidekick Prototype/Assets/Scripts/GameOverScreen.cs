using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public GameObject targetScreen;

    public void EnableDeathScreen()
    {
        targetScreen.SetActive(true);
    }
}
