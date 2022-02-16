using Microsoft.MixedReality.Toolkit.Experimental.UI;
using MoreMountains.Feedbacks;
using MRTK.Tutorials.MultiUserCapabilities;
using System;
using Core;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private NonNativeKeyboard keyboard = null;
    [SerializeField] private TextMeshPro textName = null;
    [SerializeField] private TextMeshPro textError = null;
    [SerializeField] private TextMeshPro RoomNumber = null;
    [SerializeField] private TextMeshPro PlayerAScore = null;
    [SerializeField] private TextMeshPro PlayerBScore = null;
    [SerializeField] private TextMeshPro Message = null;
    [SerializeField] private MMFeedbacks StartGameFeedback;
    [SerializeField] private GameObject ToggleShooting;
    [SerializeField] private GameObject[] buttons;
    public string Username = null;
    private bool active = false;
    public TouchScreenKeyboard HoloKeyboard;
    private int ScoreA = 0;
    private int ScoreB = 0;

    void Start()
    {
        StartGameFeedback.Initialization();
    }

    private void Update()
    {
        if (HoloKeyboard != null)
        {
            UpdateText(HoloKeyboard.text);
        }

    }

    public void ShowKeyBoard()
    {
        if (Application.isEditor)
        {
            keyboard.PresentKeyboard();

            keyboard.OnClosed += DisableKeyboard;
            keyboard.OnTextSubmitted += DisableKeyboard;
            keyboard.OnTextUpdated += UpdateText;
        }
        else
        {
            HoloKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        }
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
        Message.text = "Score 5 goals to win!";
        
        foreach(GameObject button in buttons)
        {
            button.SetActive(true);
        }
    }

    public void UpdateScore(int addA, int addB)
    {
        ScoreA += addA;
        ScoreB += addB;

        PlayerAScore.text = "Player 1 Score: " + ScoreA;
        PlayerBScore.text = "Player 2 Score: " + ScoreB;

        if (ScoreA > 4)
        {
            Message.text = "Congratulations, Player A Wins!";
        }
        else if (ScoreB > 4)
        {
            Message.text = "Congratulations, Player B Wins!";
        }
        else
        {
            Message.text = "Score 5 goals to win!";
        }

    }

    public void ResetGame()
    {
        ScoreA = 0;
        ScoreB = 0;

        PlayerAScore.text = "Player A Score: " + ScoreA;
        PlayerBScore.text = "Player B Score: " + ScoreB;
        Message.text = "First player to score 5 goals wins!";

        RoomManager.Instance.ResetBall();
    }

    public void ToggleHandShooter()
    {

        ToggleShooting.SetActive(!active);

    }
}
