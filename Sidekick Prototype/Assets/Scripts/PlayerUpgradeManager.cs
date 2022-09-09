using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUpgradeManager : MonoBehaviour
{
    public PlayerControllerRB player;

    public PlayerGun gun;

    public TimeManager timeManager;

    public List<UpgradeObject> upgrades = new List<UpgradeObject>();

    public static PlayerUpgradeManager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        player = PlayerControllerRB.instance;
        gun = PlayerGun.instance;
        SceneManager.sceneLoaded += OnLevelLoad;
    }

    /// <summary>
    /// Spawns the upgrade component onto player, calls its initalization function
    /// </summary>
    /// <param name="up">upgrade to be initialized</param>
    private void InitializeUpgrade(UpgradeObject up)
    {
        IUpgrade temp = Instantiate(up.upgradePrefab, player.gameObject.transform).GetComponent<IUpgrade>();

        temp.LoadUpgrade(player);
    }

    /// <summary>
    /// Add an upgrade to the collection of acquired upgrades. Initialize it
    /// </summary>
    /// <param name="up">Upgrade to add to player</param>
    public void AddUpgrade(UpgradeObject up)
    {
        upgrades.Add(up);
        InitializeUpgrade(up);
    }

    /// <summary>
    /// When a level is loaded, call all upgrades in collection and initialize them. 
    /// </summary>
    public void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "Hub")
        {
            DestroyPUM();
            return;
        }
        int c = 0;
        do
        {
            c++;
            if (c >= 10000)
                break;

            player = FindObjectOfType<PlayerControllerRB>();


        } while (player == null);
        Debug.Log(c);
        if(player == null)
        {
            Debug.LogError("Trying to load player upgrades, but no player instance found!");
            return;
        }

        foreach(UpgradeObject up in upgrades)
        {
            InitializeUpgrade(up);
        }
    }

    /// <summary>
    /// When the run is over, destroy self.
    /// </summary>
    public void DestroyPUM()
    {
        instance = null;
        Destroy(this.gameObject);
    }
}
