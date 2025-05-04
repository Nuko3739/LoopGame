using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowAttack : MonoBehaviour
{
    [Header("�|��֘A�̐ݒ�")]
    public GameObject ArrowPrefab;        // ���˂����̃v���n�u
    public Transform ShootPoint;          // ��𔭎˂���ʒu
    public float ArrowSpeed = 10f;         // ��̃X�s�[�h
    public float BowAttackCooldown = 0.5f; // �|�̃N�[���_�E������

    private bool isCanShoot = true;        // �|�����Ă邩�ǂ���

    private Player player;                 // �v���C���[�{�̎Q�Ɨp

    private void Start()
    {
        // �����I�u�W�F�N�g�ɂ���Player�X�N���v�g���擾
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && isCanShoot && !player.IsDodging())
        {
            StartCoroutine(ShootArrow());
        }
    }

    private IEnumerator ShootArrow()
    {
        isCanShoot = false;

        // ��𐶐�
        GameObject arrow = Instantiate(ArrowPrefab, ShootPoint.position, Quaternion.identity);

        // ��̌������v���C���[�̌����ɍ��킹��
        Vector2 arrowDirection = player.IsFacingRight() ? Vector2.right : Vector2.left;
        arrow.GetComponent<Rigidbody2D>().velocity = arrowDirection * ArrowSpeed;

        // �N�[���_�E��
        yield return new WaitForSeconds(BowAttackCooldown);
        isCanShoot = true;
    }

   
}
