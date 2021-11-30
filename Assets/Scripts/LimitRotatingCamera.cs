using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRotatingCamera : MonoBehaviour
{
    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;
    private bool editor;


    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        editor = true;
#else
        editor = false;
#endif
        CinemachineCore.GetInputAxis = this.HandleAxisInputDelegate;

    }

    public float HandleAxisInputDelegate(string axisName)
    {
        switch (axisName)
        {
            case "Mouse X":
                if (!editor)
                {
                    if (Input.touchCount > 0)
                    {
                        //Is mobile touch
                        return Input.touches[0].deltaPosition.x / TouchSensitivity_x;
                    }
                }
                if (editor)
                {
                    if (Input.GetMouseButton(0))
                    {
                        // is mouse click
                        return Input.GetAxis("Mouse X");
                    }
                }
                break;
            case "Mouse Y":
                if (!editor)
                {
                    if (Input.touchCount > 0)
                    {
                        //Is mobile touch
                        return Input.touches[0].deltaPosition.y / TouchSensitivity_y;
                    }
                }
                if (editor)
                {
                    if (Input.GetMouseButton(0))
                    {
                        // is mouse click
                        return Input.GetAxis(axisName);
                    }
                }
                break;
            default:
                Debug.LogError("Input <" + axisName + "> not recognized.", this);
                break;
        }

        return 0f;
    }
}
