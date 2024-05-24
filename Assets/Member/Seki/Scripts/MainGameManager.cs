using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGameManager : MonoBehaviour
{
    //�v���C���[�i�[
    [SerializeField] PlayerManager PlayerObj;
    //�G�i�[
    [SerializeField] EnemyManager EnemyObj;
    //�o�ߎ��ԑ���
    float BattleTime = 0.0f;
    //�Q�[�����J�n���Ă邩
    bool GameStart=false;
    //�����o�pobj
    [SerializeField] public GameObject[] SpriteList;
    //�G���U�������񐔁i�p���B���\�L�p�j
    public int AttackCount = 0;
    //�p���B������
    public int ParryCount = 0;


    void Start()
    {
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
    }

    void Update()
    {
        //�^�C���v���A
        if (GameStart) BattleTime += Time.deltaTime;
    }

    //�v���C���[��HP��0�ɂȂ�����Ăяo��
    public void EnemyWin(int EnemyHP,int EnemyMaxHP)
    {
        Debug.Log("����");
        Debug.Log(EnemyHP);
        Debug.Log(EnemyMaxHP);
        GameStart = false;
        /*
        �GHP�̎c�聓
        �퓬���Ԃ��i�[
        ���U���g�G�l�~�[�o�[�W�����֑J��
         */
        //�G�cHP
        PlayerPrefs.SetInt("StageNum",EnemyHP);
        //�G�ő�HP
        PlayerPrefs.SetInt("MaxHP", EnemyMaxHP);
        //�퓬����
        PlayerPrefs.SetFloat("Time",BattleTime);
        //���s
        PlayerPrefs.SetInt("IsWin", 0);
        //�G�U����
        PlayerPrefs.SetInt("EnemyAttackCount",AttackCount);
        //�p���B������
        PlayerPrefs.SetInt("ParryCount", ParryCount);

        //�J��
        FadeManager.Instance.LoadScene("ResultScene", 1.0f);
    }

    //�G��HP���O�ɂȂ����Ƃ��ɌĂяo��
    public void PlayerWin(int EnemyHP,int EnemyMaxHP)
    {
        Debug.Log("����");
        Debug.Log(EnemyHP);
        Debug.Log(EnemyMaxHP);
        GameStart = false;
        //�G�cHP
        PlayerPrefs.SetInt("StageNum", EnemyHP);
        //�G�ő�HP
        PlayerPrefs.SetInt("MaxHP", EnemyMaxHP);
        //�퓬����
        PlayerPrefs.SetFloat("Time", BattleTime);
        //���s
        PlayerPrefs.SetInt("IsWin", 0);
        //�G�U����
        PlayerPrefs.SetInt("EnemyAttackCount", AttackCount);
        //�p���B������
        PlayerPrefs.SetInt("ParryCount", ParryCount);


        FadeManager.Instance.LoadScene("ResultScene", 1.0f);
    }



}
