using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float starthp;
    private float health;
    private PhotonView view;
    private PlayerManager playerManager;
    
    private void Start()
    {
        health = starthp;
        view = GetComponent<PhotonView>();
        playerManager =  PhotonView.Find((int)view.InstantiationData[0]).GetComponent<PlayerManager>();
    }


    public void TakeDamage(float damage)
    {
        view.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if (!view.IsMine) return;
        health -= damage;
        if (health < 0) Die();
    }
    public void Die()
    {
        playerManager.Die();
    }
}
