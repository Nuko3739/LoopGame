using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    [Header("スコア表示")]
    public Text FinalScoreText; // ゲームオーバー画面にスコアを表示するUIテキスト

    [Header("メニュー項目")]
    public GameObject[] MenuItems; // Game Over画面のメニュー項目（Restart, Ranking, Title）
    private int currentIndex = 0;  // 現在選択されているメニューのインデックス
    public Color SelectedColor = Color.yellow; // 選択中のメニュー項目の色
    public Color DefaultColor = Color.white;   // 非選択状態のメニュー項目の色

    [Header("UI要素")]
    public GameObject GameOverUI; // Game Over画面全体のUIグループ
    public GameObject RankingUI;  // ランキング画面全体のUIグループ
    public Text RankingText;      // ランキング内容を表示するテキスト

    private bool isRankingVisible = false; // 現在ランキング画面が表示されているかのフラグ

    [Header("シーン名")]
    public string GameSceneName = "GameScene"; // リスタート時に読み込むゲームシーン名
    public string TitleSceneName = "TitleScene"; // タイトルに戻る際のシーン名

    private void Start()
    {
        // PlayerPrefs から保存された最終スコアを取得して表示
        if (FinalScoreText != null)
        {
            int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // スコアの取得（デフォルト値: 0）
            FinalScoreText.text = $"FINAL SCORE: {finalScore:D7}"; // スコアをUIに反映
        }
        else
        {
            Debug.LogError("FinalScoreText がアタッチされていません！"); // テキストがリンクされていない場合
        }

        // 初期表示設定：GameOverUIを表示し、RankingUIを非表示にする
        GameOverUI.SetActive(true);
        RankingUI.SetActive(false);

        // 初期状態でメニューUIの更新を行う
        UpdateMenuUI();
    }

    private void Update()
    {
        if (!isRankingVisible)
        {
            // Game Overメニューの入力処理
            HandleInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // ランキング画面表示中にEnterを押すとGameOver画面に戻る
            ToggleRankingUI(false);
        }
    }

    /// <summary>
    /// ユーザーの入力（矢印キーとEnterキー）を処理する
    /// </summary>
    private void HandleInput()
    {
        // 上矢印キーで選択インデックスを減少
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = MenuItems.Length - 1; // ループ処理
            UpdateMenuUI(); // メニューのUI更新
        }

        // 下矢印キーで選択インデックスを増加
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= MenuItems.Length) currentIndex = 0; // ループ処理
            UpdateMenuUI(); // メニューのUI更新
        }

        // Enterキーで選択されたアクションを実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteAction();
        }
    }

    /// <summary>
    /// メニューUIの選択状態を更新する
    /// </summary>
    private void UpdateMenuUI()
    {
        for (int i = 0; i < MenuItems.Length; i++)
        {
            var text = MenuItems[i].GetComponent<Text>(); // メニュー項目のTextコンポーネントを取得
            if (text != null)
            {
                // 選択中の項目は黄色、非選択の項目は白色に設定
                text.color = (i == currentIndex) ? SelectedColor : DefaultColor;
            }
        }
    }

    /// <summary>
    /// 選択されているメニュー項目に応じてアクションを実行する
    /// </summary>
    private void ExecuteAction()
    {
        string selectedMenu = MenuItems[currentIndex].name; // 現在選択中のメニュー名を取得

        switch (selectedMenu)
        {
            case "Restart": // Restartが選択された場合
                SceneManager.LoadScene(GameSceneName); // ゲームシーンにリスタート
                break;
            case "Ranking": // Rankingが選択された場合
                ToggleRankingUI(true); // ランキング画面を表示する
                break;
            case "Title": // Titleが選択された場合
                SceneManager.LoadScene(TitleSceneName); // タイトルシーンに移動
                break;
            default: // 想定外の項目の場合
                Debug.LogWarning("Unknown menu item selected: " + selectedMenu);
                break;
        }
    }

    /// <summary>
    /// GameOverUIとRankingUIの表示を切り替える
    /// </summary>
    /// <param name="showRanking">ランキング画面を表示するかどうか</param>
    private void ToggleRankingUI(bool showRanking)
    {
        isRankingVisible = showRanking;

        // GameOverUIを非表示、RankingUIを表示（またはその逆）
        GameOverUI.SetActive(!showRanking);
        RankingUI.SetActive(showRanking);

        if (showRanking)
        {
            UpdateRankingUI(); // ランキングUIを更新
        }
    }

    /// <summary>
    /// ランキングUIの内容を更新して表示する
    /// </summary>
    private void UpdateRankingUI()
    {
        if (RankingText != null)
        {
            System.Text.StringBuilder rankingBuilder = new System.Text.StringBuilder("SCORE RANKING\n");
            for (int i = 0; i < 10; i++) // 上位10位まで表示
            {
                int score = PlayerPrefs.GetInt($"Ranking_{i}", 0); // 保存されているスコアを取得
                rankingBuilder.AppendLine($"{i + 1}位　{score:D7}");
            }
            RankingText.text = rankingBuilder.ToString(); // テキストに反映
        }
        else
        {
            Debug.LogError("RankingText がアタッチされていません！");
        }
    }
}