using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// パーティクルのスピードを変えるクラス
/// </summary>
public class ParticleSpeed : MonoBehaviour
{

    //速度を変える対象のパーティクル
    private List<ParticleSystem> _particleSystems;

    //パーティクルの速度
    [SerializeField]
    private float _spped = 0.4f;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Start()
    {
        //子にある全てのパーティクルを取得
        _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    //=================================================================================
    //イベント
    //=================================================================================

    //Inspectorで値(_speed)を変更した時に呼ばれる
    private void OnValidate()
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
            ChangeSpeed(particle, _particle);
        }
    }

    //=================================================================================
    //変更
    //=================================================================================

    //指定したパーティクルの速度を変更
    private void ChangeSpeed(ParticleSystem particle, float speed)
    {
        var beameffect = particle.main;
        beameffect.simulationSpeed = speed;
    }

}