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
    //���i�[�p�}�l�[�W���[
    [SerializeField] MainGameManager MainGameObj;
    //�A�Ŗh�~�t���O
    bool playerlose=false;
    //�K�[�h���Ԍv���p
    float GirdTime = 0.0f;
    //���ŗp
    private float HanteiTime = 0.0f;
    //�R���g���[���[�pbool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;

    [SerializeField] AudioClip Gird;
    [SerializeField] AudioClip Parrywin;
    private AudioSource AudioSource;

    private CompositeDisposable disposables_=new CompositeDisposable();


    void Start()
    {
        //������
        GirdTime = 0.0f;
        MainGameObj.SpriteList[1].gameObject.SetActive(false);
        MainGameObj.SpriteList[2].gameObject.SetActive(false);
        MainGameObj.SpriteList[3].gameObject.SetActive(false);
        playerlose = false;
        //HP�\���i�����̂ŗv���P�j
        GameObject child = MainGameObj.SpriteList[4];
        child = child.transform.GetChild(0).gameObject;

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
        AudioSource=gameObject.AddComponent<AudioSource>();
    }


        void Update()
        {
            //�K�[�h��
            if (Input.GetKey(KeyCode.Space)||GirdButton)
            {
                Debug.Log("�K�[�h");
                MainGameObj.SpriteList[1].gameObject.SetActive(true);
                MainGameObj.Girdnow = true;
                //�^�C���v��
                GirdTime += Time.deltaTime;
                //0.25�b�ȓ��Ńp���B�A��������s��
                if (GirdTime > 0.5)
                {
                    MainGameObj.ParryReception = false;
                }
                else
                {
                    MainGameObj.ParryReception = true;
                }
            }
            else
            {
                MainGameObj.Girdnow = false;
                MainGameObj.SpriteList[1].gameObject.SetActive(false);
                //�����ĂȂ���Ώ�����
                GirdTime = 0.0f;
            }

            //�p���B�\���ԓ���P(��)�Ńp���B����
            if ( Input.GetKeyDown(KeyCode.P) || ParryAttackButton)
            {
            Debug.Log("ParryAttack"+MainGameObj.ParryAttack);
            Debug.Log("ParryHits"+MainGameObj.ParryHits);
            if (MainGameObj.ParryAttack && MainGameObj.ParryHits)
            {
                MainGameObj.ParryHits = false;
                MainGameObj.SpriteList[3].gameObject.SetActive(true);
                HanteiTime = 0.0f;
                EnemyObj.EnemyDamage();
                MainGameObj.ParryCount++;
                Debug.Log("�p���B����");
                AudioSource.clip = Parrywin;
                AudioSource.Play();
            }
            }

            //�p���B������ʂ̍폜
            if (MainGameObj.SpriteList[3].gameObject.activeSelf)
            {
                HanteiTime += Time.deltaTime;
                if (HanteiTime > 1.0f) MainGameObj.SpriteList[3].gameObject.SetActive(false);
            }


            //HP��0�Ń��U���g��
            if (MainGameObj.PlayerHp <= 0&&!playerlose)
            {
            playerlose = true;
            PlayerPrefs.SetInt("IsWin", 0);
                MainGameObj.toResult(EnemyObj.EnemyHP,EnemyObj.EnemyMaxHP);
            }
        }

    //Subscribe�폜
    private void OnDestroy()
    {
        disposables_.Dispose();
    }
}
