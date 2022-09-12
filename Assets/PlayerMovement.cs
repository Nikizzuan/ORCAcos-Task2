using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class PlayerMovement : MonoBehaviourPunCallbacks
{

    public bool isholdingball;
    public float speed;
    public float shootPointSpeed;
    public GameObject shootPoint;
    public GameObject playBall;
    public Rigidbody2D rb;
    Vector3 moveDirection = Vector2.zero;
    Vector3 AimDirection = Vector2.zero;
    public Vector2 startpostion;
    private GameObject ball;
    [SerializeField] private PhotonView view;

    public float lunchForce;
    private Vector2 direction;
    public InputAction PlayerControl;
    bool waspress = false;

    private void OnEnable()
    {
        if (!view.IsMine) return;
        PlayerControl.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        if (!view.IsMine) return;
        PlayerControl.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        if (!view.IsMine) return;
       startpostion = this.gameObject.transform.position;
       ball = this.gameObject.transform.GetChild(0).gameObject;
     
        rb = this.GetComponent<Rigidbody2D>();
    }



    private void Update()
    {
  
        if (!view.IsMine) return;
        if (isholdingball)
        {
           
            Aimmovement();
        }
        else
        {

            movement();

        }
    }

    private void movement()
    {
        
        #if UNITY_ANDROID || UNITY_IOS
                //gets the primary touch position using the new input system
               moveDirection = Touchscreen.current.primaryTouch.position.ReadValue();

        #elif UNITY_STANDALONE
                //gets the current mouse position using the new input system
                moveDirection = Mouse.current.position.ReadValue();

        #endif
             
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(moveDirection);
            Vector2 myPosition = rb.position;

        if (Mathf.Abs(mousePosition.y - myPosition.y) <= 2)
        {
            myPosition.x = Mathf.Lerp(myPosition.x, mousePosition.x, 10);
            myPosition.x = Mathf.Clamp(myPosition.x, -1.95f, 1.95f);
            rb.position = myPosition;
        }

        }


    private void Aimmovement()
    {
#if UNITY_ANDROID || UNITY_IOS
        //gets the primary touch position using the new input system
       AimDirection = Touchscreen.current.primaryTouch.position.ReadValue();
        Touch activeTouch = Touch.activeFingers[0].currentTouch;
        if (activeTouch.phase.ToString() == "Ended")
        {
            waspress = true;
        }
       
#elif UNITY_STANDALONE
        //gets the current mouse position using the new input system
        AimDirection = Mouse.current.position.ReadValue();
         waspress = Mouse.current.rightButton.wasReleasedThisFrame;
#endif
      
    if (waspress)
    {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Shoot", RpcTarget.All, direction);

        }
    else
    {
        Vector2 ballPosition = transform.position;
        Vector2 ArrowPositon = Camera.main.ScreenToWorldPoint(AimDirection);
        //Mouse.current.position.ReadValue()
         direction = ArrowPositon * shootPointSpeed - ballPosition;
        if (PhotonNetwork.IsMasterClient)
        {
            shootPoint.transform.right = -direction;
        }
        else
        {
            shootPoint.transform.right = direction;
        }
    }
        
    }

    [PunRPC]
    void Shoot(Vector2 direction)
    {


        playBall = GameObject.FindGameObjectWithTag("Ball");
  
  
           playBall.GetComponent<Rigidbody2D>().AddForce(direction * lunchForce, ForceMode2D.Force);

     
        this.GetComponentInParent<PlayerMovement>().isholdingball = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        waspress = false;
       // this.GetComponent<PlayerMovement>().PlayerControl.Disable();

    }
}
