using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.InputSystem;
public class patterntracing : MonoBehaviour
{

    public GameObject LinePrefabs;

    Line activeLine;

    private void Start()
    {
    
    }

    private void Update()
    {
        Touch activeTouch = Touch.activeFingers[0].currentTouch;
        if (activeTouch.phase.ToString() == "Began")
        {
            GameObject newLine = Instantiate(LinePrefabs);
            activeLine = newLine.GetComponent<Line>();
        }


        if (activeTouch.phase.ToString() == "Began")
        {
            activeLine = null;
        }

        if (activeLine= null)
        {
            Vector2 screenpoint = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
            activeLine.updateLine(screenpoint);
        }
    }
}
