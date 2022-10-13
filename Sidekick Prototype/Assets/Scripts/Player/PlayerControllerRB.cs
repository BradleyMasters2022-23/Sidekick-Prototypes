using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControllerRB : IDamagable
{
    private Rigidbody rb;

    [Header("---Camera---")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 angleClamp;
    [SerializeField] private GameObject camPivotPoint;
    private PlayerControls controller;
    private InputAction move;
    private InputAction mouse;
    private InputAction jump;
    private InputAction debugRestart;
    private InputAction sprint;

    private float verticalLookRotation = 0f;
    private float horizontalLookRotation = 0f;

    [Header("---Movement---")]
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] [Range(0, 1)] private float accelerationTime;
    [SerializeField] [Range(0, 1)] private float startingSpeedPercentage;
    [SerializeField] [Range(0, 1)] private float decelerationTime;
    [SerializeField] [Range(0, 1)] private float airModifier;
    [SerializeField] [Range(1, 3)] private float sprintModifier;
    [SerializeField] Animator playerAnimator;
    /// <summary>
    /// Direction the player is inputting
    /// </summary>
    private Vector3 direction;
    private Vector2 lastInput = new Vector2(0,0);
    private float currSpeed;
    private float accelerationTimer;
    private float decelerationTimer;
    private bool sprinting;

    [Header("---Jumping---")]
    [SerializeField] private float jumpForce;
    [Tooltip("Cooldown for both refreshing jumps and between jumps")]
    [SerializeField] private float jumpCooldown;
    [Tooltip("Whether or not the player loses their first jump if falling off a ledge")]
    [SerializeField] private bool disableFirstJumpOnFall;
    [Tooltip("Whether or not the player can pivot movement when jumping")]
    [SerializeField] private bool jumpRedirectControl;
    public int maxJumps = 1;
    private int remainingJumps = 0;
    private float jumpT;


    [Header("---Gravity---")]
    [SerializeField] private float gravityMultiplier;
    //[SerializeField] private float maxFallVelocity;
    //[SerializeField] private float landVelocity;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    private bool grounded = true;

    //Animation
    public Animator pennyAnimator;


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

        sprint = controller.Player.Sprint;
        sprint.started += ToggleSprint;
        sprint.canceled += ToggleSprint;
        sprint.Enable();

        verticalLookRotation = camPivotPoint.transform.localRotation.x;

        if (!sectionedHealth && PlayerUpgradeManager.instance.currHealth != 0)
        {
            health = PlayerUpgradeManager.instance.currHealth;
            UpdateSlider();
        }
        else if(PlayerUpgradeManager.instance.currHealth != 0)
        {
            LoadSectionedHealth();
        }

        currSpeed = 0;
        accelerationTimer = 0;

    }

    private void Update()
    {
        ManageCamera();

        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        // Remove the first jump if they fall off
        if (disableFirstJumpOnFall && !grounded && remainingJumps == maxJumps)
            remainingJumps--;

        // Restore jumps on ground
        if (grounded && jumpT >= jumpCooldown)
            remainingJumps = maxJumps;
        
        // Manage jump cooldown timer
        if (jumpT < jumpCooldown)
            jumpT += Time.deltaTime;

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Reset timers
        if (lastInput == Vector2.zero)
        {
            accelerationTimer = accelerationTime * startingSpeedPercentage;
        }
        else
        {
            decelerationTimer = 0f;
        }

        // Lerp speed if accelerating or decelerating
        if (move.ReadValue<Vector2>() != Vector2.zero
            && currSpeed < maxMoveSpeed)
        {
            accelerationTimer += Time.deltaTime;

            currSpeed = Mathf.Lerp(0, maxMoveSpeed, (accelerationTimer / accelerationTime));
        }
        else if (move.ReadValue<Vector2>() == Vector2.zero
            && currSpeed > 0)
        {
            decelerationTimer += Time.deltaTime;

            currSpeed = Mathf.Lerp(maxMoveSpeed, 0, (decelerationTimer / decelerationTime));
        }


        ManageMovement();
        lastInput = move.ReadValue<Vector2>();
    }

    #region Player Movement

    private void ManageMovement()
    {
        // Get the player input
        Vector2 currInput = move.ReadValue<Vector2>();

        // Limit the velocity when on the ground
        Vector2 limitVel = new Vector2(rb.velocity.x, rb.velocity.z);
        if (grounded && limitVel.magnitude > maxMoveSpeed && !sprinting)
        {
            limitVel = limitVel.normalized * maxMoveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.y);
        }
        else if (grounded && limitVel.magnitude > (maxMoveSpeed * sprintModifier) && sprinting)
        {
            limitVel = limitVel.normalized * maxMoveSpeed * sprintModifier;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.y);
        }

        // Variables for animation 
        float _xaxis = rb.velocity.x;
        float _zaxis = rb.velocity.z;

        pennyAnimator.SetFloat("X Move", currInput.x);
        pennyAnimator.SetFloat("Y Move", currInput.y);

        if (grounded)
        {
            // Calculate grounded movement
            Vector3 temp;
            direction = transform.right * currInput.x + transform.forward * currInput.y;

            if (direction != Vector3.zero)
            {
                temp = Mathf.Pow(currSpeed, 2) * Time.deltaTime * direction;
            }
            else
            {
                temp = Mathf.Pow(currSpeed, 2) * Time.deltaTime * rb.velocity.normalized;
            }

            temp.y = rb.velocity.y;
            rb.velocity = temp;

            //Animation
            pennyAnimator.SetBool("Is Jumping", false);
        }
        else
        {
            // Calculate midair movement
            if (airModifier != 0)
            {
                // Calculate new horizontal movement
                Vector3 airDir = transform.right * currInput.x + transform.forward * currInput.y;
                airDir = airDir.normalized * Mathf.Pow((maxMoveSpeed * airModifier), 2) * Time.deltaTime;

                airDir.y = 0;

                // Get new velocity
                Vector3 newVelocity = rb.velocity + airDir;
                Vector3 newXZVelocity = new Vector3(newVelocity.x, 0, newVelocity.z);

                // limit the horizontal speed, seperately from the vertical speed
                if (newXZVelocity.magnitude > maxMoveSpeed / 2)
                {
                    newXZVelocity = newXZVelocity.normalized * (maxMoveSpeed / 2);
                    newVelocity.y = rb.velocity.y;
                    newVelocity.x = newXZVelocity.x;
                    newVelocity.z = newXZVelocity.z;
                }

                rb.velocity = newVelocity;

                //Animation
                pennyAnimator.SetBool("Is Jumping", true);

            }            
        }
    }

    private void ManageCamera()
    {
        // look left and right
        transform.Rotate(new Vector3(0, mouse.ReadValue<Vector2>().x, 0) * Time.deltaTime * lookSensitivity);

        // look up and down
        verticalLookRotation -= mouse.ReadValue<Vector2>().y * Time.deltaTime * lookSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, angleClamp.x, angleClamp.y);
        camPivotPoint.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    private void Jump(InputAction.CallbackContext c)
    {
        if(remainingJumps > 0 && jumpT >= jumpCooldown)
        {
            if(jumpRedirectControl)
            {
                // Allow for redirection on jumps
                Vector2 currInput = move.ReadValue<Vector2>();
                Vector3 airDir = transform.right * currInput.x + transform.forward * currInput.y;
                airDir = airDir.normalized * Mathf.Pow((maxMoveSpeed), 2) * Time.deltaTime;
                airDir.y = 0;

                Vector3 newVelocity = Vector3.zero + airDir;

                // limit the speed
                if (newVelocity.magnitude > maxMoveSpeed / 2)
                {
                    newVelocity = newVelocity.normalized * (maxMoveSpeed / 2);
                    newVelocity.y = rb.velocity.y;
                }
                rb.velocity = newVelocity;
            }
            
            jumpT = 0;
            grounded = false;
            remainingJumps--;
            // reset vertical velocity before applying jump
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ToggleSprint(InputAction.CallbackContext ctx)
    {
        sprinting = !sprinting;

        if(sprinting)
        {
            currSpeed = maxMoveSpeed * sprintModifier;
        }
        else
        {
            currSpeed = maxMoveSpeed;
        }
    }

    public Transform GetGroundCheck()
    {
        return groundCheck;
    }

    #endregion


    #region Misc

    public void DebugRestartScene(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        health = maxHealth;
        if (sectionedHealth)
            LoadSectionedHealth();
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        if (PlayerUpgradeManager.instance != null)
            PlayerUpgradeManager.instance.currHealth = health;

        move.Disable();
        mouse.Disable();
        jump.Disable();
        debugRestart.Disable();
        sprint.Disable();
    }

    private void LoadSectionedHealth()
    {
        health = PlayerUpgradeManager.instance.currHealth;
        int sectionToHeal = (int)Mathf.Ceil(health / (float)healthPerSection);
        health = sectionToHeal * healthPerSection;

        sectionIndex = sectionToHeal - 1;

        for (int i = numOfSections - 1; i >= sectionToHeal; i--)
        {
            sections[i].value = 0;
        }
    }


    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Die()
    {
        Debug.Log("Player dead");
        if (invulnerable)
            return;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        FindObjectOfType<GameOverScreen>().EnableDeathScreen();
    }

    #endregion
}
