using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerRB : IDamagable
{
    public float playerSpeed;
    public float lookSpeed;

    public Vector2 angleClamp;

    public GameObject cam;
    public PlayerControls controller;
    private InputAction move;
    private InputAction mouse;
    private InputAction jump;
    private InputAction debugRestart;

    private float verticalLookRotation = 0f;
    private float horizontalLookRotation = 0f;


    public GameObject playerBody;

    private Rigidbody rb;

    private Vector3 direction;

    [Header("Default Stuff")]
    public int maxJumps = 1;
    public int remainingJumps = 0;
    public float jumpCooldown;
    private float jumpT;

    [Header("Gravity Stuff")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float maxFallVelocity;
    [SerializeField] private float landVelocity;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private bool grounded = true;
    private float defGrav;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

       

        if(Physics.gravity.y >= -10)
            Physics.gravity *= gravityMultiplier;

        controller = new PlayerControls();
        move = controller.Player.Move;
        move.Enable();
        mouse = controller.Player.Mouse;
        mouse.Enable();
        jump = controller.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        debugRestart = controller.Player.RestartSceneDEBUG;
        debugRestart.performed += DebugRestartScene;
        debugRestart.Enable();

        verticalLookRotation = cam.transform.localRotation.x;

        if (PlayerUpgradeManager.instance.currHealth != 0)
        {
            health = PlayerUpgradeManager.instance.currHealth;
            UpdateSlider();
        }
    }

    public Transform GetGroundCheck()
    {
        return groundCheck;
    }

    public void DebugRestartScene(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        if(PlayerUpgradeManager.instance != null)
            PlayerUpgradeManager.instance.currHealth = health;

        move.Disable();
        mouse.Disable();
        jump.Disable();
        debugRestart.Disable();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Player Movement
        direction = transform.right * move.ReadValue<Vector2>().x + transform.forward * move.ReadValue<Vector2>().y;

        //playerBody.transform.LookAt(transform.position + direction);
        rb.MovePosition(transform.position + direction * playerSpeed * Time.deltaTime);

    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if(grounded && jumpT >= jumpCooldown)
        {
            remainingJumps = maxJumps;
        }

        if (jumpT < jumpCooldown)
            jumpT += Time.deltaTime;


        // look left and right
        transform.Rotate(new Vector3(0, mouse.ReadValue<Vector2>().x, 0) * Time.deltaTime * lookSpeed);

        //horizontalLookRotation += mouse.ReadValue<Vector2>().x * Time.deltaTime * lookSpeed;
        //horizontalLookRotation %= 360;

        // look up and down
        verticalLookRotation -= mouse.ReadValue<Vector2>().y * Time.deltaTime * lookSpeed;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, angleClamp.x, angleClamp.y);
        cam.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        //cam.transform.localRotation = Quaternion.Euler(verticalLookRotation, horizontalLookRotation, 0f);
    }

    private void Jump(InputAction.CallbackContext c)
    {
        if(remainingJumps > 0 && jumpT >= jumpCooldown)
        {
            jumpT = 0;
            grounded = false;
            remainingJumps--;
            // reset vertical velocity before applying jump
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Die()
    {
        Debug.Log("Player dead");
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        FindObjectOfType<GameOverScreen>().EnableDeathScreen();
    }
}
