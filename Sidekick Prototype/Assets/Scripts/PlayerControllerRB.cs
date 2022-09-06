using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerRB : MonoBehaviour
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
    private InputAction jump;
    private InputAction debugRestart;

    private float verticalLookRotation = 0f;

    public Transform shootPoint;
    public GameObject bullet;

    private CharacterController pc;

    private Rigidbody rb;

    private Vector3 direction;

    [Header("Gravity Stuff")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float maxFallVelocity;
    [SerializeField] private float landVelocity;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool grounded = true;
    Vector3 velocity;

    private void Start()
    {
        gravity *= gravityMultiplier;

        Cursor.lockState = CursorLockMode.Locked;

        //pc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        controller = new PlayerControls();
        move = controller.Player.Move;
        move.Enable();
        mouse = controller.Player.Mouse;
        mouse.Enable();
        shoot = controller.Player.Shoot;
        shoot.Enable();
        shoot.performed += Shoot;
        jump = controller.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        debugRestart = controller.Player.RestartSceneDEBUG;
        debugRestart.performed += DebugRestartScene;
        debugRestart.Enable();

        verticalLookRotation = cam.transform.localRotation.x;
    }

    public void DebugRestartScene(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        move.Disable();
        mouse.Disable();
        shoot.Disable();
        jump.Disable();
        debugRestart.Disable();
    }

    private void FixedUpdate()
    {
        direction = transform.right * move.ReadValue<Vector2>().x + transform.forward * move.ReadValue<Vector2>().y;
        rb.MovePosition(transform.position + direction * playerSpeed * Time.deltaTime);
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        //RaycastHit hitInfo;

        //if (grounded && Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity))
        //{
        //    if (transform.parent != hitInfo.transform)
        //    {
        //        transform.SetParent(hitInfo.transform, true);
        //        Debug.Log("Assigning new parent");
        //    }
        //}
        //else
        //{
        //    if (transform.parent != null)
        //    {
        //        transform.SetParent(null, true);
        //        Debug.Log("Clearing parent");
        //    }
        //}


        // Player movement
        //direction = transform.right * move.ReadValue<Vector2>().x + transform.forward * move.ReadValue<Vector2>().y;
        //pc.Move(direction * playerSpeed * Time.deltaTime);

        // look left and right
        transform.Rotate(new Vector3(0, mouse.ReadValue<Vector2>().x, 0) * Time.deltaTime * lookSpeed);

        // look up and down
        verticalLookRotation -= mouse.ReadValue<Vector2>().y * Time.deltaTime * lookSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, angleClamp.x, angleClamp.y);
        cam.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        //Gravity();

        //pc.Move(velocity * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext c)
    {
        //if (grounded)
        //{
        //    Debug.Log("Jumping");
        //    grounded = false;
        //    velocity.y = jumpForce;
        //    pc.Move(velocity * Time.deltaTime);
        //}

        if(grounded)
        {
            grounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Gravity()
    {
        if (grounded && velocity.y != landVelocity)
        {
            velocity.y = landVelocity;
            pc.Move(velocity);
        }
        else if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, -maxFallVelocity, maxFallVelocity);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Shoot(InputAction.CallbackContext c)
    {
        GameObject t = Instantiate(bullet, shootPoint.position, transform.rotation);
        t.transform.LookAt(shootCam.GetTarget());
    }

    
}
