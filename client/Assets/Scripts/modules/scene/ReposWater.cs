﻿using UnityEngine;
using System.Collections;

public class ReposWater : MonoBehaviour
{
    Transform mTrans;
    Transform mCamTrans;

    void Start()
    {
        mTrans = transform;
//        mCamTrans = SceneMgr.Instance.mainCamera.transform;
        //mCamTrans = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (mCamTrans != null)
        {
            Vector3 pos = mCamTrans.position;
            pos.y = 0.0f;

            if (mTrans.position != pos)
            {
                mTrans.rotation = Quaternion.identity;
                mTrans.position = pos;
            }
        }
    }
}