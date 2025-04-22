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

    [Header("�����L���O�֘A")]
    private List<int> RankingSores = new List<int>();//�����L���O�p�̃X�R�A���X�g
    [Header("�Q�[���I�[�o�[�֘A")]
    public string GameOverSceneName = "GameOver"; // �Q�[���I�[�o�[�V�[���̖��O


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



    // �X�R�A���w�肵���l�����y���Z�z���ĕۑ����郁�\�b�h
    //�����ł�score�𑦍��ɕۑ�����B
    public void AddScore(int amount)
    {
        CurrentScore += amount; // �X�R�A�����Z
        PlayerPrefs.SetInt("FinalScore", CurrentScore); // �X�R�A��PlayerPrefs�ɕۑ�
        PlayerPrefs.Save(); // �ۑ��𔽉f

        //PlayerPrefs�c�ۑ����������̂��Q�[�����I�����Ă��ۑ����邽�߂̂���
    }



    public int GetScore()
    {
        return CurrentScore;//���݂̃X�R�A��Ԃ�
    }



    // �Q�[���I�[�o�[���ɌĂяo���čŏI�X�R�A��PlayerPrefs�ɕۑ�
    public void SaveFinalScore()
    {
        PlayerPrefs.SetInt("FinalScore", CurrentScore); // �X�R�A��ۑ�
        PlayerPrefs.Save(); // �ۑ��𔽉f
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




    ////�X�R�A��ranking�ɒǉ����A�����L���O���X�V
    //public void UpdateRanking()
    //{
    //    //�����L���O��ǂݍ���
    //    LoadRanking();

    //    //���݂̃X�R�A��ranking�ɒǉ�
    //    RankingSores.Add(CurrentScore);

    //    //�����L���O���~���Ƀ\�[�g�i�����X�R�A�D��j
    //    RankingSores.Sort((a, b) => b.CompareTo(a));

    //    //�����L���O����ʂT�ʂ܂łɐ���
    //    if(RankingSores.Count > 5)
    //    {
    //        RankingSores.RemoveRange(5, RankingSores.Count - 5);
    //    }

    //    //�X�V���������L���O��ێ�
    //    SaveRanking();
    //}

    ////�����L���O��ۑ����郁�\�b�h
    //private void SaveRanking()
    //{
    //    for(int i = 0;i < RankingSores.Count; i++)
    //    {
    //        PlayerPrefs.SetInt($"Ranking_{i}",RankingSores[i]);
    //    }
    //    PlayerPrefs.Save();
    //}

    //// �����L���O��ǂݍ��ރ��\�b�h
    //private void LoadRanking()
    //{
    //    RankingSores.Clear();
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (PlayerPrefs.HasKey($"Ranking_{i}"))
    //        {
    //            RankingSores.Add(PlayerPrefs.GetInt($"Ranking_{i}"));
    //        }
    //    }
    //}

    //// �����L���O��\���p�̃t�H�[�}�b�g�ɕϊ����郁�\�b�h
    //public string GetFormattedRanking()
    //{
    //    string formattedRanking = "RANKING:\n";
    //    for (int i = 0; i < RankingSores.Count; i++)
    //    {
    //        formattedRanking += $"{i + 1}. {RankingSores[i]:D7}\n";
    //    }
    //    return formattedRanking;
    //}

    private void OnDestroy()
    {
        // �I�u�W�F�N�g���j�������Ƃ��ɃX�R�A��ۑ�
        PlayerPrefs.SetInt("FinalScore", CurrentScore);
    }


    // �X�R�A��ۑ����ăQ�[���I�[�o�[�V�[���֑J��
    public void HandleGameOver()
    {
        PlayerPrefs.SetInt("FinalScore", CurrentScore); // ���݂̃X�R�A��ۑ�
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameOverSceneName); // �Q�[���I�[�o�[�V�[���ɑJ��
    }
}