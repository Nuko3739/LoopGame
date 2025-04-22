using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNode
{
    public int Score;       // スコア
    public ScoreNode Next;  // 次のノードへのリンク

    public ScoreNode(int score)
    {
        Score = score;
        Next = null;
    }
}
