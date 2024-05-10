using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTmp : MonoBehaviour
{
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource;


    //ヒットする拍数、生成時の拍+攻撃までの拍数

    //アクティブ時アニメーション初期化

    //アニメーション演出用Audio設定

    private float time = 0.0f;

    //以下テスト用　120BPM
    private void Update()
    {
        time += Time.deltaTime;
        if(time < 0.5f)
        {

        }
    }


}
