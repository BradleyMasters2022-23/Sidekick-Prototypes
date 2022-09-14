using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    /// <summary>
    /// Reference to the main camera
    /// </summary>
    private Camera mainCamera;

    // Start is called before the first frame update
    void OnEnable()
    {
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Makes the singularity look at camera 
    /// </summary>
    void Update()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
