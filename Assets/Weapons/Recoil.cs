using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentrotation;
    private Vector3 targetrotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [Header("Settings")]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnspeed;


    private void Update()
    {
        targetrotation = Vector3.Lerp(targetrotation, Vector3.zero, returnspeed * Time.deltaTime);
        currentrotation = Vector3.Slerp(currentrotation, targetrotation, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentrotation);
    }

    public void RecoilFire()
    {
        targetrotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
