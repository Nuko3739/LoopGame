using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    public int Score; // 表示するスコア
    public Text ScoreText; // スコアを表示するテキスト
    //public ScoreManager ScoreManager; // スコア管理クラスへの参照
    public ScoreManager ScoreManager; // Unityエディタで直接アサイン

    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefsからスコアを取得
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        // テキストにスコアを表示
        ScoreText.text = $"Score: {finalScore}";
    }

    // Update is calledA once per frame
    void Update()
    {
        if (ScoreManager != null)
        {
            Score = ScoreManager.GetScore(); // ScoreManagerから現在のスコアを取得
            ScoreText.text = $"Score: {Score}"; // スコアをテキストに表示
        }
    }
}
