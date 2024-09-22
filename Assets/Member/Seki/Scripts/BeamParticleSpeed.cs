using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamParticleSpeed : MonoBehaviour
{
    [SerializeField, Header("目")] ParticleSystem EyeParticle;
    [SerializeField, Header("白ビーム")] ParticleSystem WhiteBeam;
    [SerializeField, Header("赤ビーム")] ParticleSystem RedBeam;
    [SerializeField, Header("親")] ParticleSystem ParentObj;



    void Awake()
    {
        //停止
        ParentObj.Stop();
    }

    public void SpeedChange(float multiplication)
    {
        Debug.Log("でたよー");
        ParticleSystem.MainModule eye = EyeParticle.main;
        ParticleSystem.MainModule white = WhiteBeam.main;
        ParticleSystem.MainModule red = RedBeam.main;
        eye.simulationSpeed = multiplication;
        white.simulationSpeed = multiplication;
        red.simulationSpeed = multiplication;

        ParentObj.Play();
    }
}
