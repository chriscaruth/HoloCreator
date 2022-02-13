using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class PhotonUser : MonoBehaviour
    {
        private PhotonView _pv;
        private TextMeshPro _text;

        private void Start()
        {
            _pv = GetComponent<PhotonView>();
            _text = GetComponentInChildren<TextMeshPro>();
            
            if (_text != null)
            {
                _text.text = _pv.Owner.NickName;
            }

            if (!_pv.IsMine) return;
            
            _pv.RPC("PunRPC_SetNickName", RpcTarget.AllBuffered, _text.text);
        }

        [PunRPC]
        private void PunRPC_SetNickName(string nName)
        {
            gameObject.name = nName;
        }

        [PunRPC]
        private void PunRPC_ShareAzureAnchorId(string anchorId)
        {
            GenericNetworkManager.Instance.azureAnchorId = anchorId;

            Debug.Log("\nPhotonUser.PunRPC_ShareAzureAnchorId()");
            Debug.Log("GenericNetworkManager.instance.azureAnchorId: " + GenericNetworkManager.Instance.azureAnchorId);
            Debug.Log("Azure Anchor ID shared by user: " + _pv.Controller.UserId);
        }

        public void ShareAzureAnchorId()
        {
            if (_pv != null)
                _pv.RPC("PunRPC_ShareAzureAnchorId", RpcTarget.AllBuffered,
                    GenericNetworkManager.Instance.azureAnchorId);
            else
                Debug.LogError("PV is null");
        }
    }
}
