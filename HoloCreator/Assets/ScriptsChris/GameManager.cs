using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using MoreMountains.Feedbacks;
using MRTK.Tutorials.MultiUserCapabilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    // Start is called before the first frame update
    [SerializeField] private NonNativeKeyboard keyboard = null;
    [SerializeField] private TextMeshPro textName = null;
    [SerializeField] private TextMeshPro textError = null;
    [SerializeField] private TextMeshPro RoomNumber = null;
    [SerializeField] private MMFeedbacks StartGameFeedback;
    [SerializeField] private MMFeedbacks JoinedRoomFeedback;
    public string Username = null;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartGameFeedback.Initialization();
        JoinedRoomFeedback.Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
