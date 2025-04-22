using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    public int Score; // �\������X�R�A
    public Text ScoreText; // �X�R�A��\������e�L�X�g
    //public ScoreManager ScoreManager; // �X�R�A�Ǘ��N���X�ւ̎Q��
    public ScoreManager ScoreManager; // Unity�G�f�B�^�Œ��ڃA�T�C��

    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs����X�R�A���擾
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        // �e�L�X�g�ɃX�R�A��\��
        ScoreText.text = $"Score: {finalScore}";
    }

    // Update is calledA once per frame
    void Update()
    {
        if (ScoreManager != null)
        {
            Score = ScoreManager.GetScore(); // ScoreManager���猻�݂̃X�R�A���擾
            ScoreText.text = $"Score: {Score}"; // �X�R�A���e�L�X�g�ɕ\��
        }
    }
}
