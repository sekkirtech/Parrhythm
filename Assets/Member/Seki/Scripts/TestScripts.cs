using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�̍U�����L�[����Ŕ���������f�o�b�O�p�X�N���v�g
/// </summary>
public class TestScripts : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] MainGameManager maingamemanager;
    
    // Start is called before the first frame update
    void Start()
    {
        //maingamemanager.SpriteList[0].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)&&maingamemanager.BeatFlag)
        {
            StartCoroutine(maingamemanager.EnemmyAttack(1,1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && maingamemanager.BeatFlag)
        {
            StartCoroutine(maingamemanager.EnemmyAttack(2,1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && maingamemanager.BeatFlag)
        {
            StartCoroutine(maingamemanager.EnemmyAttack(3,1));
        }
    }
}
