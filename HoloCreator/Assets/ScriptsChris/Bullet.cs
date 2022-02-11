using MRTK.Tutorials.MultiUserCapabilities;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool isUser = default;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ball") && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
