using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //�v���C���[�i�[
    [SerializeField] PlayerManager playerId;
    //�G��HP
    [SerializeField]int EnemyHP = 3;
    //��
    int Beat = 0;
    //�G���U�������񐔁i�p���B���\�L�p�j
    int AttackCount = 0;
    //BGM�pSource
    [SerializeField] AudioClip[] BGMClip;
    //

    void Start()
    {
        if(playerId == null)
        {
            Debug.Log("Player�̃X�N���v�g���Ȃ�����A�^�b�`");
        }
    }

    void Update()
    {
        
    }
}
