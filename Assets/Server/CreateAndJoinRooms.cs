using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createfield;
    [SerializeField] private TMP_InputField joinfield;
    [SerializeField] private TMP_InputField namefield;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createfield.text);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinfield.text);
    }
    public void SetName()
    {
        PhotonNetwork.NickName = namefield.text;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Game");
    }
}
