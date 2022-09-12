using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public GameObject MainMenu;

    [Header("Main Screen")]
    public GameObject player1;
    public GameObject player2;
    public TMP_InputField createNameInput;
    public TMP_InputField joinNameInput;
    public TextMeshProUGUI errorMsg;


    void Awake()
    {
        // if an instance already exists and it's not this one - destroy us
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            // set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        errorMsg.text = "Disconnected :" + cause;
        Debug.Log(cause);
        base.OnDisconnected(cause);
    }

    public override void OnConnectedToMaster()
    {
        errorMsg.text = "Connected to photon";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MainMenu.SetActive(true);

    }


    public void CreateRoom()
    {

        /*if (createNameInput.text.Length < 1)
        {
            Debug.LogError("Error: please check if the name inputfield and the create room inputfield are not empty!");
            errorMsg.text = "Error: please check if the name inputfield is not empty!";
            return;
        }
        if (createNameInput.text.Length < 1)
        {
            Debug.LogError("Error: please check if the name inputfield and the create room inputfield are not empty!");
            errorMsg.text = "Error: please check if the name inputfield is not empty!";
            return;
        }*/
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom("New", roomOptions, null);
        
    }

    public override void OnCreatedRoom()
    {
        
        Debug.Log("Created room");
    }

    public void JoinRoomDirect()
    {
        if (joinNameInput.text.Length < 1)
        {
            Debug.LogError("Error: please check if the name inputfield is not empty!");
            errorMsg.text = "Error: please check if the name inputfield is not empty!";
            return;
        }
        PhotonNetwork.JoinRoom(joinNameInput.text);
    }
    public void JoinRoom(string roomname)
    {
       /* if (roomname.Length < 1)
        {
            Debug.LogError("Error: please check if the name inputfield is not empty!");
            errorMsg.text = "Error: please check if the name inputfield is not empty!";
            return;
        }*/
        PhotonNetwork.JoinRoom(roomname);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game Scene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error: creating room : " + returnCode);
        errorMsg.text = "Error: creating room : " + returnCode;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinRandomOrCreateRoom(null, 2, MatchmakingMode.FillRoom, TypedLobby.Default, null, null, roomOptions, null);
        // JoinRoom("New");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error: joining exists : " + returnCode);
        errorMsg.text = "Error: joining exists : " + returnCode;

    }


}
