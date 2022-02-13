using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using MoreMountains.Feedbacks;
using MRTK.Tutorials.MultiUserCapabilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private NonNativeKeyboard keyboard = null;
    [SerializeField] private TextMeshPro textName = null;
    [SerializeField] private TextMeshPro textError = null;
    [SerializeField] private TextMeshPro RoomNumber = null;
    [SerializeField] private MMFeedbacks StartGameFeedback;
    [SerializeField] private MMFeedbacks JoinedRoomFeedback;
    [SerializeField] private MMFeedbacks ScaleObjectFeedback;
    [SerializeField] private GameObject ScaleField;
    public string Username = null;

    void Start()
    {
        StartGameFeedback.Initialization();
        JoinedRoomFeedback.Initialization();
        ScaleObjectFeedback.Initialization();
    }

    public void ShowKeyBoard()
    {
        keyboard.PresentKeyboard();

        keyboard.OnClosed += DisableKeyboard;
        keyboard.OnTextSubmitted += DisableKeyboard;
        keyboard.OnTextUpdated += UpdateText;
    }

    private void UpdateText(string text)
    {
        Username = text;
        textName.text = "Username: " + Username;
    }

    private void DisableKeyboard(object sender, EventArgs e)
    {
        keyboard.OnTextUpdated -= UpdateText;
        keyboard.OnClosed -= DisableKeyboard;
        keyboard.OnTextSubmitted -= DisableKeyboard;

        keyboard.Close();
    }

    public void StartGame()
    {
        if (Username.Length > 0)
        {
            Debug.Log("Start Game");
            
            StartGameFeedback.PlayFeedbacks();
            
            GenericNetworkManager.Instance.ConnectToNetwork();
        }
        else
        {
            textError.text = "Please enter in a username";
        }
    }

    public void JoinedRoom(string Room)
    {
        RoomNumber.text = "Room Number: " + Room;
        JoinedRoomFeedback.PlayFeedbacks();
    }

    public void ScaleObjects()
    {
        if (ScaleField.transform.localScale.x < 0.1f)
        {
            ScaleObjectFeedback.PlayFeedbacks();
        }
    }
}
