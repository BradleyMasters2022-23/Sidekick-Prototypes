using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildrenOnSpawn : MonoBehaviour
{
    private void Awake()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }
}
