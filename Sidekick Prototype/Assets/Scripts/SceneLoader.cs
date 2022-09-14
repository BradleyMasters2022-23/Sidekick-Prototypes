using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;


    public void LoadScene()
    {
        Time.timeScale = 1f;

        // Reset instances
        RoomGenerator rm = RoomGenerator.instance;
        if (rm != null)
            rm.DestroyRoomGen();

        PlayerUpgradeManager pum = PlayerUpgradeManager.instance;
        if (pum != null)
            pum.DestroyPUM();


        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
