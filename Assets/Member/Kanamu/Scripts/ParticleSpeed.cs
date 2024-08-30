using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �p�[�e�B�N���̃X�s�[�h��ς���N���X
/// </summary>
public class ParticleSpeed : MonoBehaviour
{

    //���x��ς���Ώۂ̃p�[�e�B�N��
    private List<ParticleSystem> _particleSystems;

    //�p�[�e�B�N���̑��x
    [SerializeField]
    private float _spped = 1;

    //=================================================================================
    //������
    //=================================================================================

    private void Start()
    {
        //�q�ɂ���S�Ẵp�[�e�B�N�����擾
        _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    //=================================================================================
    //�C�x���g
    //=================================================================================

    //Inspector�Œl(_speed)��ύX�������ɌĂ΂��
    private void OnValidate()
    {
        //���s���ȊO�̓X���[
        if (!Application.isPlaying)
        {
            return;
        }

        //�S�p�[�e�B�N���̑��x��ύX
        foreach (var particle in _particleSystems)
        {
            ChangeSpeed(particle, _spped);
        }
    }

    //=================================================================================
    //�ύX
    //=================================================================================

    //�w�肵���p�[�e�B�N���̑��x��ύX
    private void ChangeSpeed(ParticleSystem particle, float speed)
    {
        var main = particle.main;
        main.simulationSpeed = speed;
    }

}