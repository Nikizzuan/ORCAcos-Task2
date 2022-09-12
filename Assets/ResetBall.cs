using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ResetBall : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    public void Resetball(GameObject loser)
    {
       // if (!view.IsMine) return;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        this.transform.position = loser.transform.GetChild(0).gameObject.transform.position;
    }

}
