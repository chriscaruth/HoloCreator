using System;
using Core;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    public class RoomManager : SingletonPun<RoomManager>
    {
        [SerializeField] private GameObject photonUserPrefab = default;
        [SerializeField] private GameObject _playingField;
        [SerializeField] private GameObject _ball;   
        [SerializeField] private GameObject _waitingScreen;


        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;
        private bool _isWaitingForPlayer;
        private GameObject _fieldSpawn;
        private GameObject _ballSpawn;

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
        }

        private void Update()
        {
            if (_isWaitingForPlayer && playersInRoom == 2)
            {
                LoadPlayingField();
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            if (PhotonNetwork.PrefabPool is DefaultPool pool)
            {
                if (photonUserPrefab != null)
                {
                    pool.ResourceCache.Add(photonUserPrefab.name, photonUserPrefab);
                }

                if (_playingField != null)
                {
                    pool.ResourceCache.Add(_playingField.name, _playingField);
                }

                if (_ball != null)
                {
                    pool.ResourceCache.Add(_ball.name, _ball);
                }
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;

 
            CreatPlayer();

            if (myNumberInRoom == 1)
            {
                _isWaitingForPlayer = true;
                _waitingScreen.SetActive(true);

            }
        }

        private void StartGame()
        {
            CreatPlayer();
        }

        private void LoadPlayingField()
        {
            _isWaitingForPlayer = false;
            
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            
            _waitingScreen.SetActive(false);

            var spawnFieldPosition = new Vector3(0, -.4f, 1.5f);
            _fieldSpawn = PhotonNetwork.Instantiate(_playingField.name, spawnFieldPosition, Quaternion.identity);

            SpawnBall();

        }

        public void SpawnBall()
        {
            _ballSpawn = PhotonNetwork.Instantiate(_ball.name, new Vector3(_fieldSpawn.transform.position.x, _fieldSpawn.transform.position.y + 1f, _fieldSpawn.transform.position.z), Quaternion.identity);
        }

        public void DestroyBall()
        {
            if (_ballSpawn != null)
            {
                PhotonNetwork.Destroy(_ballSpawn);
            }
        }

        public void ResetBall()
        {
            photonView.RPC("ResetBallRPC", RpcTarget.All);
        }

        [PunRPC]
        public void ResetBallRPC()
        {
            DestroyBall();
            SpawnBall();
        }

        private void CreatPlayer()
        {
            var position = playersInRoom == 1 
                ? Vector3.zero 
                : new Vector3(0, 0, 3);

            var rotation = playersInRoom == 1
                ? Quaternion.identity
                : Quaternion.Euler(0, 180, 0);
            
            PhotonNetwork.Instantiate(photonUserPrefab.name, position, rotation);
        }
    }
}
