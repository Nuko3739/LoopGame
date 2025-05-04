using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player; // Player�N���X�̎Q�Ƃ��擾

    [Header("�U���֘A�̐ݒ�")]
    public int Player_TotalAttackFrames = 7;        //player�U���A�j���[�V�����̑S�̃t���[����
    public int Player_ColliderEnableStartFrame = 3; //�R���C�_�[��L���ɂ���t���[��
    public int Player_ColliderEnableEndFrame = 4; �@//�R���C�_�[�𖳌��ɂ���t���[��

    public float AnimationFrameDuration = 0.033f;   // �t���[�����Ƃ̎��ԁi��Ƃ���30FPS�Ȃ��0.033�b�j

    public Collider2D AttackCollider;//�U������̃R���C�_�[
    public float AttackCooldown = 0.45f;//�U���̃N�[���_�E���^�C��

    public Animator animator;
    private bool isCanAttack = true;

    private int PlayerAttackLayerIndex;//�A�j���[�V�����̍U���p���C���[���擾���邽�߂̕ϐ�

    public int AnimationFrameDutation { get; private set; }

    private void Start()
    {
        //player���Q��
        player = GetComponent<Player>();

       //Animater�R���|�[�l���g�̃A�T�C����Y��Ȃ��悤�ɂ��܂�
       if (animator == null)
        {
            animator = GetComponent<Animator>();//�����ŃA�T�C������
        }

        // Player_Attack ���C���[�̃C���f�b�N�X���擾
        PlayerAttackLayerIndex = animator.GetLayerIndex("Player_Attack");

        // �U���R���C�_�[�𖳌��ɂ��Ă���
        AttackCollider.enabled = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && isCanAttack && !player.IsDodging()) // J�L�[�ōU��
        {
            StartCoroutine(Attack());//�U������
        }
    }

    private IEnumerator Attack()
    {
        isCanAttack = false; // �U���t���O��false�ɂ��ăN�[���_�E�����Ԓ��͍ēx�U���ł��Ȃ��悤�ɂ���
        animator.SetTrigger("AttackTrigger");

        //�����ł͂R�t���[���ڂ���U��������o�������̂ł���܂őҋ@������
        // �U������t���[���܂ő҂�
        yield return new WaitForSeconds(AnimationFrameDuration * Player_ColliderEnableStartFrame);
                                        //�t���[�����Ƃ̎��ԁ@�@//�R���C�_�[��L���ɂ��n�߂�t���[�����@�@�@

        //�R���C�_�[��L���ɂ���
        AttackCollider.enabled = true;//�R���C�_�[��L���ɂ���t���[�����ォ��R���C�_�[��L���ɂ���


        //���ˑ��֌W���Ђǂ��o�O�̉����ɂȂ��Ă���A�ҋ@���Ԃ����Ă��炳��ɑҋ@���Ԃ�p�ӂ��҂�����Ȃ炱��͂�߂��ق�������
        //StartCoroutine(EnableAttackCollider());


        //�U���R���C�_�[���L���ɂȂ��Ă��疳���ɂȂ�܂ł̊Ԃ̎��Ԃ��v�Z�i�����ł͂P�t���[�������U�������\�����Ă���j���đҋ@����
        //���̌��ł͍U���R���C�_�[�L�����Ԃł���@�y�U�����莞��(�PF�j���擾���đҋ@���Ă���z
        yield return new WaitForSeconds(AnimationFrameDuration * (Player_ColliderEnableEndFrame - Player_ColliderEnableStartFrame));

        //�R���C�_�[�𖳌��ɂ���
        AttackCollider.enabled = false;


        // �U���̃N�[���_�E�����J�n
        yield return new WaitForSeconds(AttackCooldown); // �N�[���_�E���̊�����҂�
        isCanAttack = true; // �N�[���_�E���I����ɍēx�U���\�ɂ���
    }

    // �G�Ƀ_���[�W��^���鏈��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // AttackCollider ���L���ŁA�Փ˂����I�u�W�F�N�g�� "Enemy" �^�O�������Ă���ꍇ�̂ݏ�������
        if (AttackCollider.enabled && collision.CompareTag("Enemy") && collision.gameObject.tag != "Enemy_Search")
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Enemy�Ƀ_���[�W��^����i�_���[�W�l�͉���1�j
                Debug.Log("Enemy Hp:" + enemy.EnemyHp); // EnemyHp�̊m�F�p
            }
        }
    }
}
