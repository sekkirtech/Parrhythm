using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTmp : MonoBehaviour
{
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource1;


    //�q�b�g���锏���A�������̔�+�U���܂ł̔���

    //�A�N�e�B�u���A�j���[�V����������

    //�A�j���[�V�������o�pAudio�ݒ�

    private float time = 0.0f;

    int hakucount = 0;
    int MAXCount = 3;
    bool b=true;
    //�ȉ��e�X�g�p�@120BPM
    private void Start()
    {
       // StartCoroutine(Hakuco());
    }

    IEnumerator Hakuco()
    {
        AudioSource1.clip=haku;
        for(int i = 0; i < MAXCount+1; i++)
        {
            yield return new WaitForSeconds(0.5f);
            AudioSource1.Play();
            Debug.Log(i);
        }
        AudioSource1.clip=hakufin; 
        AudioSource1.Play();
    }


}
