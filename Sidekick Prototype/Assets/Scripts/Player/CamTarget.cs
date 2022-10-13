using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public Vector3 targetPos;
    public Transform defaultPos;

    public LayerMask layersToIgnore;

    [Range(0f, 3f)]
    [SerializeField] private float seeBuffer;

    RaycastHit hitInfo;

    Camera cam;
    Plane[] planes;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        targetPos = defaultPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Offset to fire raycast infront of player
        Vector3 firePoint = cam.transform.position + transform.forward * -transform.localPosition.z;

        if(Physics.Raycast(firePoint, cam.transform.forward, out hitInfo, Mathf.Infinity, ~layersToIgnore))
        {
            targetPos = hitInfo.point;
        }
        else
        {
            targetPos = defaultPos.position;
        }
    }

    private void FixedUpdate()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
    }

    public RaycastHit GetHit()
    {
        return hitInfo;
    }
    public Vector3 GetTarget()
    {
        return targetPos;
    }

    /// <summary>
    /// Check if a point is in vision of the camera
    /// </summary>
    /// <param name="pos">point to check</param>
    /// <returns>In camera vision</returns>
    public bool InCamVision(Vector3 pos)
    {
        foreach(Plane plane in planes)
        {
            if (plane.GetDistanceToPoint(pos) <= seeBuffer)
            {
                return false;
            }
        }

        return true;
    }
}
