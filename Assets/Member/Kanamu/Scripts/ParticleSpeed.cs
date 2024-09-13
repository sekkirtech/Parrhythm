using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// �p�[�e�B�N���̃X�s�[�h��ς���N���X
/// </summary>
public class ParticleSpeed : MonoBehaviour
{

    //���x��ς���Ώۂ̃p�[�e�B�N��
    private List<ParticleSystem> _particleSystems;

    private ParticleSystem particle;

    //�p�[�e�B�N���̑��x
    [SerializeField]
    private float _spped = 0.4f;


    private void Start()
    {
        particle = this.GetComponent<ParticleSystem>();

        // ��~
        particle.Stop();

        //�q�ɂ���S�Ẵp�[�e�B�N�����擾
        _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();
    }


    //Inspector�Œl(_speed)��ύX�������ɌĂ΂��
<<<<<<< HEAD
    private void OnValidate()
=======
/*    private void OnValidate()
>>>>>>> main
    {
        //���s���ȊO�̓X���[
        if (!Application.isPlaying)
        {
            return;
        }

        //�S�p�[�e�B�N���̑��x��ύX
        foreach (var particle in _particleSystems)
        {
            float _particle = 1  * _spped;
<<<<<<< HEAD
            ChangeSpeed(particle, _particle);
        }
    }

    //�w�肵���p�[�e�B�N���̑��x��ύX
    private void ChangeSpeed(ParticleSystem particle, float speed)
=======
            ChangeSpeed( _particle);
        }
    }*/

    //�w�肵���p�[�e�B�N���̑��x��ύX
    public void ChangeSpeed(float speed)
>>>>>>> main
    {
        var beameffect = particle.main;
        beameffect.simulationSpeed = speed;

        particle.Play();
    }
    

}