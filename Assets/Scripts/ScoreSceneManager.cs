using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSceneManager : MonoBehaviour
{
    private int currentScore = 0; // 現在のスコアを保持

    private void Start()
    {
        // シーンを跨いでオブジェクトを保持
        DontDestroyOnLoad(gameObject);

        // シーン切り替え時のイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // イベント登録解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // スコアを次のシーンに引き継ぐ
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 次のシーンでScoreManagerを探す
        var scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            // ScoreManagerにスコアを引き継ぐ
            scoreManager.AddScore(currentScore);
        }
    }

    // スコアを更新するメソッド
    public void UpdateScore(int score)
    {
        currentScore = score;
    }

    // スコアをリセットするメソッド（必要なら追加）
    public void ResetScore()
    {
        currentScore = 0;
    }
}
