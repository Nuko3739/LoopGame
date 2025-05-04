using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player; // Playerクラスの参照を取得

    [Header("攻撃関連の設定")]
    public int Player_TotalAttackFrames = 7;        //player攻撃アニメーションの全体フレーム数
    public int Player_ColliderEnableStartFrame = 3; //コライダーを有効にするフレーム
    public int Player_ColliderEnableEndFrame = 4; 　//コライダーを無効にするフレーム

    public float AnimationFrameDuration = 0.033f;   // フレームごとの時間（例として30FPSなら約0.033秒）

    public Collider2D AttackCollider;//攻撃判定のコライダー
    public float AttackCooldown = 0.45f;//攻撃のクールダウンタイム

    public Animator animator;
    private bool isCanAttack = true;

    private int PlayerAttackLayerIndex;//アニメーションの攻撃用レイヤーを取得するための変数

    public int AnimationFrameDutation { get; private set; }

    private void Start()
    {
        //playerを参照
        player = GetComponent<Player>();

       //Animaterコンポーネントのアサインを忘れないようにします
       if (animator == null)
        {
            animator = GetComponent<Animator>();//自動でアサインする
        }

        // Player_Attack レイヤーのインデックスを取得
        PlayerAttackLayerIndex = animator.GetLayerIndex("Player_Attack");

        // 攻撃コライダーを無効にしておく
        AttackCollider.enabled = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isCanAttack && !player.IsDodging()) // Jキーで攻撃
        {
            StartCoroutine(Attack());//攻撃処理
        }
    }

    private IEnumerator Attack()
    {
        isCanAttack = false; // 攻撃フラグをfalseにしてクールダウン期間中は再度攻撃できないようにする
        animator.SetTrigger("AttackTrigger");

        //ここでは３フレーム目から攻撃判定を出したいのでそれまで待機させる
        // 攻撃判定フレームまで待つ
        yield return new WaitForSeconds(AnimationFrameDuration * Player_ColliderEnableStartFrame);
                                        //フレームごとの時間　　//コライダーを有効にし始めるフレーム数　　　

        //コライダーを有効にする
        AttackCollider.enabled = true;//コライダーを有効にするフレーム数後からコライダーを有効にする


        //↓依存関係がひどくバグの温床になっている、待機時間をしてからさらに待機時間を用意し待たせるならこれはやめたほうがいい
        //StartCoroutine(EnableAttackCollider());


        //攻撃コライダーが有効になってから無効になるまでの間の時間を計算（ここでは１フレームだけ攻撃判定を表示している）して待機する
        //この個所では攻撃コライダー有効時間である　【攻撃判定時間(１F）を取得して待機している】
        yield return new WaitForSeconds(AnimationFrameDuration * (Player_ColliderEnableEndFrame - Player_ColliderEnableStartFrame));

        //コライダーを無効にする
        AttackCollider.enabled = false;


        // 攻撃のクールダウンを開始
        yield return new WaitForSeconds(AttackCooldown); // クールダウンの完了を待つ
        isCanAttack = true; // クールダウン終了後に再度攻撃可能にする
    }

    // 敵にダメージを与える処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // AttackCollider が有効で、衝突したオブジェクトが "Enemy" タグを持っている場合のみ処理する
        if (AttackCollider.enabled && collision.CompareTag("Enemy") && collision.gameObject.tag != "Enemy_Search")
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Enemyにダメージを与える（ダメージ値は仮に1）
                Debug.Log("Enemy Hp:" + enemy.EnemyHp); // EnemyHpの確認用
            }
        }
    }
}
