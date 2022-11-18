using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKGunScript : Gun
{
    public override IEnumerator FireBullet()
    {
        //play sound
        animator.Play("Fire_Anim");
        return base.FireBullet();
    }
}
