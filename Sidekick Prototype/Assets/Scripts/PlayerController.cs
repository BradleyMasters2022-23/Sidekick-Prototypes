using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float lookSpeed;

    public Vector2 angleClamp;

    public GameObject cam;
    public CamTarget shootCam;
    public PlayerControls controller;
    private InputAction move;
    private InputAction mouse;
    private InputAction shoot;

    private float verticalLookRotation = 0f;

    public Transform shootPoint;
    public GameObject bullet;



    private Vector3 direction;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = new PlayerControls();
        move = controller.Player.Move;
        move.Enable();
        mouse = controller.Player.Mouse;
        mouse.Enable();
        shoot = controller.Player.Shoot;
        shoot.Enable();
        shoot.performed += Shoot;

        verticalLookRotation = cam.transform.localRotation.x;
    }

    private void OnDisable()
    {
        move.Disable();
        mouse.Disable();
        shoot.Disable();
    }

    private void Update()
    {
        // Player movement
        direction = new Vector3(move.ReadValue<Vector2>().x, 0, move.ReadValue<Vector2>().y); 
        transform.Translate(direction * playerSpeed * Time.deltaTime);
        
        // look left and right
        transform.Rotate(new Vector3(0, mouse.ReadValue<Vector2>().x, 0) * Time.deltaTime * lookSpeed);

        // look up and down
        verticalLookRotation -= mouse.ReadValue<Vector2>().y * Time.deltaTime * lookSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, angleClamp.x, angleClamp.y);
        cam.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Player shoot");

        GameObject t = Instantiate(bullet, shootPoint.position, transform.rotation);
        t.transform.LookAt(shootCam.GetTarget());
    }
}
