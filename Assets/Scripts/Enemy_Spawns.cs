using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawns : MonoBehaviour
{
    [Header("生成可能なEnemyのリスト")]
    public List<GameObject> EnemyPrefabs; // プレハブ化されたEnemyリスト
    public Collider2D SpawnArea;          // Enemyを生成可能な範囲

    // ランダムなEnemyを生成する
    public EnemyData SpawnRandomEnemy()
    {
        if (EnemyPrefabs.Count == 0 || SpawnArea == null)
        {
            Debug.LogError("EnemyPrefabまたはSpawnAreaが設定されていません！");
            return null;
        }

        // 生成位置をランダムで決定
        Vector2 spawnPosition = new Vector2(
            Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x),//横軸をランダム設定
            Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y)// 縦軸をランダム設定
        );

        // Enemyをランダムに選択して生成
        GameObject selectedEnemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];
        GameObject spawnedEnemy = Instantiate(selectedEnemyPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"Enemy生成: {spawnedEnemy.name} at {spawnPosition}");

        // 生成情報を保存
        return new EnemyData
        {
            EnemyPrefab = selectedEnemyPrefab,
            Position = spawnPosition
        };
    }
}

// Enemyの生成情報を保持する構造体
[System.Serializable]
public class EnemyData
{
    public GameObject EnemyPrefab; // 生成したEnemyのプレハブ
    public Vector2 Position;       // 生成位置
}
