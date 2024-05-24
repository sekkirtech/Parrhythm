using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //�G�I�u�W�F�N�g
    [SerializeField] EnemyManager EnemyObj;
    //�A�Ŗh�~�t���O
    bool playerlose=false;
    //�K�[�h���Ԍv���p
    float GirdTime = 0.0f;
    //���ŗp
    private float HanteiTime = 0.0f;
    //�R���g���[���[�pbool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;


    private CompositeDisposable disposables_=new CompositeDisposable();


    void Start()
    {
        //������
        GirdTime = 0.0f;
        playerlose = false;

        //null�`�F�b�N
        if (EnemyObj == null)
        {
            Debug.Log("Enemy���Ȃ��̂ŃA�^�b�`���܂�");
            GameObject enemyseki = GameObject.Find("Player");
            EnemyObj = enemyseki.GetComponent<EnemyManager>();
        }
        //�R���g���[���[�����o�^
        ControllerManager.Instance.L2ButtonObservable.Subscribe(x => GirdButton = true).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonObservable.Subscribe(x => GirdButton = true).AddTo(disposables_);
        ControllerManager.Instance.L2ButtonUpObservable.Subscribe(x => GirdButton = false).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonUpObservable.Subscribe(x => GirdButton = false).AddTo(disposables_);
        ControllerManager.Instance.WestButtonObservable.Subscribe(x => ParryAttackButton = true).AddTo(disposables_);
        ControllerManager.Instance.WestButtonUpObservable.Subscribe(x=>ParryAttackButton = false).AddTo(disposables_);
    }


        void Update()
        {
            //�K�[�h��
            if (Input.GetKey(KeyCode.Space)||GirdButton)
            {
                Debug.Log("�K�[�h");
                MainGameManager.Instance.SpriteList[1].gameObject.SetActive(true);
                MainGameManager.Instance.Girdnow = true;
                //�^�C���v��
                GirdTime += Time.deltaTime;
                //0.25�b�ȓ��Ńp���B�A��������s��
                if (GirdTime > 0.5)
                {
                    MainGameManager.Instance.ParryReception = false;
                }
                else
                {
                    MainGameManager.Instance.ParryReception = true;
                }
            }
            else
            {
                MainGameManager.Instance.Girdnow = false;
                MainGameManager.Instance.SpriteList[1].gameObject.SetActive(false);
                //�����ĂȂ���Ώ�����
                GirdTime = 0.0f;
            }

            //�p���B�\���ԓ���P(��)�Ńp���B����
            if (Input.GetKeyDown(KeyCode.P)||ParryAttackButton && MainGameManager.Instance.ParryAttack && MainGameManager.Instance.ParryHits)
            {
                MainGameManager.Instance.ParryHits = false;
                MainGameManager.Instance.SpriteList[3].gameObject.SetActive(true);
                HanteiTime = 0.0f;
                EnemyObj.EnemyDamage();
                MainGameManager.Instance.ParryCount++;
                Debug.Log("�p���B����");
            }

            //�p���B������ʂ̍폜
            if (MainGameManager.Instance.SpriteList[3].gameObject.activeSelf)
            {
                HanteiTime += Time.deltaTime;
                if (HanteiTime > 1.0f) MainGameManager.Instance.SpriteList[3].gameObject.SetActive(false);
            }


            //HP��0�Ń��U���g��
            if (MainGameManager.Instance.PlayerHp <= 0&&!playerlose)
            {
            playerlose = true;
            PlayerPrefs.SetInt("IsWin", 0);
                MainGameManager.Instance.toResult(MainGameManager.Instance.EnemyHP,MainGameManager.Instance.EnemyMaxHP);
            }
        }

    //Subscribe�폜
    private void OnDestroy()
    {
        disposables_.Dispose();
    }
}
