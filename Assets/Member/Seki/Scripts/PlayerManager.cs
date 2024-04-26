using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //�v���C���[��HP
    int PlayerHp = 3;
    //�G�I�u�W�F�N�g
    [SerializeField] EnemyManager EnemyObj;
    //���i�[�p�}�l�[�W���[
    [SerializeField] MainGameManager MainGameObj;
    //�K�[�h���t���O
    bool Girdnow=false;
    //�p���B��t�t���O
    bool ParryReception = false;
    //�p���B�\�t���O
    bool ParryAttack=false;
    //���������p�t���O
    bool LongPushnow=false;
    //�K�[�h���Ԍv���p
    float GirdTime = 0.0f;
    //�p���B�������p�A�ő΍�
    bool ParryHits = false;


    
    void Start()
    {
        //������
        Girdnow = false;
        ParryReception = false;
        LongPushnow = false;
        GirdTime = 0.0f;

        //null�`�F�b�N
        if (EnemyObj == null)
        {
            Debug.Log("Enemy���Ȃ��̂ŃA�^�b�`���܂�");
            GameObject enemyseki = GameObject.Find("Player");
            EnemyObj = enemyseki.GetComponent<EnemyManager>();
        }
    }

    void Update()
    {
        //�K�[�h��
        if(Input.GetKey(KeyCode.Space))
        {
            Girdnow=true;
            //�^�C���v��
            GirdTime += Time.deltaTime;
            //0.25�b�ȓ��Ńp���B�A��������s��
            if (GirdTime > 0.25)
            {
                ParryReception = false;
            }
            else
            {
                ParryReception=true;
            }
        }
        else
        {
            Girdnow = false;
            //�����ĂȂ���Ώ�����
            GirdTime=0.0f;
        }

        //�p���B�\���ԓ���P(��)�Ńp���B����
        if(Input.GetKeyDown(KeyCode.P)&&ParryAttack&&ParryHits)
        {
            ParryHits = false;
            Debug.Log("�p���B����");
        }
        //HP��0�Ń��U���g��
        if (PlayerHp == 0)
        {
            MainGameObj.EnemyWin();
        }

    }
    /// <summary>
    /// �G�̍U����������Ƃ��ɓG�I�u�W�F�N�g����Ăяo��
    /// �K�[�h�����ĂȂ�������_���[�W
    /// �p���B��t���ԓ��Ȃ�0.25�b�����p���B�\�ɂ���
    /// </summary>
     public IEnumerator EnemmyAttack()
    {
        if (!Girdnow)
        {
            PlayerHp--;
            yield break;
        }
        if (ParryReception)
        {
            ParryHits=true;
            ParryAttack = true;
            yield return new WaitForSeconds(0.25f);
            ParryAttack=false;
        }
    }
}
