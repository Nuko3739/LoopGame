using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("���ʐݒ�")]
    public int EnemyHp = 2; // �G��HP

    protected SpriteRenderer spriteRenderer; // �X�v���C�g�����_���[�̎Q��
    protected Rigidbody2D rb; // Rigidbody2D�̎Q��
    protected bool isFacingRight = true; // �G���E�������Ă��邩�ǂ����̃t���O
    protected bool isAlive = true; // �G���������Ă��邩�ǂ����̃t���O

    // �����ݒ胁�\�b�h
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D���擾
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer���擾

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer���ݒ肳��Ă��܂���I");
        }
    }

    // �_���[�W���󂯂鏈��
    public virtual void TakeDamage(int damage)
    {
        if (!isAlive) return; // �G�����Ɏ��S���Ă���ꍇ�͏������Ȃ�

        EnemyHp -= damage; // HP������������
        StartCoroutine(FlashDamage()); // �_���[�W���󂯂��ۂ̓_�ŃG�t�F�N�g

        if (EnemyHp <= 0) // HP��0�ȉ��ɂȂ����ꍇ
        {
            Die(); // �G�����S������
        }
    }

    // �_���[�W���󂯂��ۂ̓_�ŃG�t�F�N�g
    protected IEnumerator FlashDamage()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("spriteRenderer��null�ł��BSpriteRenderer���������ݒ肳��Ă��邩�m�F���Ă��������B");
            yield break; // �����𒆒f
        }
        spriteRenderer.color = Color.blue;�@�@// �_���[�W���󂯂��ۂɐ��_��
        yield return new WaitForSeconds(0.1f);// 0.1�b�ҋ@
        spriteRenderer.color = Color.white;   // ���̐F�ɖ߂�



        //spriteRenderer.color = Color.red; // �_���[�W���󂯂��ۂɐԂ��_��
        //yield return new WaitForSeconds(0.1f); // 0.1�b�ҋ@
        //spriteRenderer.color = Color.white; // ���̐F�ɖ߂�
    }

    // ���S����
    protected virtual void Die()
    {
        isAlive = false; // �G�����S��ԂɂȂ�
        Destroy(gameObject); // �G�̃Q�[���I�u�W�F�N�g��j�󂷂�
    }

    // �����𔽓]�����郁�\�b�h
    protected void Flip()
    {
        isFacingRight = !isFacingRight; // �����̃t���O�𔽓]
        Vector3 scale = transform.localScale;
        scale.x *= -1; // �X�v���C�g��X���̃X�P�[���𔽓]
        transform.localScale = scale; // ���]�����X�P�[����K�p
    }
}
