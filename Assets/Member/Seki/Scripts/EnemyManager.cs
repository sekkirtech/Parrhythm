using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //�v���C���[�i�[
    [SerializeField] PlayerManager PlayerId;
    //�G��HP
    [SerializeField]int EnemyHP = 3;
    //�v���t�@�u
    [SerializeField] GameObject[] AttackObj;
    //���i�[�p�}�l�[�W���[
    [SerializeField] MainGameManager MainGameObj;
    //�G���U�������񐔁i�p���B���\�L�p�j
    int AttackCount = 0;
    //BGM�pSource
    //[SerializeField] AudioClip[] BGMClip;
    //��������Ă邩
    bool EnemySlain=false;

    void Start()
    {
        //null�`�F�b�N
        if(PlayerId == null)
        {
            Debug.Log("Player�̃X�N���v�g���Ȃ�����A�^�b�`");
            GameObject playerseki = GameObject.Find("Enemy");
            PlayerId = playerseki.GetComponent<PlayerManager>();
        }
        for (int i = 0; i < AttackObj.Length; i++)
        {
            if (AttackObj[i] == null) Debug.LogError(i + "�Ԃ����݂��܂���");
        }
    }

    void Update()
    {
        
    }
}
