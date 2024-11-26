using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("スコア関連")]
    public Text ScoreText; // スコア表示用のUI（Unity Inspectorで設定する）
    private int CurrentScore = 0; // スコアの初期値（初期値は0）

    [Header("タイマー関連")]
    public Text TimeText; // タイム表示用のUI（Unity Inspectorで設定する）
    private float timer = 0f; // タイマーの初期値
    private bool isTiming = true; // タイマーが進行中かどうかを管理するフラグ

    // 毎フレーム実行される処理
    private void Update()
    {
        // タイマーが有効な場合
        if (isTiming)
        {
            timer += Time.deltaTime; // 時間を経過分だけ加算
            UpdateTimeUI(); // タイム表示を更新
        }
    }

    // スコアを指定した値だけ加算するメソッド
    public void AddScore(int amount)
    {
        CurrentScore += amount; // スコアを加算
        UpdateScoreUI(); // スコア表示を更新
    }

    // タイムボーナスを計算してスコアに加算するメソッド
    public void AddTimeBonus()
    {
        int timeBonus = CalculateTimeBonus(); // 現在のタイムに応じたボーナスを計算
        CurrentScore += timeBonus; // タイムボーナスをスコアに加算
        UpdateScoreUI(); // スコア表示を更新
        Debug.Log($"Time Bonus: {timeBonus}"); // タイムボーナスの値をログに表示
    }

    // タイマーをリセットするメソッド
    public void ResetTimer()
    {
        timer = 0f; // タイマーを0にリセット
        UpdateTimeUI(); // タイム表示を更新
    }

    // 現在のタイムに基づいてタイムボーナスを計算するメソッド
    private int CalculateTimeBonus()
    {
        // タイマーの値に応じてボーナスを計算して返す
        if (timer <= 20f) return 10000; // 20秒以下の場合
        if (timer <= 30f) return 5000;  // 30秒以下の場合
        if (timer <= 60f) return 2500;  // 60秒以下の場合
        return 1000;                   // それ以上の場合
    }

    // スコア表示を更新するメソッド
    private void UpdateScoreUI()
    {
        // スコアのUIが設定されている場合
        if (ScoreText != null)
        {
            // スコアを「7桁のゼロ埋め」形式で表示
            ScoreText.text = $"SCORE: {CurrentScore:D7}";
        }
    }

    // タイム表示を更新するメソッド
    private void UpdateTimeUI()
    {
        // タイムのUIが設定されている場合
        if (TimeText != null)
        {
            // 分、秒、小数点以下を計算
            int minutes = Mathf.FloorToInt(timer / 60f); // タイマーを60で割った整数部分を分とする
            int seconds = Mathf.FloorToInt(timer % 60f); // タイマーを60で割った余りを秒とする
            int milliseconds = Mathf.FloorToInt((timer * 100) % 100); // タイマーを100倍して余りを小数点以下の値とする

            // 計算結果をフォーマットしてタイムUIに反映
            TimeText.text = $"TIME: {minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }
}