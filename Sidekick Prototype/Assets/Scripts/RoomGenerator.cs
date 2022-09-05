using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private string[] sceneNames;


    private string currRoom;
    private string lastRoom;

    public static RoomGenerator instance;

    public int count;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            instance.count = 0;
        }
        else
        {
            Destroy(this.gameObject);
        }

        currRoom = SceneManager.GetActiveScene().name;
    }


    public void SelectRoom()
    {
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


}
