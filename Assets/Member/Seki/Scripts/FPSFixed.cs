using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FPS�Œ�
/// </summary>
public class FPSFixed : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
