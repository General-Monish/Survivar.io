using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickMovement : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 JoyStickVec;
    public Vector2 JoyStickTouchPos;
    public Vector2 JoyStickOriginalPos;
    private float joyStickkRadius;
    // Start is called before the first frame update
    void Start()
    {
        JoyStickOriginalPos = joystickBG.transform.position;
        joyStickkRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;

    }

    public void PointerDown()
    {
        joystick.transform.position =Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        JoyStickTouchPos = Input.mousePosition;
    }
 
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        JoyStickVec = (dragPos - JoyStickTouchPos).normalized;

        float joystickDistance = Vector2.Distance(dragPos, JoyStickTouchPos);

        if (joystickDistance < joyStickkRadius)
        {
            joystick.transform.position = JoyStickTouchPos + JoyStickVec * joystickDistance;

        }
        else
        {
            joystick.transform.position = JoyStickTouchPos + JoyStickVec * joyStickkRadius;

        }
    }
    public void PointerUp()
    {
        JoyStickVec = Vector2.zero;
        joystick.transform.position = JoyStickOriginalPos;
        joystickBG.transform.position = JoyStickOriginalPos;
    }
}
