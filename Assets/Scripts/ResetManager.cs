using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [Header("���[�v�����֘A")]
    public ScoreManager ScoreManager; // ScoreManager�ւ̎Q�ƁiInspector�Őݒ�j
    public Transform RestartPosition; // �v���C���[��߂��ʒu
    public string PlayerTag = "Player"; // �v���C���[�̃^�O

    [Header("Enemy�����֘A")]
    public List<GameObject> EnemyPrefabs; // �X�|�[������Enemy�̃v���n�u���X�g
    public BoxCollider2D SpawnArea;       // �X�|�[���͈́iBoxCollider2D�j
    public int EnemiesPerLoop = 5;        // �e���[�v�Ő�������Enemy�̐�

    private List<GameObject> SpawnedEnemies = new List<GameObject>(); // ��������Enemy��ێ�

    private void Start()
    {
        if (RestartPosition == null)
        {
            Debug.LogError("RestartPosition���ݒ肳��Ă��܂���I");
        }

        if (EnemyPrefabs == null || EnemyPrefabs.Count == 0)
        {
            Debug.LogError("EnemyPrefabs���ݒ肳��Ă��܂���I");
        }

        if (SpawnArea == null)
        {
            Debug.LogError("SpawnArea���ݒ肳��Ă��܂���I");
        }
    }

    //���[�v�����J�n
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag)) // �v���C���[�̂ݏ��������s
        {
            Debug.Log("�v���C���[���S�[���ɓ��B���܂����B");
            StartCoroutine(HandleStageEnd(collision));
        }
    }

    //�X�e�[�W�̃��[�v����
    private IEnumerator HandleStageEnd(Collider2D player)
    {
        // �t�F�[�h�A�E�g�����i���j
        //���݂̓R���[�`���őҋ@�A���̑҂��Ă鎞�Ԃ͓G�̓������~�߂邱�Ƃ�Y�ꂸ��
        //�@�@�@�@�@�@�@�@�@�@�@�@���Ԓ�~�͂܂���������
        yield return StartCoroutine(FadeOut());

        // �^�C���{�[�i�X���X�R�A�ɉ��Z
        ScoreManager.AddTimeBonus();

        // �v���C���[�����X�^�[�g�ʒu�Ɉړ�
        if (RestartPosition != null)
        {
            player.transform.position = RestartPosition.position;
            Debug.Log("�v���C���[�����X�^�[�g�ʒu�Ɉړ����܂����B");
        }
        else
        {
            Debug.LogError("RestartPosition���ݒ肳��Ă��Ȃ����߁A�v���C���[���ړ��ł��܂���ł����B");
        }

        // �^�C�}�[���Z�b�g
        ScoreManager.ResetTimer();
        // Enemy��������
        SpawnNewEnemies();
    }

    private IEnumerator FadeOut()
    {
        Debug.Log("�t�F�[�h�A�E�g��...");
        yield return new WaitForSeconds(2f); // �t�F�[�h�����̑�ւƂ���2�b�ҋ@
        Debug.Log("�t�F�[�h�A�E�g�I���B");
    }

    private void SpawnNewEnemies()
    {
        if (EnemyPrefabs == null || EnemyPrefabs.Count == 0)
        {
            Debug.LogError("EnemyPrefabs���ݒ肳��Ă��܂���B�X�|�[������Enemy��ݒ肵�Ă��������B");
            return;
        }

        if (SpawnArea == null)
        {
            Debug.LogError("SpawnArea���ݒ肳��Ă��܂���B�X�|�[���G���A��ݒ肵�Ă��������B");
            return;
        }

        //���݂͂T�̌Œ�
        Debug.Log($"Enemy��{EnemiesPerLoop}�̐������܂��B");

        // �X�e�[�W�̍����������擾
        float minHeight = SpawnArea.bounds.min.y; // �X�|�[���G���A�͈͂̉��[(�ŏ��l��Y���W)���擾
        float maxHeight = SpawnArea.bounds.max.y; // �X�|�[���G���A�͈͂̏�[(�ő�l��Y���W)���擾

        for (int i = 0; i < EnemiesPerLoop; i++)
        {
            // BoxCollider2D�͈̔͂���ɃX�|�[���ʒu�������_���ݒ�
            Vector2 spawnPosition = new Vector2(
                Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x),
                Random.Range(minHeight, maxHeight) // �����𐧌�
            );

            // Enemy�v���n�u�������_���ɑI��
            GameObject RandomEnemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];

            // Enemy�𐶐����ă��X�g�ɒǉ�
            GameObject newEnemy = Instantiate(RandomEnemyPrefab, spawnPosition, Quaternion.identity);
            //Quaternion.identity�͗p�r: �I�u�W�F�N�g�������ɓ���̉�]��ݒ�B�ł��̏ꍇ�͉�]��K�p���Ȃ�

            SpawnedEnemies.Add(newEnemy);

            Debug.Log($"Enemy����: {newEnemy.name} at {spawnPosition}");
        }
    }
}
