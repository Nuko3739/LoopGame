using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �v���C���[��ǐՂ���Crab�^�̓G�N���X
public class Enemy_Crab : EnemyBase
{
    public float Enemy_MoveSpeed = 3f; // �G�̈ړ����x
    public float Enemy_Player_X_Flip = 0.5f; // �v���C���[�Ƃ�X���̋��������̒l�ȏ�Ŕ��]
    public CircleCollider2D PlayerDetectionCollider; // �v���C���[�����m����T�[�N���R���C�_�[

    private Transform Player; // �v���C���[�̈ʒu���
    private bool isChasing = false; // �v���C���[�ǐՒ����ǂ����̃t���O

    // �����ݒ胁�\�b�h
    protected override void Start()
    {
        base.Start(); // EnemyBase��Start()���Ăяo���Ċ�{�ݒ���s��

        // �v���C���[��Transform���擾
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        isChasing = false; // ������Ԃł͒ǐՃ��[�h���I�t�ɂ���
    }

    // ���t���[���Ăяo����郁�\�b�h
    private void Update()
    {
        if (isChasing && isAlive) // �v���C���[��ǐՒ��Ő������Ă���ꍇ�̂�
        {
            ChasePlayer(); // �v���C���[��ǐ�
        }
        else
        {
            rb.velocity = Vector2.zero; // ���m�͈͊O�̏ꍇ�͒�~
        }
    }

    // �v���C���[�����m�͈͂ɓ������Ƃ��ɒǐՂ��J�n����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���m�p�̃R���C�_�[�݂̂ŒǐՔ�����s��
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("�v���C���[�����m�͈͂ɓ���܂���");
        }
    }

    // �v���C���[�����m�͈͂���o���ۂɒǐՂ��~���郁�\�b�h
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false; // �v���C���[�ǐՒ�~
            Debug.Log("�v���C���[�����m�͈͂���o�܂���");
        }
    }

    // �v���C���[��ǐՂ��郁�\�b�h
    private void ChasePlayer()
    {
        Vector2 direction = (Player.position - transform.position).normalized; // �v���C���[�ւ̕������v�Z
        rb.velocity = new Vector2(direction.x * Enemy_MoveSpeed, rb.velocity.y); // X�������̑��x��ݒ�

        FlipDirection(direction.x); // �v���C���[�̈ʒu�ɉ����ēG�̌����𔽓]
    }

    // �v���C���[�̈ʒu�ɉ����ēG�̌����𔽓]���郁�\�b�h
    private void FlipDirection(float directionX)
    {
        if ((directionX < 0 && isFacingRight) || (directionX > 0 && !isFacingRight)) // �v���C���[�̕����ɉ�����
        {
            Flip(); // �����𔽓]
        }
    }
}