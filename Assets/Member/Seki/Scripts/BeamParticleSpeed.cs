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
        //遷移時の誤爆防止
        ParentObj.Stop();
    }

    /// <summary>
    /// ビームの再生
    /// </summary>
    /// <param name="multiplication">再生スピード倍率</param>
    public void SpeedChange(float multiplication)
    {
        //Debug.Log("う〇びーむ");
        //パーティクル分割アタッチ
        ParticleSystem.MainModule eye = EyeParticle.main;
        ParticleSystem.MainModule white = WhiteBeam.main;
        ParticleSystem.MainModule red = RedBeam.main;
        //スピード設定
        eye.simulationSpeed = multiplication;
        white.simulationSpeed = multiplication;
        red.simulationSpeed = multiplication;

        //再生
        ParentObj.Play();
    }
}
