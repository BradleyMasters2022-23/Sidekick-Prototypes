using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private PhysicMaterial platformMat;
    private PhysicMaterial defaultMat;
    [SerializeField] private Transform[] waypoints;
    private int wpIndex;
    public float speed;

    private Rigidbody rb;

    void Awake()
    {
        defaultMat = GetComponent<Collider>().material;
        wpIndex = 0;
    }


    public void CheckWaypoint()
    {
        if(transform.position == waypoints[wpIndex].position)
        {
            if (wpIndex == waypoints.Length - 1)
                wpIndex = 0;
            else
                wpIndex++;
        }
    }


    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[wpIndex].position, speed * Time.deltaTime * TimeManager.worldTime);

        float surfacePos = transform.position.y + (transform.localScale.y / 2);

        if (rb != null && rb.GetComponent<PlayerControllerRB>().GetGroundCheck().position.y >= surfacePos)
        {
            // Get the direction its currently going
            Vector3 temp = waypoints[wpIndex].position - transform.position;

            // Apply the speed to the player object on front in the direction its moving
            rb.position += (temp.normalized * speed * Time.deltaTime * TimeManager.worldTime);

        }

        CheckWaypoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision enter hit");

        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            GetComponent<Collider>().material = platformMat;
            rb = collision.gameObject.GetComponent<Rigidbody>();

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("Collision exit hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            rb = null;
            GetComponent<Collider>().material = defaultMat;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
    //    {
    //        rb = other.gameObject.GetComponent<Rigidbody>();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        rb = null;
    //    }
    //}
}
