using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Gun : MonoBehaviour
{
    #region Variables
    [Header("Input")]
    private PlayerInput input;
    private InputAction aim;
    private InputAction fire;
    private InputAction reload;

    [Header("Privates")]
    [SerializeField] private Transform aimpoint;
    [SerializeField] private Transform sightpoint;
    [SerializeField] private Transform gunpoint;
    [SerializeField] private GameObject pre;
    [SerializeField] private LayerMask playerlayer;
    [SerializeField] private LayerMask enemylayer;


    [SerializeField]private PhotonView view;
    private Transform firepoint;
    private Camera cam;
    protected Animator animator;

    private float aimspeed;
    private float firerate;
    private float reloadtime;
    private float reloadtimeempty;
    
    private int maxammo;
    private int ammoreserve;
    private int currentammo;
    private int damage;

    private bool fireing;
    private bool aimingdownsights;
    private bool reloading;

    private Recoil recoil;

    [Header("Data")]
    [SerializeField] private GunSO Data;

    #endregion

    #region standard functions
    private void Awake()
    {
        //components
        input = new PlayerInput();
        cam = GameObject.Find("FPS cam").GetComponent<Camera>();
        firepoint = GameObject.Find("Firepoint").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        recoil = GameObject.Find("CamRecoil").GetComponent<Recoil>();

        //data
        aimspeed = Data.aimspeed;
        firerate = Data.firerate;
        maxammo = Data.maxammo;
        ammoreserve = Data.ammoreserve;
        reloadtime = Data.reloadtime;
        reloadtimeempty = Data.reloadtimeempty;
        damage = Data.damage;

        
        currentammo = maxammo;
        aimpoint.position = gunpoint.position;
    }
    private void Update()
    {
        if (!view.IsMine) return;
        Aim();
        Fire();
        if (reload.IsPressed() && !reloading)
        {
            StartCoroutine(Reload());
        }
    }
    #endregion

    #region Input
    private void OnEnable()
    {
        aim = input.Player.Aim;
        fire = input.Player.Fire;
        reload = input.Player.Reload;
        aim.Enable();
        fire.Enable();
        reload.Enable();
    }
    private void OnDisable()
    {
        aim.Disable();
        fire.Disable();
        reload.Disable();
    }
    #endregion

    #region Functions
    public void Aim()
    {
        switch (aim.IsPressed())
        {
            case true:
                aimpoint.position = Vector3.Lerp(aimpoint.position, sightpoint.position, Time.deltaTime * aimspeed);
                aimingdownsights = true;
                break;
            case false:
                aimpoint.position = Vector3.Lerp(aimpoint.position, gunpoint.position, Time.deltaTime * aimspeed);
                aimingdownsights = false;
                break;
        }
    }

    public virtual void Fire()
    {
        if (fire.IsPressed() && !fireing && currentammo > 0 && !reloading)
        {
            StartCoroutine(FireBullet());
            currentammo--;
        }
        if(currentammo == 0)
        {
            //play empty sound
        }
        
    }

    public virtual IEnumerator FireBullet()
    {
        fireing = true;
        recoil.RecoilFire();
        RaycastHit hit;
        switch (aimingdownsights)
        {
            case false:
                //shoot ray from cam (add accuracy)
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit,enemylayer, ~playerlayer))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        GameObject GO = Instantiate(pre, hit.point, hit.transform.rotation);
                        Destroy(GO, .5f);

                        if (!hit.collider.gameObject.GetComponent<HealthScript>())
                        {
                            Debug.Log("NO HEALTH SCRIPT!!");
                            break;
                        }
                        hit.collider.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
                    }
                }
                break;
            case true:
                //shoot ray from firepoint(add accuracy)
                if (Physics.Raycast(firepoint.position, firepoint.forward, out hit, enemylayer, ~playerlayer))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        GameObject GO = Instantiate(pre,hit.point, hit.transform.rotation);
                        Destroy(GO,.5f);

                        if (!hit.collider.gameObject.GetComponent<HealthScript>())
                        {
                            Debug.Log("NO HEALTH SCRIPT!!");
                            break;
                        }
                        hit.collider.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
                    }
                }
                break;
        }
        yield return new WaitForSeconds(firerate);
        fireing = false;
    }
    public IEnumerator Reload()
    {
        reloading = true;
        float localreloadtime;

        int ammoneeded = maxammo - currentammo;
        switch (ammoneeded)
        {
            case 0:
                reloading = false;
                yield break;
            case 30:
                animator.SetBool("Reload_Empty", true);
                localreloadtime = reloadtimeempty;
                break;
            default:
                animator.SetBool("Reloading", true);
                localreloadtime = reloadtime;
                break;
        }

        yield return new WaitForSeconds(localreloadtime);

        ammoreserve -= ammoneeded;
        currentammo += ammoneeded;

        animator.SetBool("Reloading", false);
        animator.SetBool("Reload_Empty", false);
        reloading = false;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    #endregion
}
