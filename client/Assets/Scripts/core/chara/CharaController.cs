﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class CharaController : Singleton<CharaController>, ITick
{
    /// <summary>
    /// Raycast points used to determine if the ship has hit shallow water.
    /// </summary>

    //public Transform[] raycastPoints;
    public List<Transform> raycastPoints;

    /// <summary>
    /// Mask to use when raycasting.
    /// </summary>

    public LayerMask raycastMask;

    // Left/right, acceleration
    Vector2 mInput = Vector2.zero;
    Vector2 mSensitivity = new Vector2(6f, 1f);

    float mSpeed = 0f;
    float mSteering = 0f;
    float mTargetSpeed = 0f;
    float mTargetSteering = 0f;

    float turningSpeed = 60f;
    float movementSpeed = 7f;

    Transform mTrans;
//    GameShip mStats;
//    Cannon[] mCannons;

    /// <summary>
    /// For controlling the ship via external means (such as AI)
    /// </summary>

    public Vector2 input { get { return mInput; } set { mInput = value; } }

    /// <summary>
    /// Current speed (0-1 range)
    /// </summary>

    public float speed { get { return mSpeed; } }

    /// <summary>
    /// Current steering value (-1 to 1 range)
    /// </summary>

    public float steering { get { return mSteering; } }

    /// <summary>
    /// Helper function that finds the ship control script that contains the specified child in its transform hierarchy.
    /// </summary>

    static public CharaController Find(Transform trans)
    {
        while (trans != null)
        {
            CharaController ctrl = trans.GetComponent<CharaController>();
            if (ctrl != null) return ctrl;
            trans = trans.parent;
        }
        return null;
    }

    public CharaController()
    {
        raycastPoints = new List<Transform>();
    }

    /// <summary>
    /// Cache the transform
    /// </summary>

    public void Init()
    {
        if (SceneMgr.Instance.curSceneGO == null)
            return;
        mTrans = EntityMainRole.Instance.transform;
//        mStats = mTrans.gameObject.GetComponent<GameShip>();
//        mCannons = mTrans.gameObject.GetComponentsInChildren<Cannon>();
        // 碰撞点
        Transform point = mTrans.FindChild("Raycast Point");
        raycastPoints.Add(point);
        // 碰撞检测层
        raycastMask.value = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Ship"));
        TickMgr.Instance.AddTick(this);
    }

    /// <summary>
    /// Update the input values, calculate the speed and steering, and move the transform.
    /// </summary>

    public void OnTick(float dt)
    {
        // Update the input values if controlled by the player
        //if (controlledByInput) UpdateInput();
        UpdateInput();

        bool shallowWater = false;

        // Determine if the ship has hit shallow water
//        if (raycastPoints.Count != 0)
//        {
//            for (int i = 0; i < raycastPoints.Count; i++)
//            {
//                Transform point = raycastPoints[i];
//                if (Physics.Raycast(point.position + Vector3.up * 10f, Vector3.down, 10f, raycastMask))
//                {
//                    shallowWater = true;
//                    Debug.Log("shallow water !!!!!");
//                    break;
//                }
//            }
//        }
//
//        // 触礁停船
//        if (shallowWater) mInput.y = 0f;
//        float delta = Time.deltaTime;
//
//        // Slowly decay the speed and steering values over time and make sharp turns slow down the ship.
//        mTargetSpeed = Mathf.Lerp(mTargetSpeed, 0f, delta * (0.5f + Mathf.Abs(mTargetSteering)));
//        mTargetSteering = Mathf.Lerp(mTargetSteering, 0f, delta * 3f);
//
//        // Calculate the input-modified speed
//        mTargetSpeed = shallowWater ? 0f : Mathf.Clamp01(mTargetSpeed + delta * mSensitivity.y * mInput.y);
//        mSpeed = Mathf.Lerp(mSpeed, mTargetSpeed, Mathf.Clamp01(delta * (shallowWater ? 8f : 5f)));
//
//        // Steering is affected by speed -- the slower the ship moves, the less maneuverable is the ship
//        mTargetSteering = Mathf.Clamp(mTargetSteering + delta * mSensitivity.x * mInput.x * (0.1f + 0.9f * mSpeed), -1f, 1f);
//        mSteering = Mathf.Lerp(mSteering, mTargetSteering, delta * 5f);
//
//        // Move the ship
//        mTrans.localRotation = mTrans.localRotation * Quaternion.Euler(0f, mSteering * delta * turningSpeed, 0f);
//        mTrans.localPosition = mTrans.localPosition + mTrans.localRotation * Vector3.forward * (mSpeed * delta * movementSpeed);
    }

    /// <summary>
    /// Update the input (used when ship is controlled by the player).
    /// </summary>
  

    void UpdateInput()
    {
#if UNITY_EDITOR
        mInput.y = Mathf.Clamp01(Input.GetAxis("Vertical"));
        mInput.x = Input.GetAxis("Horizontal");
#else
        mInput.x = Input.acceleration.x;
        mInput.y = Mathf.Abs(Input.acceleration.y);
#endif
//
//        // Fire the cannons
//        if (mCannons != null && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.JoystickButton0)))
//        {
//            Vector3 dir = GameCamera.flatDirection;
//            Vector3 left = mTrans.rotation * Vector3.left;
//            Vector3 right = mTrans.rotation * Vector3.right;
//
//            left.y = 0f;
//            right.y = 0f;
//
//            left.Normalize();
//            right.Normalize();
//
//            // Calculate the maximum firing range using the best available cannon
//            float maxRange = 1f;
//
////            foreach (Cannon cannon in mCannons)
////            {
////                float range = cannon.maxRange;
////                if (range > maxRange) maxRange = range;
////            }
//
//            // Aim and fire the cannons on each side of the ship, force-firing if the camera is looking that way
//            AimAndFire(left, maxRange, Vector3.Angle(dir, left) < 60f);
//            AimAndFire(right, maxRange, Vector3.Angle(dir, right) < 60f);
//        }
    }

    /// <summary>
    /// Aim and fire the cannons given the specified direction and maximum range.
    /// </summary>

    void AimAndFire(Vector3 dir, float maxRange, bool forceFire)
    {
        Debug.Log("Aim And Fire");
//        float distance = maxRange * 1.2f;
////        GameUnit gu = GameUnit.Find(mStats, dir, distance, 60f);
//
//        // If a unit was found, override the direction and angle
//        if (gu != null)
//        {
//            dir = gu.transform.position - mTrans.position;
//            distance = dir.magnitude;
//            if (distance > 0f) dir *= 1.0f / distance;
//            else distance = maxRange;
//
//            // Fire in the target direction
//            Fire(dir, distance);
//        }
//        else if (forceFire)
//        {
//            // No target found -- only fire if asked to
//            Fire(dir, distance);
//        }
    }

    /// <summary>
    /// Fire the ship's cannons in the specified direction.
    /// </summary>

    public void Fire(Vector3 dir, float distance)
    {
//        if (mCannons != null)
//        {
//            foreach (Cannon cannon in mCannons)
//            {
//                cannon.Fire(dir, distance);
//            }
//        }
    }
}