using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class rigidbodysync : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    Vector2 _networkPosition;
    Quaternion _networkRotation;
    Rigidbody2D _rb;
    [SerializeField] private PhotonView view;

    public float teleportfarDistance;

    public float smoothPos = 2.0f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
           // stream.SendNext(_rb.rotation);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _networkPosition = (Vector2)stream.ReceiveNext();
           // _networkRotation = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _networkPosition += (_rb.velocity * lag);
        }
    }

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (!view.IsMine)
        {
            _rb.position = Vector3.MoveTowards(_rb.position, _networkPosition, smoothPos *Time.fixedDeltaTime);
           // _rb.rotation = Quaternion.RotateTowards(_rb.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);

            if (Vector3.Distance(_rb.position, _networkPosition) > teleportfarDistance)
            {
                _rb.position = _networkPosition;
            }
        }
    }
}
