using System;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using UnityEngine;

public class HandShooter : MonoBehaviourPunCallbacks, IMixedRealityInputHandler
{
    [SerializeField] private GameObject _bulletPrefab;
    
    public override void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        if (PhotonNetwork.PrefabPool is DefaultPool pool)
        {
            if (_bulletPrefab == null)
            {
                return;
            }

            pool.ResourceCache.Add(_bulletPrefab.name, _bulletPrefab);
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction.Description != "Select") //Has to be a better way to determine event type
        {
            return;
        }
        
        var pointer = eventData.InputSource.Pointers.FirstOrDefault();

        if (pointer == null)
        {
            return;
        }

        var bullet = PhotonNetwork.Instantiate(_bulletPrefab.name, pointer.Position, pointer.Rotation);
        var shootDirection = pointer.BaseCursor.Position - pointer.Position;
        
        bullet.GetComponent<Rigidbody>().AddForce(shootDirection.normalized * 750);
    }
}
