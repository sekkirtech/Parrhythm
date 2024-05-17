using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //���ŗp
    private TextMeshProUGUI HPtext;
    private float HanteiTime = 0.0f;


    
    void Start()
    {
        //������
        Girdnow = false;
        ParryReception = false;
        LongPushnow = false;
        GirdTime = 0.0f;
        MainGameObj.SpriteList[1].gameObject.SetActive(false);
        MainGameObj.SpriteList[2].gameObject.SetActive(false);
        MainGameObj.SpriteList[3].gameObject.SetActive(false);
        //HP�\���i�����̂ŗv���P�j
        GameObject child = MainGameObj.SpriteList[4];
        child = child.transform.GetChild(0).gameObject;
        HPtext = child.GetComponent<TextMeshProUGUI>();

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

        //�f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("�K�[�h");
        //�K�[�h��
        if (Input.GetKey(KeyCode.Space))
        {
            MainGameObj.SpriteList[1].gameObject.SetActive(true);
            Girdnow=true;
            //�^�C���v��
            GirdTime += Time.deltaTime;
            //0.25�b�ȓ��Ńp���B�A��������s��
            if (GirdTime > 0.5)
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
            MainGameObj.SpriteList[1].gameObject.SetActive(false);
            //�����ĂȂ���Ώ�����
            GirdTime =0.0f;
        }

        //�p���B�\���ԓ���P(��)�Ńp���B����
        if(Input.GetKeyDown(KeyCode.P)&&ParryAttack&&ParryHits)
        {
            ParryHits = false;
            MainGameObj.SpriteList[3].gameObject.SetActive(true);
            HanteiTime = 0.0f;
            EnemyObj.EnemyDamage();
            Debug.Log("�p���B����");
        }

        if (MainGameObj.SpriteList[3].gameObject.activeSelf)
        {
            HanteiTime += Time.deltaTime;
            if(HanteiTime>1.0f) MainGameObj.SpriteList[3].gameObject.SetActive(false);
        }

        //HP�X�V
        if(HPtext != null)
        {
            HPtext.text = "PlayerHP:" + PlayerHp;
        }
        else
        {
            Debug.Log("�v���C���[�e�L�X�g�G���[");
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
            Debug.Log("�_���[�W���󂯂��I");
            PlayerHp--;
            yield break;
        }
        if (ParryReception)
        {
            Debug.Log("�p���B�\�I");
            ParryHits=true;
            ParryAttack = true;
            MainGameObj.SpriteList[2].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ParryAttack=false;
            Debug.Log("�p���C�I��");
            MainGameObj.SpriteList[2].gameObject.SetActive(false);
        }
    }
}
