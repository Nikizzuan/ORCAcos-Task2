using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Passcode : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerEnterHandler,IPointerUpHandler
{

    static Passcode passcode;

    public GameObject LinePrefabs;
    public string itemName;
    public GameManager GM;
    private GameObject Line;
    public void OnDrag(PointerEventData eventData)
    {
        UpdateLine(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Line = Instantiate(LinePrefabs, transform.position, Quaternion.identity, transform.parent.parent);
      //  GM.addnumber(passcode.gameObject.GetComponent<Button>());
        UpdateLine(eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        passcode = this;

        //  UpdateLine(passcode.transform.position);
     
            UpdateLine(passcode.transform.position);
            GM.addnumber(passcode.gameObject.GetComponent<Button>());
       
           

      
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    
        if (!this.Equals(passcode) && itemName.Equals(passcode.itemName))
        {
            //UpdateLine(passcode.transform.position);
        }
        else {
            Destroy(Line);
        }
    }

    void UpdateLine(Vector3 position) {

        Vector3 direction = position - transform.position;
        Line.transform.right = direction;

        Line.transform.localScale = new Vector3((direction.magnitude / 20), 0.3f, 0.3f);
    
    }
   
}
