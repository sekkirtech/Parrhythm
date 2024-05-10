using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("çUåÇ");
            StartCoroutine(playerManager.EnemmyAttack());
        }
    }
}
