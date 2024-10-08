using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MufflerRigSync : MonoBehaviour
{

    [SerializeField, Header("同期対象")] private GameObject SyncObj;

    private Transform SyncObjTrans;


    private Transform ThisObjTrans;


    void Start()
    {
        if (SyncObj == null)
        {
            Debug.LogError("同期対象が選択されてません");
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
