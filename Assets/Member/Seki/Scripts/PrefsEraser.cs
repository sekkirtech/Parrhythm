using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsEraser : MonoBehaviour
{

    //PlayerPrefs�̏�����
    //�����L���O�����̏ꍇ��z�肵All�͎g��Ȃ�

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
