using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "New Gun")]
public class GunSO : ScriptableObject
{
    public new string name;

    public float aimspeed;
    public float firerate;
    public float reloadtimeempty;
    public float reloadtime;

    public int maxammo;
    public int ammoreserve;
    public int damage;
}
