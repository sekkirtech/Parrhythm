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

    //private float time = 0.0f;
    private bool x = true;

    //int hakucount = 0;
    int MAXCount = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        maingamemanager.SpriteList[0].gameObject.SetActive(false);
        x =true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)&&maingamemanager.BeatFlag)
        {
            Debug.Log("1îèçUåÇ");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && maingamemanager.BeatFlag)
        {
            Debug.Log("2îèçUåÇ");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && maingamemanager.BeatFlag)
        {
            Debug.Log("3îèçUåÇ");
            maingamemanager.BeatFlag = false;
            StartCoroutine(maingamemanager.EnemmyAttack(3));
        }
    }
}
