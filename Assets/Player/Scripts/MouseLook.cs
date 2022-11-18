using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
public class MouseLook : MonoBehaviour
{
    #region Variables
    [Header("PlayerInput")]
    private PlayerInput input;
    private InputAction look;

    [Header("Settings")]
    public float sens = 100f;

    [Header("Privates")]
    [SerializeField] private Transform camholder;
    [SerializeField] private Camera cam;
    private GameObject playerbody;
    private float xrotation;
    private float yrotation;
    private PhotonView view;
    #endregion

    #region Input
    private void OnEnable()
    {
        look = input.Player.Look;
        look.Enable();
    }
    private void OnDisable()
    {
        look.Disable();
    }
    #endregion

    #region Standard Functions
    void Awake()
    {
        input = new PlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        playerbody = GameObject.Find("Player");
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(cam.gameObject);
            gameObject.layer = 8;
            gameObject.tag = "Enemy";
        }
    }

    void Update()
    {
        if (!view.IsMine) return;

        float mouseX = look.ReadValue<Vector2>().x * sens * Time.deltaTime;
        float mouseY = look.ReadValue<Vector2>().y * sens * Time.deltaTime;

        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);
        
        camholder.localRotation = Quaternion.Euler(xrotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    #endregion
}
