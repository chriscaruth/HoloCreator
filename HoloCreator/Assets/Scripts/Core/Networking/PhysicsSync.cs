using Photon.Pun;
using UnityEngine;

namespace Core.Networking
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsSync : MonoBehaviourPunCallbacks, IPunObservable
    {
        private Rigidbody _rigidbody;
        private Vector3 _networkPosition;
    
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _networkPosition, Time.fixedDeltaTime);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rigidbody.position);
                stream.SendNext(_rigidbody.velocity);
            }
            else
            {
                _networkPosition = (Vector3) stream.ReceiveNext();
                _rigidbody.velocity = (Vector3) stream.ReceiveNext();

                var lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                _rigidbody.position += _rigidbody.velocity * lag;
            }
        }
    }
}
