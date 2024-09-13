using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// パーティクルのスピードを変えるクラス
/// </summary>
public class ParticleSpeed : MonoBehaviour
{

    //速度を変える対象のパーティクル
    private List<ParticleSystem> _particleSystems;

    private ParticleSystem particle;

    //パーティクルの速度
    [SerializeField]
    private float _spped = 0.4f;


    private void Start()
    {
        particle = this.GetComponent<ParticleSystem>();

        // 停止
        particle.Stop();

        //子にある全てのパーティクルを取得
        _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();
    }


    //Inspectorで値(_speed)を変更した時に呼ばれる
    /*    private void OnValidate()
        {
            //実行中以外はスルー
            if (!Application.isPlaying)
            {
                return;
            }

            //全パーティクルの速度を変更
            foreach (var particle in _particleSystems)
            {
                float _particle = 1  * _spped;
                ChangeSpeed( _particle);
            }
        }*/

    //指定したパーティクルの速度を変更
    public void ChangeSpeed(float speed)
    {
        var beameffect = particle.main;
        beameffect.simulationSpeed = speed;

        particle.Play();
    }


}