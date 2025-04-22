using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScoreLinkedList
{
    private ScoreNode head; // リストの先頭ノード

    public ScoreLinkedList()
    {
        head = null;
    }

    // スコアをランキングに追加（降順に挿入）
    public void AddScore(int score)
    {
        Debug.Log($"Adding score to linked list: {score}"); // 確認用ログ
        ScoreNode newNode = new ScoreNode(score);

        // リストが空の場合、先頭に追加
        if (head == null || head.Score < score)
        {
            newNode.Next = head;
            head = newNode;
            return;
        }

        // 適切な位置に挿入（降順ソート）
        ScoreNode current = head;
        while (current.Next != null && current.Next.Score >= score)
        {
            current = current.Next;
        }

        newNode.Next = current.Next;
        current.Next = newNode;
    }

    // ランキングをPlayerPrefsに保存
    public void SaveToPlayerPrefs()
    {
        int index = 0;
        ScoreNode current = head;

        while (current != null && index < 10)
        {
            PlayerPrefs.SetInt($"Ranking_{index}", current.Score);
            current = current.Next;
            index++;
        }

        PlayerPrefs.Save();
    }

    // ランキングをPlayerPrefsから読み込む
    public void LoadFromPlayerPrefs()
    {
        head = null;

        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey($"Ranking_{i}"))
            {
                AddScore(PlayerPrefs.GetInt($"Ranking_{i}"));
            }
        }
    }

    // ランキングを文字列としてフォーマット（UI表示用）
    public string GetFormattedRanking()
    {
        StringBuilder ranking = new StringBuilder("SCORE RANKING\n");
        ScoreNode current = head;
        int rank = 1;

        while (current != null && rank <= 10)
        {
            // 順位とスコアの間にタブを挿入
            ranking.AppendLine($"{rank,2}位    {current.Score:D7}");
            current = current.Next;
            rank++;
        }

        return ranking.ToString();
    }
}