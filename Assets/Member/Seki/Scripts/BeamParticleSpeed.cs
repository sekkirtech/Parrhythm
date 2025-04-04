using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamParticleSpeed : MonoBehaviour
{
    [SerializeField, Header("��")] ParticleSystem EyeParticle;
    [SerializeField, Header("���r�[��")] ParticleSystem WhiteBeam;
    [SerializeField, Header("�ԃr�[��")] ParticleSystem RedBeam;
    [SerializeField, Header("�e")] ParticleSystem ParentObj;



    void Awake()
    {
        //�J�ڎ��̌딚�h�~
        ParentObj.Stop();
    }

    /// <summary>
    /// �r�[���̍Đ�
    /// </summary>
    /// <param name="multiplication">�Đ��X�s�[�h�{��</param>
    public void SpeedChange(float multiplication)
    {
        //Debug.Log("���Z�с[��");
        //�p�[�e�B�N�������A�^�b�`
        ParticleSystem.MainModule eye = EyeParticle.main;
        ParticleSystem.MainModule white = WhiteBeam.main;
        ParticleSystem.MainModule red = RedBeam.main;
        //�X�s�[�h�ݒ�
        eye.simulationSpeed = multiplication;
        white.simulationSpeed = multiplication;
        red.simulationSpeed = multiplication;

        //�Đ�
        ParentObj.Play();
    }
}
