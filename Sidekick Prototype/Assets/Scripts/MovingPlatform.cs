using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints;
    public int wpIndex;
    public float speed;

    public Rigidbody rb;

    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
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
        

        if (rb != null)
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
        Debug.Log("Collision enter hit");


        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            rb = collision.gameObject.GetComponent<Rigidbody>();

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            rb = null;


        }
    }
}
