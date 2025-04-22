using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    private ScoreLinkedList rankingList = new ScoreLinkedList(); // �����N���X�g
    public Text RankingText;        // �����L���O�\���p��UI
    public GameObject GameOverUI;   // GameOver�\���p��UI
    public GameObject RankingUI;    // �����L���O�\���p��UI

    private bool isRankingVisible = false; // �����L���O�\���t���O

    private void Start()
    {
        rankingList.LoadFromPlayerPrefs(); // �����L���O��ǂݍ���
        UpdateRankingUI(); // �����L���O��UI�ɕ\��

        // �����\���ݒ�
        GameOverUI.SetActive(true);
        RankingUI.SetActive(false);
    }

    private void Update()
    {
        // Enter�L�[��UI��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleRankingUI();
        }
    }

    // �V�����X�R�A�������L���O�ɒǉ�
    public void AddNewScore(int score)
    {
        Debug.Log("AddNewScore �ɓn���ꂽ�X�R�A: " + score); // �m�F�p
        rankingList.AddScore(score); // �����N���X�g�ɃX�R�A��ǉ�
        rankingList.SaveToPlayerPrefs(); // �����L���O��ۑ�
        UpdateRankingUI();
        Debug.Log("�����L���O�̕ۑ����������܂����B");
    }

    // �����L���OUI���X�V
    private void UpdateRankingUI()
    {
        if (RankingText != null)
        {
            RankingText.text = rankingList.GetFormattedRanking();
        }
    }

    // UI�\���̐؂�ւ�
    private void ToggleRankingUI()
    {
        isRankingVisible = !isRankingVisible;

        GameOverUI.SetActive(!isRankingVisible); // GameOverUI�̕\���؂�ւ�
        RankingUI.SetActive(isRankingVisible);   // �����L���OUI�̕\���؂�ւ�
    }
}
