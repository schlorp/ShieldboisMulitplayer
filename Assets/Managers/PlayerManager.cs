using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    private PhotonView view;
    private string nickname;
    private GameObject player;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (view.IsMine)
        {
            CreateController();
        }
    }
    void CreateController()
    {
        float randomX = Random.Range(-20, 20);
        float randomZ = Random.Range(-20, 20);

        
        player = PhotonNetwork.Instantiate("Player", new Vector3(randomX, 2, randomZ), Quaternion.identity, 0, new object[] {view.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(player);
        CreateController();
    }

}
