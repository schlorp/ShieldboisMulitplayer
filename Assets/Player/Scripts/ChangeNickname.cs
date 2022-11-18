using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChangeNickname : MonoBehaviour
{
    [SerializeField] private PhotonView playerview;
    private TMP_Text nicknametext;
    private void Start()
    {
        nicknametext = GetComponent<TMP_Text>();
        nicknametext.text = playerview.Owner.NickName;
    }

}
