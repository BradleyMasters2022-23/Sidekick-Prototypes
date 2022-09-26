using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTrigger : MonoBehaviour
{
    //public RoomGenerator roomManager;
    public Color tempColor;
    public Color lockColor;
    public FieldType type;

    public enum FieldType
    {
        Door, 
        Entrance, 
        Exit
    }


    private void Awake()
    {
        GetComponent<Renderer>().material.color = lockColor;
    }


    public void SetEntrance()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        GameObject p = FindObjectOfType<PlayerControllerRB>().gameObject;
        p.transform.position = this.transform.position;
        p.transform.rotation = this.transform.rotation;
    }

    public void SetExit()
    {
        type = FieldType.Exit;
    }

    public void LockDoor()
    {
        GetComponent<Collider>().isTrigger = false;
        GetComponent<Renderer>().material.color = lockColor;
    }

    public void UnlockDoor()
    {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Renderer>().material.color = tempColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RoomGenerator.instance.SelectRoom();
        }
    }
}
