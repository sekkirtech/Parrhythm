using UnityEngine;
using System.Collections;
using UnityEngine.UI; // UI�R���|�[�l���g�̎g�p

public class ButtonMenu : MonoBehaviour
{
    Button StageSelect;
    Button License;

    void Start()
    {
        // �{�^���R���|�[�l���g�̎擾
        StageSelect = GameObject.Find("/Canvas/Button").GetComponent<Button>();
        License = GameObject.Find("/Canvas/Button1").GetComponent<Button>();

        // �ŏ��ɑI����Ԃɂ������{�^���̐ݒ�
        StageSelect.Select();
    }
}