using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadCaller : MonoBehaviour
{
    private SelectableUI selectableUI;

    private void Start()
    {
        selectableUI = GetComponent<SelectableUI>();
    }
}
