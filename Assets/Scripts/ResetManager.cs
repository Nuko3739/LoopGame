using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [Header("ループ処理関連")]
    public ScoreManager ScoreManager; // ScoreManagerへの参照（Inspectorで設定）
    public Transform RestartPosition; // プレイヤーを戻す位置
    public string PlayerTag = "Player"; // プレイヤーのタグ

    [Header("Enemy生成関連")]
    public List<GameObject> EnemyPrefabs; // スポーンするEnemyのプレハブリスト
    public BoxCollider2D SpawnArea;       // スポーン範囲（BoxCollider2D）
    public int EnemiesPerLoop = 5;        // 各ループで生成するEnemyの数

    private List<GameObject> SpawnedEnemies = new List<GameObject>(); // 生成したEnemyを保持

    private void Start()
    {
        if (RestartPosition == null)
        {
            Debug.LogError("RestartPositionが設定されていません！");
        }

        if (EnemyPrefabs == null || EnemyPrefabs.Count == 0)
        {
            Debug.LogError("EnemyPrefabsが設定されていません！");
        }

        if (SpawnArea == null)
        {
            Debug.LogError("SpawnAreaが設定されていません！");
        }
    }

    //ループ処理開始
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag)) // プレイヤーのみ処理を実行
        {
            Debug.Log("プレイヤーがゴールに到達しました。");
            StartCoroutine(HandleStageEnd(collision));
        }
    }

    //ステージのループ処理
    private IEnumerator HandleStageEnd(Collider2D player)
    {
        // フェードアウト処理（仮）
        //現在はコルーチンで待機、この待ってる時間は敵の動きを止めることを忘れずに
        //　　　　　　　　　　　　時間停止はまだ未実装↑
        yield return StartCoroutine(FadeOut());

        // タイムボーナスをスコアに加算
        ScoreManager.AddTimeBonus();

        // プレイヤーをリスタート位置に移動
        if (RestartPosition != null)
        {
            player.transform.position = RestartPosition.position;
            Debug.Log("プレイヤーをリスタート位置に移動しました。");
        }
        else
        {
            Debug.LogError("RestartPositionが設定されていないため、プレイヤーを移動できませんでした。");
        }

        // タイマーリセット
        ScoreManager.ResetTimer();
        // Enemy生成処理
        SpawnNewEnemies();
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("フェードアウト中...");
        yield return new WaitForSeconds(2f); // フェード処理の代替として2秒待機
        Debug.Log("フェードアウト終了。");
    }

    private void SpawnNewEnemies()
    {
        if (EnemyPrefabs == null || EnemyPrefabs.Count == 0)
        {
            Debug.LogError("EnemyPrefabsが設定されていません。スポーンするEnemyを設定してください。");
            return;
        }

        if (SpawnArea == null)
        {
            Debug.LogError("SpawnAreaが設定されていません。スポーンエリアを設定してください。");
            return;
        }

        //現在は５体固定
        Debug.Log($"Enemyを{EnemiesPerLoop}体生成します。");

        // ステージの高さ制限を取得
        float minHeight = SpawnArea.bounds.min.y; // スポーンエリア範囲の下端(最小値のY座標)を取得
        float maxHeight = SpawnArea.bounds.max.y; // スポーンエリア範囲の上端(最大値のY座標)を取得

        for (int i = 0; i < EnemiesPerLoop; i++)
        {
            // BoxCollider2Dの範囲を基にスポーン位置をランダム設定
            Vector2 spawnPosition = new Vector2(
                Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x),
                Random.Range(minHeight, maxHeight) // 高さを制限
            );

            // Enemyプレハブをランダムに選択
            GameObject RandomEnemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];

            // Enemyを生成してリストに追加
            GameObject newEnemy = Instantiate(RandomEnemyPrefab, spawnPosition, Quaternion.identity);
            //Quaternion.identityは用途: オブジェクト生成時に特定の回転を設定。でこの場合は回転を適用しない

            SpawnedEnemies.Add(newEnemy);

            Debug.Log($"Enemy生成: {newEnemy.name} at {spawnPosition}");
        }
    }
}
