using System;
using MRTK.Tutorials.MultiUserCapabilities;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    private float _lifetime = 1;

    private void Update() 
    { 
        _lifetime -= Time.deltaTime; 
 
        if (_lifetime <= 0 && photonView.IsMine) 
        {
            PhotonNetwork.Destroy(gameObject); 
        }
    }
}
