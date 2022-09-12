using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{
     
    [Header("GameScene")]
  
    private PhotonView view;
    public TextMeshProUGUI RandomNum;
    public TextMeshProUGUI playerInput;
    public TextMeshProUGUI Player1;
    public TextMeshProUGUI Player2;
    public Healthbar playerBar;
    public Button start;
    public GameObject winPanel;
    public TextMeshProUGUI winMsg;
    private bool gameHasStart;

    [Header("keypads")]
    public List<Button> Keypads;

    [Header("Timer")]
    private float currentMatchTime;
    private Coroutine timerCoroutine;
    public TextMeshProUGUI ui_timer;
    public int matchLength = 10;


    void Awake()
    {
       
        view = this.GetComponent<PhotonView>();
        start.interactable = false;
        gameHasStart = false;
    }

    private void Start()
    {

        view.RPC("UpdateReadystatus", RpcTarget.All);
       
        
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && gameHasStart == false)
        {
            start.interactable = true;
            gameHasStart = true;
        }
    }

    private void InitializeTimer()
    {

     
        currentMatchTime = matchLength;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
      
        timerCoroutine = StartCoroutine(Timer());
        
    }

    private void InitializePrivewTimer()
    {


        currentMatchTime = 10;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(StartCountdown());

    }

    private void RefreshTimerUI()
    {
        string minutes = (currentMatchTime / 60).ToString("00");
        string seconds = (currentMatchTime % 60).ToString("00");
        ui_timer.text = $"{seconds}";
    }

    private void initRandomNumber() {
        RandomNum.text = "";
        int rand;
        for (int i = 0; i < 9; i++)
        {
            do
            {
                rand = Random.Range(1, 10);
            } while (RandomNum.text.Contains(rand.ToString()));

            RandomNum.text = RandomNum.text + " " + rand;
        }
    }

    private void initKetpad()
    {
        string temp = "";
        int rand;
        foreach (var button in Keypads)
        {

            do
            {
                rand = Random.Range(1, 10);
            } while (temp.Contains(rand.ToString()));
            temp = temp + " " + rand;
            button.GetComponentInChildren<TextMeshProUGUI>().text = rand.ToString();


        }
        
    }

    public void addnumber(Button btn) {

        if (playerInput.text.Length > 16) return;
 
        playerInput.text = playerInput.text + " " + btn.GetComponentInChildren<TextMeshProUGUI>().text;
        checkPattern(playerInput.text);
    }



    private void checkPattern(string currentinput) {

        if (currentMatchTime > 0 && currentinput.Length >= 16)
        {
            if (!currentinput.Equals(RandomNum.text)) return;
           
            if (PhotonNetwork.IsMasterClient)
            {
                view.RPC("UpdateGui", RpcTarget.All, 10);
            }
            else {
                view.RPC("UpdateGui", RpcTarget.All, -10);
            }
            view.RPC("initround", RpcTarget.All);
        }
    
    }

    [PunRPC]
    private void initround() {
        playerInput.text = "";
        Player1.text = "Player 1";
        Player2.text = "Player 2";
        initKetpad();
        initRandomNumber();
        InitializeTimer();
        currentMatchTime = currentMatchTime - 5;


    }


    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        currentMatchTime -= 1;
        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
            
            view.RPC("initround", RpcTarget.All);
        }
        else
        {
            RefreshTimerUI();
            timerCoroutine = StartCoroutine(Timer());
        }

    }

    float currCountdownValue;
    private IEnumerator StartCountdown()
    {
        while (currentMatchTime > 0)
        {
           
            yield return new WaitForSeconds(1.0f);
            RefreshTimerUI();
            currentMatchTime -= 1;
        }
    }

    public void clear(TextMeshProUGUI playerInput) {

        playerInput.text = "";
    }

    public void StartGame(Button btn)
    {

        view.RPC("initround", RpcTarget.All);
        btn.interactable = false;
    }


    [PunRPC]
    private void UpdateGui(int amount)
    {
        if (playerBar.health >= 100)
        {
            winPanel.SetActive(true);
            winMsg.text = "Player 1 Win";
        }
        else if (playerBar.health <= 0)
        {
            winPanel.SetActive(true);
            winMsg.text = "Player 2 Win";
        }
        else { playerBar.GainHealth(amount); }
        
    }

    [PunRPC]
    private void UpdateReadystatus()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player1.text = "Player 1 Ready!!";
           
        }
        else
        {
            Player1.text = "Player 1 Ready!!";
            Player2.text = "Player 2 Ready!!";
           
        }
    }

    public void LeaveRoom()
    {

        PhotonNetwork.Disconnect();

    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("Lobby Scene");
    }



}




