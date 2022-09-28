using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public Vector3 targetPos;
    public Transform defaultPos;

    public LayerMask layersToIgnore;

    RaycastHit hitInfo;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        targetPos = defaultPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, Mathf.Infinity, ~layersToIgnore))
        {
            targetPos = hitInfo.point;
        }
        else
        {
            targetPos = defaultPos.position;
        }
    }

    public RaycastHit GetHit()
    {
        return hitInfo;
    }
    public Vector3 GetTarget()
    {
        return targetPos;
    }
}
