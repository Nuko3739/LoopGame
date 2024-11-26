using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : EnemyBase
{
    public float Enemy_MoveSpeed = 2.0f;       // Batの移動速度
    public float Enemy_LeftBoundary = -5.0f;  // 左端の位置
    public float Enemy_RightBoundary = 5.0f;  // 右端の位置
    public float MaxRiseFallHeight = 3.5f;    // 上下運動の最大振幅
    public float VerticalSpeed = 2.0f;        // 上下運動の速度

    private bool Enemy_MovingRight = true;    // 現在の移動方向が右かどうか
    private float Enemy_Initial_Y;           // 初期のY位置
    private float phaseOffset;               // 上下運動のランダム位相オフセット
    //　　　　　　　　　↑Enemyが上下するタイミングをそれぞれずらすため

    private void Start()
    {
        base.Start(); // EnemyBaseのStart()メソッドを呼び出す
        Enemy_Initial_Y = transform.position.y; // 初期Y位置を保存
        phaseOffset = Random.Range(0f, Mathf.PI * 2); // ランダム位相オフセットを生成
    }

    // 毎フレーム呼び出されるメソッド
    private void Update()
    {
        if (isAlive) // 敵が生存している場合のみ
        {
            PatrolWithVerticalMovement(); // 左右移動しつつ上下運動を行う
        }
    }

    // 左右に反復移動を行いながら上下運動を加えるメソッド
    private void PatrolWithVerticalMovement()
    {
        Vector2 newPosition = transform.position; // リスポーンを踏まえリスポーン位置を取得する変数

        // 水平方向の移動
        if (Enemy_MovingRight)
        {
            newPosition.x += Enemy_MoveSpeed * Time.deltaTime;

            if (newPosition.x >= Enemy_RightBoundary) // 右端に到達
            {
                Enemy_MovingRight = false; // 左に移動するように設定
                Flip(); // 向きを反転
            }
        }
        else
        {
            newPosition.x -= Enemy_MoveSpeed * Time.deltaTime;

            if (newPosition.x <= Enemy_LeftBoundary) // 左端に到達
            {
                Enemy_MovingRight = true; // 右に移動するように設定
                Flip(); // 向きを反転
            }
        }

        // 上下運動を計算（ランダムな位相オフセットを追加）
        newPosition.y = Enemy_Initial_Y + Mathf.Sin(Time.time * VerticalSpeed + phaseOffset) * MaxRiseFallHeight;
        //Mathf.Sinは（サイン関数）の値を計算するのに使用、バットの上下揺れを制御。

        // 計算した新しい位置に移動
        transform.position = newPosition;
    }
}