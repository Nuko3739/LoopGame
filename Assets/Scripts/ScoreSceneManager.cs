using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSceneManager : MonoBehaviour
{
    private int currentScore = 0; // ���݂̃X�R�A��ێ�

    private void Start()
    {
        // �V�[�����ׂ��ŃI�u�W�F�N�g��ێ�
        DontDestroyOnLoad(gameObject);

        // �V�[���؂�ւ����̃C�x���g�o�^
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �C�x���g�o�^����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �X�R�A�����̃V�[���Ɉ����p��
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���̃V�[����ScoreManager��T��
        var scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            // ScoreManager�ɃX�R�A�������p��
            scoreManager.AddScore(currentScore);
        }
    }

    // �X�R�A���X�V���郁�\�b�h
    public void UpdateScore(int score)
    {
        currentScore = score;
    }

    // �X�R�A�����Z�b�g���郁�\�b�h�i�K�v�Ȃ�ǉ��j
    public void ResetScore()
    {
        currentScore = 0;
    }
}
