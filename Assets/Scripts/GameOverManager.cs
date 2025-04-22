using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverManager : MonoBehaviour
{
    [Header("�X�R�A�\��")]
    public Text FinalScoreText; // �Q�[���I�[�o�[��ʂɃX�R�A��\������UI�e�L�X�g

    [Header("���j���[����")]
    public GameObject[] MenuItems; // Game Over��ʂ̃��j���[���ځiRestart, Ranking, Title�j
    private int currentIndex = 0;  // ���ݑI������Ă��郁�j���[�̃C���f�b�N�X
    public Color SelectedColor = Color.yellow; // �I�𒆂̃��j���[���ڂ̐F
    public Color DefaultColor = Color.white;   // ��I����Ԃ̃��j���[���ڂ̐F

    [Header("UI�v�f")]
    public GameObject GameOverUI; // Game Over��ʑS�̂�UI�O���[�v
    public GameObject RankingUI;  // �����L���O��ʑS�̂�UI�O���[�v
    public Text RankingText;      // �����L���O���e��\������e�L�X�g

    private bool isRankingVisible = false; // ���݃����L���O��ʂ��\������Ă��邩�̃t���O

    [Header("�V�[����")]
    public string GameSceneName = "GameScene"; // ���X�^�[�g���ɓǂݍ��ރQ�[���V�[����
    public string TitleSceneName = "TitleScene"; // �^�C�g���ɖ߂�ۂ̃V�[����

    private void Start()
    {
        // PlayerPrefs ����ۑ����ꂽ�ŏI�X�R�A���擾���ĕ\��
        if (FinalScoreText != null)
        {
            int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // �X�R�A�̎擾�i�f�t�H���g�l: 0�j
            FinalScoreText.text = $"FINAL SCORE: {finalScore:D7}"; // �X�R�A��UI�ɔ��f
        }
        else
        {
            Debug.LogError("FinalScoreText ���A�^�b�`����Ă��܂���I"); // �e�L�X�g�������N����Ă��Ȃ��ꍇ
        }

        // �����\���ݒ�FGameOverUI��\�����ARankingUI���\���ɂ���
        GameOverUI.SetActive(true);
        RankingUI.SetActive(false);

        // ������ԂŃ��j���[UI�̍X�V���s��
        UpdateMenuUI();
    }

    private void Update()
    {
        if (!isRankingVisible)
        {
            // Game Over���j���[�̓��͏���
            HandleInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // �����L���O��ʕ\������Enter��������GameOver��ʂɖ߂�
            ToggleRankingUI(false);
        }
    }

    /// <summary>
    /// ���[�U�[�̓��́i���L�[��Enter�L�[�j����������
    /// </summary>
    private void HandleInput()
    {
        // ����L�[�őI���C���f�b�N�X������
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = MenuItems.Length - 1; // ���[�v����
            UpdateMenuUI(); // ���j���[��UI�X�V
        }

        // �����L�[�őI���C���f�b�N�X�𑝉�
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= MenuItems.Length) currentIndex = 0; // ���[�v����
            UpdateMenuUI(); // ���j���[��UI�X�V
        }

        // Enter�L�[�őI�����ꂽ�A�N�V���������s
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteAction();
        }
    }

    /// <summary>
    /// ���j���[UI�̑I����Ԃ��X�V����
    /// </summary>
    private void UpdateMenuUI()
    {
        for (int i = 0; i < MenuItems.Length; i++)
        {
            var text = MenuItems[i].GetComponent<Text>(); // ���j���[���ڂ�Text�R���|�[�l���g���擾
            if (text != null)
            {
                // �I�𒆂̍��ڂ͉��F�A��I���̍��ڂ͔��F�ɐݒ�
                text.color = (i == currentIndex) ? SelectedColor : DefaultColor;
            }
        }
    }

    /// <summary>
    /// �I������Ă��郁�j���[���ڂɉ����ăA�N�V���������s����
    /// </summary>
    private void ExecuteAction()
    {
        string selectedMenu = MenuItems[currentIndex].name; // ���ݑI�𒆂̃��j���[�����擾

        switch (selectedMenu)
        {
            case "Restart": // Restart���I�����ꂽ�ꍇ
                SceneManager.LoadScene(GameSceneName); // �Q�[���V�[���Ƀ��X�^�[�g
                break;
            case "Ranking": // Ranking���I�����ꂽ�ꍇ
                ToggleRankingUI(true); // �����L���O��ʂ�\������
                break;
            case "Title": // Title���I�����ꂽ�ꍇ
                SceneManager.LoadScene(TitleSceneName); // �^�C�g���V�[���Ɉړ�
                break;
            default: // �z��O�̍��ڂ̏ꍇ
                Debug.LogWarning("Unknown menu item selected: " + selectedMenu);
                break;
        }
    }

    /// <summary>
    /// GameOverUI��RankingUI�̕\����؂�ւ���
    /// </summary>
    /// <param name="showRanking">�����L���O��ʂ�\�����邩�ǂ���</param>
    private void ToggleRankingUI(bool showRanking)
    {
        isRankingVisible = showRanking;

        // GameOverUI���\���ARankingUI��\���i�܂��͂��̋t�j
        GameOverUI.SetActive(!showRanking);
        RankingUI.SetActive(showRanking);

        if (showRanking)
        {
            UpdateRankingUI(); // �����L���OUI���X�V
        }
    }

    /// <summary>
    /// �����L���OUI�̓��e���X�V���ĕ\������
    /// </summary>
    private void UpdateRankingUI()
    {
        if (RankingText != null)
        {
            System.Text.StringBuilder rankingBuilder = new System.Text.StringBuilder("SCORE RANKING\n");
            for (int i = 0; i < 10; i++) // ���10�ʂ܂ŕ\��
            {
                int score = PlayerPrefs.GetInt($"Ranking_{i}", 0); // �ۑ�����Ă���X�R�A���擾
                rankingBuilder.AppendLine($"{i + 1}�ʁ@{score:D7}");
            }
            RankingText.text = rankingBuilder.ToString(); // �e�L�X�g�ɔ��f
        }
        else
        {
            Debug.LogError("RankingText ���A�^�b�`����Ă��܂���I");
        }
    }
}