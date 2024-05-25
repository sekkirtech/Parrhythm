using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //�v���C���[�i�[
    [SerializeField] PlayerManager PlayerObj;
    //�G��HP
    [SerializeField]public int EnemyMaxHP = 3;
    public int EnemyHP = 0;
    //�v���t�@�u
    [SerializeField] GameObject[] AttackObj;
    //���i�[�p�}�l�[�W���[
    [SerializeField] MainGameManager MainGameObj;
    //BGM�pSource
    //[SerializeField] AudioClip[] BGMClip;
    //��������Ă邩
    bool EnemySlain=false;
    //���ŗp
    private TextMeshProUGUI HPtext;
    //HP�o�[script�i�[
    HpBar Bar;

    void Start()
    {
        //������
        EnemySlain=false;
        //HP�\��
        Bar=GameObject.Find("Slider").GetComponent<HpBar>();
        Bar.Init(EnemyMaxHP);
        EnemyHP = EnemyMaxHP;

        //null�`�F�b�N
        if (PlayerObj == null)
        {
            Debug.Log("Player�̃X�N���v�g���Ȃ�����A�^�b�`���܂�");
            GameObject playerseki = GameObject.Find("Player");
            PlayerObj = playerseki.GetComponent<PlayerManager>();
        }
        for (int i = 0; i < 3; i++)
        {
            //�v���n�u�������m��̂��߃G���[�ŕ\�L
            //if (AttackObj[i] == null) Debug.LogError("�U���I�u�W�F�N�g��" + i + "�Ԃ�����܂���");
        }
    }

    void Update()
    {
        //HP���O�ȉ��Ń��U���g�i�}�C�i�X�s�������Ȃ̂Łj
        if (EnemyHP <= 0&&!EnemySlain)
        {
            EnemySlain = true;
            PlayerPrefs.SetInt("IsWin", 1);
            MainGameObj.toResult(EnemyHP,EnemyMaxHP);
        }
    }

    public void EnemyDamage()
    {
        //�A�j���[�V�����}��
        EnemyHP--;
        Bar.SetHp(1);
    }
}
