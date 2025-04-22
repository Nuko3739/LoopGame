using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    private ScoreLinkedList rankingList = new ScoreLinkedList(); // リンクリスト
    public Text RankingText;        // ランキング表示用のUI
    public GameObject GameOverUI;   // GameOver表示用のUI
    public GameObject RankingUI;    // ランキング表示用のUI

    private bool isRankingVisible = false; // ランキング表示フラグ

    private void Start()
    {
        rankingList.LoadFromPlayerPrefs(); // ランキングを読み込む
        UpdateRankingUI(); // ランキングをUIに表示

        // 初期表示設定
        GameOverUI.SetActive(true);
        RankingUI.SetActive(false);
    }

    private void Update()
    {
        // EnterキーでUIを切り替える
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleRankingUI();
        }
    }

    // 新しいスコアをランキングに追加
    public void AddNewScore(int score)
    {
        Debug.Log("AddNewScore に渡されたスコア: " + score); // 確認用
        rankingList.AddScore(score); // リンクリストにスコアを追加
        rankingList.SaveToPlayerPrefs(); // ランキングを保存
        UpdateRankingUI();
        Debug.Log("ランキングの保存が完了しました。");
    }

    // ランキングUIを更新
    private void UpdateRankingUI()
    {
        if (RankingText != null)
        {
            RankingText.text = rankingList.GetFormattedRanking();
        }
    }

    // UI表示の切り替え
    private void ToggleRankingUI()
    {
        isRankingVisible = !isRankingVisible;

        GameOverUI.SetActive(!isRankingVisible); // GameOverUIの表示切り替え
        RankingUI.SetActive(isRankingVisible);   // ランキングUIの表示切り替え
    }
}
