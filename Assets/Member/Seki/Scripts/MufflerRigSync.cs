using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MufflerRigSync : MonoBehaviour
{

    [SerializeField, Header("�����Ώ�")] private GameObject SyncObj;

    private Transform SyncObjTrans;


    private Transform ThisObjTrans;


    void Start()
    {
        if (SyncObj == null)
        {
            Debug.LogError("�����Ώۂ��I������Ă܂���");
        }
        SyncObjTrans=SyncObj.GetComponent<Transform>();
        ThisObjTrans = this.GetComponent<Transform>();
    }


    void Update()
    {
        ThisObjTrans.position = new Vector3(SyncObjTrans.position.x, SyncObjTrans.position.y, SyncObjTrans.position.z);
        ThisObjTrans.rotation = Quaternion.Euler(SyncObjTrans.rotation.x, SyncObjTrans.rotation.y, SyncObjTrans.rotation.z);
    }
}
