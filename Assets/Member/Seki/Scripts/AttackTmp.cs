using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTmp : MonoBehaviour
{
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource;


    //�q�b�g���锏���A�������̔�+�U���܂ł̔���

    //�A�N�e�B�u���A�j���[�V����������

    //�A�j���[�V�������o�pAudio�ݒ�

    private float time = 0.0f;

    //�ȉ��e�X�g�p�@120BPM
    private void Update()
    {
        time += Time.deltaTime;
        if(time < 0.5f)
        {

        }
    }


}
