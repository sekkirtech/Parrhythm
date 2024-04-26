using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    private @ControllerInput CtrlInput;

    void Awake()
    {
        CtrlInput = new ControllerInput();
        CtrlInput.Enable();
    }


}
