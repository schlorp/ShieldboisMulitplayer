using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Movement : MonoBehaviour
{
    #region Variables
    [Header("Playerinput")]
    private PlayerInput playerinput;
    private InputAction move;
    private InputAction jump;
    private Vector2 movedirection;
    private Vector3 movedata;
    private PhotonView view;


    [Header("movement variables")]
    public CharacterController cc;
    private float speed;
    public float walkingspeed = 15;
    public float sprintspeed = 20;
    public float grav = -9.81f;
    public float jumpheight = 2f;

    [Header("masks")]
    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("jumping")]
    public Vector3 velocity;
    bool grounded;

    #endregion

    #region Input
    private void OnEnable()
    {
        move = playerinput.Player.Move;
        jump = playerinput.Player.Jump;
        move.Enable();
        jump.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    #endregion

    #region Standard Functions
    private void Awake()
    {
        //inputs
        playerinput = new PlayerInput();
        view = GetComponent<PhotonView>();
        speed = walkingspeed;
    }
    
    void Update()
    {
        if (!view.IsMine) return;
        grounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        movedirection = move.ReadValue<Vector2>();
        movedata = transform.right * movedirection.x + transform.forward * movedirection.y;

        MovePlayer();
        Jump();
        velocity.y += grav * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
    #endregion


    #region Functions
    public void MovePlayer()
    {
        cc.Move(movedata * speed * Time.deltaTime);
    }
    public void Jump()
    {
        if (jump.triggered && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2 * grav);
        }
    }
    #endregion
}
