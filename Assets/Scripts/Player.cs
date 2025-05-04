using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //  仮
    private int currentScore = 10000; // 仮のスコア（後でスコアシステムから取得する）




    [Header("Playerの設定")]
    public int PlayerHp = 3;  // プレイヤーのHP
    //public int PlayerPower = 1; //playerの攻撃力

    [Header("移動関連の設定")]
    public float MoveSpeed = 5f;    // 移動速度
    public float MoveStep = 3f;     // 回避ステップの距離

    [Header("ジャンプ関連の設定")]
    public float JumpForce = 7f;    // 最大ジャンプ力
    public float MinJumpForce = 2f; // 最小ジャンプ力
    public float MaxJumpHoldTimm = 0.2f; // ジャンプボタンの最大時間

    [Header("物理関連の設定")]
    public float GravityScale = 1f; // 重力の強さ
    public float AddGravityScale = 1.5f; //ジャンプ後の重力の加算

    [Header("回避ステップ関連")]
    public float StepDuration = 0.2f; // ステップの持続時間
    public float StepCooldown = 0.5f; // ステップのクールダウン
    private bool isDodging = false;   // ステップ中かどうか
    private bool canDodge = true;     // ステップ可能かどうか

    [Header("Sound設定")]
    public AudioClip StepSE; 　　　　 //回避用のSE
    private AudioSource audioSource;  //Sound再生用AudioSource


    private Rigidbody2D rb;           // 2D物理エンジンのRigidbody2D
    private SpriteRenderer spriteRenderer; // スプライトレンダラーの参照
    private Animator animator; // アニメーターの参照
    private bool isSmallJump = false; // 小ジャンプかどうかを管理する


    private bool isGround;            // ジャンプ関連の地面判定フラグ
    private bool isJumping;           // ジャンプ中か判定するフラグ
    private float JumpTimeCounter;    // ジャンプボタンを押している時間
    private bool isFacingRight = true; // プレイヤーの向きを追跡（デフォルトは右向き）
    private bool isInvincible = false; // 無敵状態かどうかのフラグ
    //private bool isCanAttack = true;     // 攻撃できるかどうかのフラグ


    [Header("ループ設定")]
    public Transform RestartPosition;     //playerが右端に着いてから戻る場所
                                          //↑Restart位置のTransformを格納する
                                          //[Header("攻撃関連の設定")]
                                          //public Collider2D AttackCollider; // 攻撃判定のコライダー
                                          //public float AttackCooldown = 0.5f; // 攻撃のクールダウン時間

    private ScoreManager ScoreManager;

    [Header("攻撃被弾：無敵時間の設定")]
    public float InvincibleDuration = 0.25f; // 無敵時間の長さ
    public float BlinkInterval = 0.000001f; // 点滅間隔

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D（2D版）の取得
        animator = GetComponent<Animator>();  // アニメーターの取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        ScoreManager = FindObjectOfType<ScoreManager>(); //ScoreManagerを取得

        rb.gravityScale = GravityScale;// Inspectorで設定できる重力をRigidbody2Dに反映

        audioSource = GetComponent<AudioSource>();//回避のSound用
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //Gool処理
        //タグ名Restartを持つオブジェクトを検索して、そのTransformを取得
        GameObject RestartObject = GameObject.FindGameObjectWithTag("Restart");
        if(RestartObject != null)
        {
            RestartPosition = RestartObject.transform;
        }
        else
        {　　　　　　　//確認用
            Debug.Log("Restartのオブジェクトが見つかりません");
        }

        //攻撃コライダーを初期的に無効にする
        //AttackCollider.enabled = false;
    }

    private void Update()
    {
        // プレイヤーの移動処理を呼び出し
        PlayerMove();

        //プレイヤーの回避処理の呼び出し
        PlayerStep();

        //空中にいるときに落下速度を速める
        if (!isGround && rb.velocity.y < 0)
        {
            rb.gravityScale = GravityScale * AddGravityScale; // 落下時の重力を強化
        }
        else
        {
            rb.gravityScale = GravityScale; // 通常の重力に戻す
        }

        
    }


    // 無敵状態の間、プレイヤーが点滅するコルーチン
    private IEnumerator InvincibleBlinkCoroutine()
    {
        while (isInvincible)
        {
            spriteRenderer.color = Color.clear; // 透明（白く消える）
            yield return new WaitForSeconds(BlinkInterval);
            spriteRenderer.color = Color.white; // 元の色に戻す
            yield return new WaitForSeconds(BlinkInterval);
        }
    }


    // プレイヤーがダメージを受けた際の無敵時間を付与するコルーチン
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;  // 無敵状態にする
        StartCoroutine(InvincibleBlinkCoroutine());  // 無敵状態の点滅を開始
        yield return new WaitForSeconds(InvincibleDuration);  // 無敵時間の間待機
        isInvincible = false;  // 無敵状態を解除
    }

    // プレイヤーの移動処理をメソッドに分離
    private void PlayerMove()
    {
        if (isDodging) return; // ステップ中は何もしない

        // WASD移動
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * MoveSpeed, rb.velocity.y);

        // アニメーションの設定
        if (moveX != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        // ↑ animator.SetBool("isRunning", moveX != 0);

        // 左右のキー入力に応じてキャラクターの向きを変更
        //　空中にいるときは向きを変えない
        if (isGround || Input.GetKey(KeyCode.J)) // 地面にいるか、攻撃していたら反転許可
        {
            if (moveX > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveX < 0 && isFacingRight)
            {
                Flip();
            }
        }


        // ジャンプ処理
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
            JumpTimeCounter = MaxJumpHoldTimm;
            rb.velocity = new Vector2(rb.velocity.x, MinJumpForce); // 最小ジャンプ力を設定

            isSmallJump = true; // 最初は小ジャンプと仮定しておく
        }

        // ジャンプボタンを押し続けている場合
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (JumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce); // 最大ジャンプ力まで加算
                                                                     //JumpTimeCounter -= Time.deltaTime; // 押している時間を計測
                JumpTimeCounter -= Time.deltaTime * 1.5f;  // 減少速度を速めて上昇時間を短縮

                // 長押しされたら小ジャンプではないと判定
                if (JumpTimeCounter < MaxJumpHoldTimm * 0.7f) // 70%以上押したら大ジャンプ判定
                {
                    isSmallJump = false;
                }
            }
            else
            {
                isJumping = false; // ホバリング抑止
            }
        }

        // ジャンプボタンを離した場合
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false; // ホバリング抑止
        }





        // --- ここからアニメーション制御 ---

        // ジャンプのアニメーション
        if (!isGround)
        {
            if (isSmallJump)
            {
                animator.SetBool("isSmallJump", true); // 小ジャンプアニメを再生
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true); // 通常ジャンプアニメを再生
                animator.SetBool("isSmallJump", false);
            }
        }
        else
        {
            // 地面に着いたらリセット
            animator.SetBool("isJumping", false);
            animator.SetBool("isSmallJump", false);
        }
    }

    //回避ステップのメソッド
    private void PlayerStep()
    {
        // ステップ入力処理（Lキー）
        if (Input.GetKeyDown(KeyCode.L) && canDodge && !isDodging && isGround)
        {
            StartCoroutine(DodgeStep());
        }
    }

    //ステップ処理のコルーチン
    private IEnumerator DodgeStep()
    {
        animator.SetBool("isDodging", true);
        // ステップ終わりに
        
        animator.Update(0); // 即座にアニメーション状態を反映
        // ステップSE再生
        if (StepSE != null)
        {
            audioSource.PlayOneShot(StepSE);
        }

        isDodging = true;
        canDodge = false;
        isInvincible = true;

        float stepDirection = isFacingRight ? 1f : -1f;

        
        // 点滅演出をスタート
        StartCoroutine(DodgeBlinkCoroutine());

        // ステップ移動
        rb.velocity = new Vector2(stepDirection * MoveStep / StepDuration, 0f);

        yield return new WaitForSeconds(StepDuration);

        animator.SetBool("isDodging", false);

        // 終了処理
        rb.velocity = Vector2.zero;
        isDodging = false;
        isInvincible = false;

        yield return new WaitForSeconds(StepCooldown);
        canDodge = true;
    }

    //回避ステップの青点滅
    private IEnumerator DodgeBlinkCoroutine()
    {
        while (isDodging)
        {
            // 青く点滅（描画ON/OFFではなく色変化で明示的に青系に）
            spriteRenderer.color = Color.cyan;
            yield return new WaitForSeconds(0.05f); // 50ミリ秒 ON

            spriteRenderer.color = new Color(0.3f, 0.3f, 1f); // 少し薄い青
            yield return new WaitForSeconds(0.05f); // 50ミリ秒 OFF風（別の青）
        }

        // ステップが終わったら色を戻す
        spriteRenderer.color = Color.white;
    }


    // キャラクターの向きを反転させるメソッド
    private void Flip()
    {
        isFacingRight = !isFacingRight; // 向きを反転
        Vector3 theScale = transform.localScale; // 現在のスケールを取得
        theScale.x *= -1; // X軸方向のスケールを反転
        transform.localScale = theScale; // 反転したスケールを適用
    }

    // ジャンプ関連の地面判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // 地面に接触したらisGroundをtrueにする
        {
            isGround = true;
        }

        // Enemyタグを持つオブジェクトに衝突したとき
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();  // プレイヤーがダメージを受ける
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // 地面から離れたらisGroundをfalseにする
        {
            isGround = false;
        }
    }
    
    // プレイヤーがダメージを受ける処理
    public void TakeDamage()
    {

        if (isInvincible)//無敵状態ならダメージを受けない
        {
            return;
        }

        PlayerHp--;  // HPを減らす
        Debug.Log("Player HP: " + PlayerHp);

        // ダメージを受けた際に一瞬赤く光る
        //StartCoroutine(FlashDamage());

        //無敵時間を付与する（この場合は一回のダメージで複数回ダメージを受けるのを防止するため）
        StartCoroutine(InvincibleCoroutine());

        if (PlayerHp <= 0)
        {
            Debug.Log("Playerがやられた！");

            // スコアをScoreManagerから取得
            int finalScore = 0;
            if (ScoreManager != null)
            {
                finalScore = ScoreManager.GetScore(); // ScoreManagerからスコアを取得
                //Debug.Log("ScoreManagerから取得したスコア: " + finalScore); // ここでスコアを確認
            }

            // RankingManagerを探してスコアを登録
            RankingManager rankingManager = FindObjectOfType<RankingManager>();
            if (rankingManager != null)
            {
                Debug.Log("Current Score being added: " + currentScore); // 確認用ログ
                rankingManager.AddNewScore(finalScore); // スコアを追加
            }
            else
            {
                Debug.LogError("RankingManagerが見つかりませんでした！"); // 見つからない場合のエラーログ
            }

            // ゲームオーバーシーンに遷移
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // ステップ中かどうかを他スクリプトから参照する用
    public bool IsDodging()
    {
        return isDodging;
    }
    // プレイヤーが右を向いているかを他スクリプトから参照する用
    public bool IsFacingRight()
    {
        return isFacingRight;
    }
}
