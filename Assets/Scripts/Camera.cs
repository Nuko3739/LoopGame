using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Player;  //プレイヤーのTransformを取得
    public float CameraSpeed = 0.125f; //カメラ追従の速度
    public float CameraPedX;
    public Vector3 offset; //カメラとプレイヤーの位置オフセット

    [Header("カメラX軸の移動制限範囲")]
    public float MinX = 0f;  // X軸の最小値（左端）
    public float MaxX = 10f;   // X軸の最大値（右端）


    private void LateUpdate()
    {
        // カメラのY軸とZ軸は固定して、X軸のみ追従
        Vector3 targetPosition = new Vector3(Player.position.x + offset.x, transform.position.y, transform.position.z);

        // X軸の範囲を制限する
        CameraPedX = Mathf.Clamp(targetPosition.x, MinX, MaxX);

        // スムーズに追従させるための補完処理
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(CameraPedX, targetPosition.y, targetPosition.z), CameraSpeed);
        transform.position = smoothedPosition;
    }
}
