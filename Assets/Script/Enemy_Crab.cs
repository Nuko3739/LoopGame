using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーを追跡するCrab型の敵クラス
public class Enemy_Crab : EnemyBase
{
    public float Enemy_MoveSpeed = 3f; // 敵の移動速度
    public float Enemy_Player_X_Flip = 0.5f; // プレイヤーとのX軸の距離がこの値以上で反転
    public CircleCollider2D PlayerDetectionCollider; // プレイヤーを感知するサークルコライダー

    private Transform Player; // プレイヤーの位置情報
    private bool isChasing = false; // プレイヤー追跡中かどうかのフラグ

    // 初期設定メソッド
    protected override void Start()
    {
        base.Start(); // EnemyBaseのStart()を呼び出して基本設定を行う

        // プレイヤーのTransformを取得
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        isChasing = false; // 初期状態では追跡モードをオフにする
    }

    // 毎フレーム呼び出されるメソッド
    private void Update()
    {
        if (isChasing && isAlive) // プレイヤーを追跡中で生存している場合のみ
        {
            ChasePlayer(); // プレイヤーを追跡
        }
        else
        {
            rb.velocity = Vector2.zero; // 感知範囲外の場合は停止
        }
    }

    // プレイヤーが感知範囲に入ったときに追跡を開始する
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 感知用のコライダーのみで追跡判定を行う
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("プレイヤーが感知範囲に入りました");
        }
    }

    // プレイヤーが感知範囲から出た際に追跡を停止するメソッド
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false; // プレイヤー追跡停止
            Debug.Log("プレイヤーが感知範囲から出ました");
        }
    }

    // プレイヤーを追跡するメソッド
    private void ChasePlayer()
    {
        Vector2 direction = (Player.position - transform.position).normalized; // プレイヤーへの方向を計算
        rb.velocity = new Vector2(direction.x * Enemy_MoveSpeed, rb.velocity.y); // X軸方向の速度を設定

        FlipDirection(direction.x); // プレイヤーの位置に応じて敵の向きを反転
    }

    // プレイヤーの位置に応じて敵の向きを反転するメソッド
    private void FlipDirection(float directionX)
    {
        if ((directionX < 0 && isFacingRight) || (directionX > 0 && !isFacingRight)) // プレイヤーの方向に応じて
        {
            Flip(); // 向きを反転
        }
    }
}