using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("�X�R�A�֘A")]
    public Text ScoreText; // �X�R�A�\���p��UI�iUnity Inspector�Őݒ肷��j
    private int CurrentScore = 0; // �X�R�A�̏����l�i�����l��0�j

    [Header("�^�C�}�[�֘A")]
    public Text TimeText; // �^�C���\���p��UI�iUnity Inspector�Őݒ肷��j
    private float timer = 0f; // �^�C�}�[�̏����l
    private bool isTiming = true; // �^�C�}�[���i�s�����ǂ������Ǘ�����t���O

    // ���t���[�����s����鏈��
    private void Update()
    {
        // �^�C�}�[���L���ȏꍇ
        if (isTiming)
        {
            timer += Time.deltaTime; // ���Ԃ��o�ߕ��������Z
            UpdateTimeUI(); // �^�C���\�����X�V
        }
    }

    // �X�R�A���w�肵���l�������Z���郁�\�b�h
    public void AddScore(int amount)
    {
        CurrentScore += amount; // �X�R�A�����Z
        UpdateScoreUI(); // �X�R�A�\�����X�V
    }

    // �^�C���{�[�i�X���v�Z���ăX�R�A�ɉ��Z���郁�\�b�h
    public void AddTimeBonus()
    {
        int timeBonus = CalculateTimeBonus(); // ���݂̃^�C���ɉ������{�[�i�X���v�Z
        CurrentScore += timeBonus; // �^�C���{�[�i�X���X�R�A�ɉ��Z
        UpdateScoreUI(); // �X�R�A�\�����X�V
        Debug.Log($"Time Bonus: {timeBonus}"); // �^�C���{�[�i�X�̒l�����O�ɕ\��
    }

    // �^�C�}�[�����Z�b�g���郁�\�b�h
    public void ResetTimer()
    {
        timer = 0f; // �^�C�}�[��0�Ƀ��Z�b�g
        UpdateTimeUI(); // �^�C���\�����X�V
    }

    // ���݂̃^�C���Ɋ�Â��ă^�C���{�[�i�X���v�Z���郁�\�b�h
    private int CalculateTimeBonus()
    {
        // �^�C�}�[�̒l�ɉ����ă{�[�i�X���v�Z���ĕԂ�
        if (timer <= 20f) return 10000; // 20�b�ȉ��̏ꍇ
        if (timer <= 30f) return 5000;  // 30�b�ȉ��̏ꍇ
        if (timer <= 60f) return 2500;  // 60�b�ȉ��̏ꍇ
        return 1000;                   // ����ȏ�̏ꍇ
    }

    // �X�R�A�\�����X�V���郁�\�b�h
    private void UpdateScoreUI()
    {
        // �X�R�A��UI���ݒ肳��Ă���ꍇ
        if (ScoreText != null)
        {
            // �X�R�A���u7���̃[�����߁v�`���ŕ\��
            ScoreText.text = $"SCORE: {CurrentScore:D7}";
        }
    }

    // �^�C���\�����X�V���郁�\�b�h
    private void UpdateTimeUI()
    {
        // �^�C����UI���ݒ肳��Ă���ꍇ
        if (TimeText != null)
        {
            // ���A�b�A�����_�ȉ����v�Z
            int minutes = Mathf.FloorToInt(timer / 60f); // �^�C�}�[��60�Ŋ��������������𕪂Ƃ���
            int seconds = Mathf.FloorToInt(timer % 60f); // �^�C�}�[��60�Ŋ������]���b�Ƃ���
            int milliseconds = Mathf.FloorToInt((timer * 100) % 100); // �^�C�}�[��100�{���ė]��������_�ȉ��̒l�Ƃ���

            // �v�Z���ʂ��t�H�[�}�b�g���ă^�C��UI�ɔ��f
            TimeText.text = $"TIME: {minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }
}