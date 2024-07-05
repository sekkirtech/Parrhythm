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
        // テキストファイルの全文を取得
        string loadText = file.text;

        text.text = loadText;
    }
}
