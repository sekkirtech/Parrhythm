using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainGameManager : MonoBehaviour
{
    //�v���C���[��HP
    [SerializeField]public int PlayerHp = 3;
    //�G��HP
    [SerializeField] public int EnemyMaxHP = 3;
    public int EnemyHP = 0;
    //�v���C���[�i�[
    [SerializeField]PlayerManager PlayerObj;
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
    //�K�[�h���t���O
    public bool Girdnow = false;
    private Image myimage;
    //PlayerHP�摜�i�[
    [SerializeField] GameObject[] HpSprite;
    //HP�_���[�W�摜�i�[
    [SerializeField] Sprite DamageHp;
    //�p���B��t�t���O
    public bool ParryReception = false;
    //�p���B�������p�A�ő΍�
    public bool ParryHits = false;
    //�p���B�\�t���O
    public bool ParryAttack = false;

    //�C���X�^���X
    public static MainGameManager Instance;


    void Awake()
    {
        Instance = this;
        //������
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
        Girdnow = false;
        ParryReception = false; 
        ParryHits = false;

        SpriteList[1].gameObject.SetActive(false);
        SpriteList[2].gameObject.SetActive(false);
        SpriteList[3].gameObject.SetActive(false);
    }

    void Update()
    {
        //�^�C���v���A
        if (GameStart) BattleTime += Time.deltaTime;
    }

    //���s���������Ƃ��ɌĂяo��
    public void toResult(int EnemyHP,int EnemyMaxHP)
    {
        GameStart = false;
        //�G�cHP
        PlayerPrefs.SetInt("StageNum", EnemyHP);
        //�G�ő�HP
        PlayerPrefs.SetInt("MaxHP", EnemyMaxHP);
        //�퓬����
        PlayerPrefs.SetFloat("Time", BattleTime);
        //�G�U����
        PlayerPrefs.SetInt("EnemyAttackCount", AttackCount);
        //�p���B������
        PlayerPrefs.SetInt("ParryCount", ParryCount);


        FadeManager.Instance.LoadScene("ResultScene", 1.0f);
    }


    /// <summary>
    /// �G�̍U����������Ƃ��ɓG�I�u�W�F�N�g����Ăяo��
    /// �K�[�h�����ĂȂ�������_���[�W
    /// �p���B��t���ԓ��Ȃ�0.25�b�����p���B�\�ɂ���
    /// </summary>
    public IEnumerator EnemmyAttack()
    {
        AttackCount++;
        if (!Girdnow)
        {
            Debug.Log("�_���[�W���󂯂��I");
            PlayerHp--;
            //HP�摜�����ւ�
            myimage = HpSprite[PlayerHp].GetComponent<Image>();
            myimage.sprite = DamageHp;
            yield break;
        }
        if (ParryReception)
        {
            var gpad = Gamepad.current;
            Debug.Log("�p���B�\�I");
            //�A�Ŗh�~�p�t���O
            ParryHits = true;
            //�p���B�\��
            ParryAttack = true;
            SpriteList[2].gameObject.SetActive(true);
            //�\�ɂȂ�����p�b�h�U��
            gpad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(0.15f);
            gpad.SetMotorSpeeds(0.0f, 0.0f);
            yield return new WaitForSeconds(0.35f);
            ParryAttack = false;
            Debug.Log("�p���C�I��");
            SpriteList[2].gameObject.SetActive(false);
        }
    }
}
