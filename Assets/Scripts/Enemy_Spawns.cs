using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawns : MonoBehaviour
{
    [Header("�����\��Enemy�̃��X�g")]
    public List<GameObject> EnemyPrefabs; // �v���n�u�����ꂽEnemy���X�g
    public Collider2D SpawnArea;          // Enemy�𐶐��\�Ȕ͈�

    // �����_����Enemy�𐶐�����
    public EnemyData SpawnRandomEnemy()
    {
        if (EnemyPrefabs.Count == 0 || SpawnArea == null)
        {
            Debug.LogError("EnemyPrefab�܂���SpawnArea���ݒ肳��Ă��܂���I");
            return null;
        }

        // �����ʒu�������_���Ō���
        Vector2 spawnPosition = new Vector2(
            Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x),//�����������_���ݒ�
            Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y)// �c���������_���ݒ�
        );

        // Enemy�������_���ɑI�����Đ���
        GameObject selectedEnemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];
        GameObject spawnedEnemy = Instantiate(selectedEnemyPrefab, spawnPosition, Quaternion.identity);

        Debug.Log($"Enemy����: {spawnedEnemy.name} at {spawnPosition}");

        // ��������ۑ�
        return new EnemyData
        {
            EnemyPrefab = selectedEnemyPrefab,
            Position = spawnPosition
        };
    }
}

// Enemy�̐�������ێ�����\����
[System.Serializable]
public class EnemyData
{
    public GameObject EnemyPrefab; // ��������Enemy�̃v���n�u
    public Vector2 Position;       // �����ʒu
}
