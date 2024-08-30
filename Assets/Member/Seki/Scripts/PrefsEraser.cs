using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsEraser : MonoBehaviour
{

    //PlayerPrefsの初期化
    //ランキング実装の場合を想定しAllは使わない

    void Start()
    {
        PlayerPrefs.DeleteKey("StageNum");
        PlayerPrefs.DeleteKey("CurrentHP");
        PlayerPrefs.DeleteKey("MaxHP");
        PlayerPrefs.DeleteKey("Time");
        PlayerPrefs.DeleteKey("EnemyAttackCount");
        PlayerPrefs.DeleteKey("ParryCount");
    }
}
