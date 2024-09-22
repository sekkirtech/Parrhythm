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
        //��~
        ParentObj.Stop();
    }

    public void SpeedChange(float multiplication)
    {
        Debug.Log("�ł���[");
        ParticleSystem.MainModule eye = EyeParticle.main;
        ParticleSystem.MainModule white = WhiteBeam.main;
        ParticleSystem.MainModule red = RedBeam.main;
        eye.simulationSpeed = multiplication;
        white.simulationSpeed = multiplication;
        red.simulationSpeed = multiplication;

        ParentObj.Play();
    }
}
