using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    PlayerControls controller;
    InputAction pause;
    public GameObject pauseScreen;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = new PlayerControls();
        pause = controller.Player.Pause;
        pause.Enable();
        pause.performed += TogglePause;
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        paused = !paused;

        if(paused)
        {
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;

        }
        else
        {
            pauseScreen.SetActive(false);
            Cursor.lockState= CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }


}
