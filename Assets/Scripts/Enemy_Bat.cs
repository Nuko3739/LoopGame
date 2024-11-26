using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : EnemyBase
{
    public float Enemy_MoveSpeed = 2.0f;       // Bat�̈ړ����x
    public float Enemy_LeftBoundary = -5.0f;  // ���[�̈ʒu
    public float Enemy_RightBoundary = 5.0f;  // �E�[�̈ʒu
    public float MaxRiseFallHeight = 3.5f;    // �㉺�^���̍ő�U��
    public float VerticalSpeed = 2.0f;        // �㉺�^���̑��x

    private bool Enemy_MovingRight = true;    // ���݂̈ړ��������E���ǂ���
    private float Enemy_Initial_Y;           // ������Y�ʒu
    private float phaseOffset;               // �㉺�^���̃����_���ʑ��I�t�Z�b�g
    //�@�@�@�@�@�@�@�@�@��Enemy���㉺����^�C�~���O�����ꂼ�ꂸ�炷����

    private void Start()
    {
        base.Start(); // EnemyBase��Start()���\�b�h���Ăяo��
        Enemy_Initial_Y = transform.position.y; // ����Y�ʒu��ۑ�
        phaseOffset = Random.Range(0f, Mathf.PI * 2); // �����_���ʑ��I�t�Z�b�g�𐶐�
    }

    // ���t���[���Ăяo����郁�\�b�h
    private void Update()
    {
        if (isAlive) // �G���������Ă���ꍇ�̂�
        {
            PatrolWithVerticalMovement(); // ���E�ړ����㉺�^�����s��
        }
    }

    // ���E�ɔ����ړ����s���Ȃ���㉺�^���������郁�\�b�h
    private void PatrolWithVerticalMovement()
    {
        Vector2 newPosition = transform.position; // ���X�|�[���𓥂܂����X�|�[���ʒu���擾����ϐ�

        // ���������̈ړ�
        if (Enemy_MovingRight)
        {
            newPosition.x += Enemy_MoveSpeed * Time.deltaTime;

            if (newPosition.x >= Enemy_RightBoundary) // �E�[�ɓ��B
            {
                Enemy_MovingRight = false; // ���Ɉړ�����悤�ɐݒ�
                Flip(); // �����𔽓]
            }
        }
        else
        {
            newPosition.x -= Enemy_MoveSpeed * Time.deltaTime;

            if (newPosition.x <= Enemy_LeftBoundary) // ���[�ɓ��B
            {
                Enemy_MovingRight = true; // �E�Ɉړ�����悤�ɐݒ�
                Flip(); // �����𔽓]
            }
        }

        // �㉺�^�����v�Z�i�����_���Ȉʑ��I�t�Z�b�g��ǉ��j
        newPosition.y = Enemy_Initial_Y + Mathf.Sin(Time.time * VerticalSpeed + phaseOffset) * MaxRiseFallHeight;
        //Mathf.Sin�́i�T�C���֐��j�̒l���v�Z����̂Ɏg�p�A�o�b�g�̏㉺�h��𐧌�B

        // �v�Z�����V�����ʒu�Ɉړ�
        transform.position = newPosition;
    }
}