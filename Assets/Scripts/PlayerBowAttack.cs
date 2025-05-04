using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowAttack : MonoBehaviour
{
    [Header("弓矢関連の設定")]
    public GameObject ArrowPrefab;        // 発射する矢のプレハブ
    public Transform ShootPoint;          // 矢を発射する位置
    public float ArrowSpeed = 10f;         // 矢のスピード
    public float BowAttackCooldown = 0.5f; // 弓のクールダウン時間

    private bool isCanShoot = true;        // 弓を撃てるかどうか

    private Player player;                 // プレイヤー本体参照用

    private void Start()
    {
        // 同じオブジェクトにあるPlayerスクリプトを取得
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && isCanShoot && !player.IsDodging())
        {
            StartCoroutine(ShootArrow());
        }
    }

    private IEnumerator ShootArrow()
    {
        isCanShoot = false;

        // 矢を生成
        GameObject arrow = Instantiate(ArrowPrefab, ShootPoint.position, Quaternion.identity);

        // 矢の向きをプレイヤーの向きに合わせる
        Vector2 arrowDirection = player.IsFacingRight() ? Vector2.right : Vector2.left;
        arrow.GetComponent<Rigidbody2D>().velocity = arrowDirection * ArrowSpeed;

        // クールダウン
        yield return new WaitForSeconds(BowAttackCooldown);
        isCanShoot = true;
    }

   
}
