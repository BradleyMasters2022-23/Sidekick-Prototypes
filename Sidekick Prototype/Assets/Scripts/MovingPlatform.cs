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

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.MoveTowards(transform.position, waypoints[wpIndex].position, speed * Time.deltaTime * TimeManager.worldTime);
        CheckWaypoint();
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
        velocity = this.GetComponent<Rigidbody>().velocity;

        if (rb != null)
        {
            Vector3 playerPos = rb.transform.position;
            Vector3 playerVel = rb.velocity;

            // Get the direction its currently going
            Vector3 temp = waypoints[wpIndex].position - transform.position;

            // Apply the speed to the player object on front in the direction its moving

            rb.position += (temp.normalized * speed * Time.deltaTime * TimeManager.worldTime);



            //rb.transform.position = Vector3.MoveTowards(rb.transform.position, )

        }
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
