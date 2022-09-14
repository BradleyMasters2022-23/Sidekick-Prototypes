using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSetupUnpack : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.DetachChildren();
        Destroy(this.gameObject);
    }
}
