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
        [SerializeField] private GameObject _waitingScreen;

        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;
        private bool _isWaitingForPlayer;

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

            var spawnPosition = new Vector3(0, -.4f, 1);
            PhotonNetwork.Instantiate(_playingField.name, spawnPosition, Quaternion.identity);
            
            GameManager.Instance.ScaleObjects();
        }

        private void CreatPlayer()
        {
            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, Vector3.zero, Quaternion.identity);
            TextMeshPro userName = player.GetComponentInChildren<TextMeshPro>();
            userName.text = PhotonNetwork.NickName;
        }
    }
}
