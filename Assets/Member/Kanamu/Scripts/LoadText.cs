using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadText : MonoBehaviour
{
    public TextAsset file;
    public TextMeshProUGUI text;

    void Start()
    {
        // �e�L�X�g�t�@�C���̑S�����擾
        string loadText = file.text;

        text.text = loadText;
    }
}
