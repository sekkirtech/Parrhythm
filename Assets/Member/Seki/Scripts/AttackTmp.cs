using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTmp : MonoBehaviour
{
    [SerializeField] AudioClip haku;
    [SerializeField] AudioClip hakufin;
    [SerializeField] AudioSource AudioSource1;


    //ヒットする拍数、生成時の拍+攻撃までの拍数

    //アクティブ時アニメーション初期化

    //アニメーション演出用Audio設定

    //private float time = 0.0f;

    //int hakucount = 0;
    int MAXCount = 3;
    //bool b=true;
    //以下テスト用　120BPM
    private void Start()
    {
       StartCoroutine(TempoCol());
    }

    IEnumerator TempoCol()
    {
        AudioSource1.clip = haku;
        for (int i = 0; i < MAXCount; i++)
        {
            AudioSource1.Play();
            Debug.Log(i);
            if (i == 2)
            {
               MainGameManager.Instance.SpriteList[0].SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(MainGameManager.Instance.EnemmyAttack());
        AudioSource1.clip = hakufin;
        AudioSource1.Play();
        //maingamemanager.SpriteList[0].SetActive(false);
    }
    private void Update()
    {
        
    }


}
