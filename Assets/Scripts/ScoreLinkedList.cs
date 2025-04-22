using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScoreLinkedList
{
    private ScoreNode head; // ���X�g�̐擪�m�[�h

    public ScoreLinkedList()
    {
        head = null;
    }

    // �X�R�A�������L���O�ɒǉ��i�~���ɑ}���j
    public void AddScore(int score)
    {
        Debug.Log($"Adding score to linked list: {score}"); // �m�F�p���O
        ScoreNode newNode = new ScoreNode(score);

        // ���X�g����̏ꍇ�A�擪�ɒǉ�
        if (head == null || head.Score < score)
        {
            newNode.Next = head;
            head = newNode;
            return;
        }

        // �K�؂Ȉʒu�ɑ}���i�~���\�[�g�j
        ScoreNode current = head;
        while (current.Next != null && current.Next.Score >= score)
        {
            current = current.Next;
        }

        newNode.Next = current.Next;
        current.Next = newNode;
    }

    // �����L���O��PlayerPrefs�ɕۑ�
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

    // �����L���O��PlayerPrefs����ǂݍ���
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

    // �����L���O�𕶎���Ƃ��ăt�H�[�}�b�g�iUI�\���p�j
    public string GetFormattedRanking()
    {
        StringBuilder ranking = new StringBuilder("SCORE RANKING\n");
        ScoreNode current = head;
        int rank = 1;

        while (current != null && rank <= 10)
        {
            // ���ʂƃX�R�A�̊ԂɃ^�u��}��
            ranking.AppendLine($"{rank,2}��    {current.Score:D7}");
            current = current.Next;
            rank++;
        }

        return ranking.ToString();
    }
}