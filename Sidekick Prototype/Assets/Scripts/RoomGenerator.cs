using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private string[] sceneNames;


    public string currRoom;
    public string lastRoom;
    public string returnRoom;

    public static RoomGenerator instance;

    public int count;
    public int floorLength;
    public string finalRoom;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            instance.count = 1;
        }
        else
        {
            Destroy(this.gameObject);
        }

        currRoom = SceneManager.GetActiveScene().name;
    }

    public void SelectRoom()
    {
        // If player just finished the final room, return to 'hub'
        if (currRoom == finalRoom)
        {
            ReturnToHub();
            return;
        }

        // If reaching max floor length, load the last room
        if (count+1 >= floorLength)
        {
            lastRoom = currRoom;
            currRoom = finalRoom;
            LoadRoom();

            return;
        }

        string next = sceneNames[Random.Range(0, sceneNames.Length)];

        while(next == currRoom)
        {
            next = sceneNames[Random.Range(0, sceneNames.Length)];
        }

        lastRoom = currRoom;
        currRoom = next;

        LoadRoom();
    }

    public void LoadRoom()
    {
        instance.count++;
        SceneManager.UnloadSceneAsync(lastRoom);
        SceneManager.LoadSceneAsync(currRoom);
    }

    public void ReturnToHub()
    {
        RoomGenerator.instance = null;
        PlayerUpgradeManager.instance.DestroyPUM();
        RewardStorage.instance.DestroyRS();
        
        SceneManager.UnloadSceneAsync(currRoom);
        SceneManager.LoadSceneAsync(returnRoom);
        Destroy(gameObject);
    }
}
