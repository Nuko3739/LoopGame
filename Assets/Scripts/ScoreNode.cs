using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNode
{
    public int Score;       // �X�R�A
    public ScoreNode Next;  // ���̃m�[�h�ւ̃����N

    public ScoreNode(int score)
    {
        Score = score;
        Next = null;
    }
}
