using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] MainGameManager maingamemanager;
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource1;


    //int hakucount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        maingamemanager.SpriteList[0].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)&&maingamemanager.BeatFlag)
        {
            Debug.Log("1���U��");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && maingamemanager.BeatFlag)
        {
            Debug.Log("2���U��");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && maingamemanager.BeatFlag)
        {
            Debug.Log("3���U��");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(3));
        }
    }
}
