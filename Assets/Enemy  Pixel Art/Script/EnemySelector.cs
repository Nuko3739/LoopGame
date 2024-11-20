using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelector : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] allAnimationButtons; // すべてのアニメーションボタンのリスト
    [SerializeField] private Button[] enableAnimationButtons; // 有効化するアニメーションボタンのリスト

    [Header("Game Objects")]
    [SerializeField] private GameObject[] allEnemyGOs; // シーンに存在するすべての敵キャラクター（GameObjects）のリスト
    [SerializeField] private GameObject enableEnemy; // 現在有効化されている敵キャラクター（表示中のもの）

    [Header("Text")]
    [SerializeField] private Text enemyName; // 敵の名前を表示するテキストフィールド

    // 最初に実行されるメソッド
    public void Start()
    {
        enemyName.text = ""; // 初期化：敵の名前表示フィールドを空にする
    }

    // 敵を変更するときに呼び出されるメソッド
    public void ChangeEnemies()
    {
        _DisableAllEnemies(); // すべての敵キャラクターを非表示にする
        _DisableAllButtons(); // すべてのアニメーションボタンを無効にする
        _EnableEnemy(); // 選択された敵キャラクターを有効にする
        _EnableButton(); // アニメーションボタンを有効にする
        _Rename(); // 敵の名前を更新する
    }

    // 敵の名前を更新するメソッド
    public void _Rename()
    {
        enemyName.text = enableEnemy.gameObject.name; // 有効化された敵の名前をテキストフィールドに表示
    }

    // アニメーションボタンを有効にするメソッド
    public void _EnableButton()
    {
        for (int i = 0; i < enableAnimationButtons.Length; i++)
        {
            enableAnimationButtons[i].interactable = true; // 各アニメーションボタンを有効にする
        }
    }

    // すべてのアニメーションボタンを無効にするメソッド
    public void _DisableAllButtons()
    {
        for (int i = 0; i < allAnimationButtons.Length; i++)
        {
            allAnimationButtons[i].interactable = false; // 各アニメーションボタンを無効にする
        }
    }

    // 選択された敵キャラクターを有効にするメソッド
    public void _EnableEnemy()
    {
        enableEnemy.SetActive(true); // 現在選択されている敵を有効化（表示）
    }

    // すべての敵キャラクターを非表示にするメソッド
    public void _DisableAllEnemies()
    {
        for (int i = 0; i < allEnemyGOs.Length; i++)
        {
            allEnemyGOs[i].gameObject.SetActive(false); // すべての敵キャラクターを非表示にする
        }
    }
}
