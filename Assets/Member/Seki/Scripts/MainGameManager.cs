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

    void Start()
    {
        BattleTime = 0.0f;
    }

    void Update()
    {
        //�^�C���v���A
        if (GameStart) BattleTime += Time.deltaTime;
    }

    //�v���C���[��HP��0�ɂȂ�����Ăяo��
    public void EnemyWin()
    {
        GameStart = false;
        /*
         �X�e�[�W�Z���N�g�p�Ɏc�����j��s�\obj��
        �GHP�̎c�聓
        �퓬���Ԃ��i�[
        ���U���g�G�l�~�[�o�[�W�����֑J��
         */
    }

    //�G��HP���O�ɂȂ����Ƃ��ɌĂяo��
    public void PlayerWin()
    {
        GameStart = false;
        /*
         �X�e�[�W�Z���N�g�p�Ɏc�����j��s�\obj��
        �퓬���ԃp���B�̐��������i�[
        ���U���g�v���C���[�o�[�W�����֑J��
         */
    }



}
