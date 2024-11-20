using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("共通設定")]
    public int EnemyHp = 2; // 敵のHP

    protected SpriteRenderer spriteRenderer; // スプライトレンダラーの参照
    protected Rigidbody2D rb; // Rigidbody2Dの参照
    protected bool isFacingRight = true; // 敵が右を向いているかどうかのフラグ
    protected bool isAlive = true; // 敵が生存しているかどうかのフラグ

    // 初期設定メソッド
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRendererを取得

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererが設定されていません！");
        }
    }

    // ダメージを受ける処理
    public virtual void TakeDamage(int damage)
    {
        if (!isAlive) return; // 敵が既に死亡している場合は処理しない

        EnemyHp -= damage; // HPを減少させる
        StartCoroutine(FlashDamage()); // ダメージを受けた際の点滅エフェクト

        if (EnemyHp <= 0) // HPが0以下になった場合
        {
            Die(); // 敵を死亡させる
        }
    }

    // ダメージを受けた際の点滅エフェクト
    protected IEnumerator FlashDamage()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("spriteRendererがnullです。SpriteRendererが正しく設定されているか確認してください。");
            yield break; // 処理を中断
        }
        spriteRenderer.color = Color.blue;　　// ダメージを受けた際に青く点滅
        yield return new WaitForSeconds(0.1f);// 0.1秒待機
        spriteRenderer.color = Color.white;   // 元の色に戻す



        //spriteRenderer.color = Color.red; // ダメージを受けた際に赤く点滅
        //yield return new WaitForSeconds(0.1f); // 0.1秒待機
        //spriteRenderer.color = Color.white; // 元の色に戻す
    }

    // 死亡処理
    protected virtual void Die()
    {
        isAlive = false; // 敵が死亡状態になる
        Destroy(gameObject); // 敵のゲームオブジェクトを破壊する
    }

    // 向きを反転させるメソッド
    protected void Flip()
    {
        isFacingRight = !isFacingRight; // 向きのフラグを反転
        Vector3 scale = transform.localScale;
        scale.x *= -1; // スプライトのX軸のスケールを反転
        transform.localScale = scale; // 反転したスケールを適用
    }
}
