using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player�̐ݒ�")]
    public int PlayerHp = 3;  // �v���C���[��HP
    //public int PlayerPower = 1; //player�̍U����


    [Header("�ړ��֘A�̐ݒ�")]
    public float MoveSpeed = 5f;    // �ړ����x
    public float MoveStep = 3f;     // ����X�e�b�v�̋���

    [Header("�W�����v�֘A�̐ݒ�")]
    public float JumpForce = 7f;    // �ő�W�����v��
    public float MinJumpForce = 2f; // �ŏ��W�����v��
    public float MaxJumpHoldTimm = 0.2f; // �W�����v�{�^���̍ő厞��

    [Header("�����֘A�̐ݒ�")]
    public float GravityScale = 1f; // �d�͂̋���
    public float AddGravityScale = 1.5f; //�W�����v��̏d�͂̉��Z

    private Rigidbody2D rb;           // 2D�����G���W����Rigidbody2D
    private SpriteRenderer spriteRenderer; // �X�v���C�g�����_���[�̎Q��
    private Animator animator; // �A�j���[�^�[�̎Q��


    private bool isGround;            // �W�����v�֘A�̒n�ʔ���t���O
    private bool isJumping;           // �W�����v�������肷��t���O
    private float JumpTimeCounter;    // �W�����v�{�^���������Ă��鎞��
    private bool isFacingRight = true; // �v���C���[�̌�����ǐՁi�f�t�H���g�͉E�����j
    private bool isInvincible = false; // ���G��Ԃ��ǂ����̃t���O
    //private bool isCanAttack = true;     // �U���ł��邩�ǂ����̃t���O

    public Transform RestartPosition;     //player���E�[�ɒ����Ă���߂�ꏊ
                                          //��Restart�ʒu��Transform���i�[����
    //[Header("�U���֘A�̐ݒ�")]
    //public Collider2D AttackCollider; // �U������̃R���C�_�[
    //public float AttackCooldown = 0.5f; // �U���̃N�[���_�E������

    [Header("�U����e�F���G���Ԃ̐ݒ�")]
    public float InvincibleDuration = 0.25f; // ���G���Ԃ̒���
    public float BlinkInterval = 0.000001f; // �_�ŊԊu

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D�i2D�Łj�̎擾
        animator = GetComponent<Animator>();  // �A�j���[�^�[�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();


        rb.gravityScale = GravityScale;// Inspector�Őݒ�ł���d�͂�Rigidbody2D�ɔ��f

        //Gool����
        //�^�O��Restart�����I�u�W�F�N�g���������āA����Transform���擾
        GameObject RestartObject = GameObject.FindGameObjectWithTag("Restart");
        if(RestartObject != null)
        {
            RestartPosition = RestartObject.transform;
        }
        else
        {�@�@�@�@�@�@�@//�m�F�p
            Debug.Log("Restart�̃I�u�W�F�N�g��������܂���");
        }

        //�U���R���C�_�[�������I�ɖ����ɂ���
        //AttackCollider.enabled = false;
    }

    private void Update()
    {
        // �v���C���[�̈ړ��������Ăяo��
        PlayerMove();

        //�󒆂ɂ���Ƃ��ɗ������x�𑬂߂�
        if(!isGround && rb.velocity.y < 0)
        {
            rb.gravityScale = GravityScale * AddGravityScale; // �������̏d�͂�����
        }
        else
        {
            rb.gravityScale = GravityScale; // �ʏ�̏d�͂ɖ߂�
        }


        ////�U������
        //if(Input.GetKeyDown(KeyCode.J) && isCanAttack)�@//J�L�[�ōU���i���j
        //{
        //    StartCoroutine(Attack());
        //}
    }

    ////�U������
    //private IEnumerator Attack()
    //{
    //    isCanAttack = false; // �U���t���O��false�ɂ��ăN�[���_�E�����Ԓ��͍ēx�U���ł��Ȃ��悤�ɂ���
    //    //�A�j���[�^�[�Ƀg���K�[���Z�b�g
    //    animator.SetTrigger("AttackTrigger");


    //    ////�R���[�`���ōU�������0.1�b�����L���ɂ���
    //    //StartCoroutine(EnableAttackCollider());
    //    //StartCoroutine(AttackCooldownCoroutine()); // �U���̃N�[���_�E�����J�n

    //    // 0.2�b�ҋ@���Ă���U�������L���ɂ���
    //    yield return new WaitForSeconds(0.2f);
    //    StartCoroutine(EnableAttackCollider());


    //    // �U���̃N�[���_�E�����J�n
    //    yield return StartCoroutine(AttackCooldownCoroutine()); // �N�[���_�E���̊�����҂�
    //    isCanAttack = true;//�N�[���_�E���I����ɍēx�U���\�ɂ���
    //}

    ////�U�������̃R���[�`���Ȃ�
    //private IEnumerator EnableAttackCollider()
    //{
    //    AttackCollider.enabled = true; //�U���R���C�_�[��L���ɂ���
    //    yield return new WaitForSeconds(0.1f); //0.1�b�ҋ@(�U�����莞�ԁj
    //    AttackCollider.enabled = false; //�U���R���C�_�[�𖳌��ɂ���
    //}

    //// �U���̃N�[���_�E������
    //private IEnumerator AttackCooldownCoroutine()
    //{
    //    yield return new WaitForSeconds(AttackCooldown); // �N�[���_�E�����ԑҋ@
    //    isCanAttack = true; // �N�[���_�E���I����ɍēx�U���\�ɂ���
    //}
    //// �v���C���[���_���[�W���󂯂��ۂɐԂ�����R���[�`��
    //private IEnumerator FlashDamage()
    //{
    //    spriteRenderer.color = Color.red;  // �Ԃ�����
    //    yield return new WaitForSeconds(0.1f);  // 0.1�b�ҋ@
    //    spriteRenderer.color = Color.white;  // ���̐F�ɖ߂�
    //}

    // ���G��Ԃ̊ԁA�v���C���[���Ԃ��_�ł���R���[�`��
    private IEnumerator InvincibleBlinkCoroutine()
    {
        while (isInvincible)
        {
            spriteRenderer.color = Color.red; // �Ԃ�����
            yield return new WaitForSeconds(BlinkInterval);
            spriteRenderer.color = Color.white; // ���̐F�ɖ߂�
            yield return new WaitForSeconds(BlinkInterval);
        }
    }


    // �v���C���[���_���[�W���󂯂��ۂ̖��G���Ԃ�t�^����R���[�`��
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;  // ���G��Ԃɂ���
        StartCoroutine(InvincibleBlinkCoroutine());  // ���G��Ԃ̓_�ł��J�n
        yield return new WaitForSeconds(InvincibleDuration);  // ���G���Ԃ̊ԑҋ@
        isInvincible = false;  // ���G��Ԃ�����
    }

    // �v���C���[�̈ړ����������\�b�h�ɕ���
    private void PlayerMove()
    {
        // WASD�ړ�
        float moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * MoveSpeed, rb.velocity.y);

        // �A�j���[�V�����̐ݒ�
        if (moveX != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        // �� animator.SetBool("isRunning", moveX != 0);

        // ���E�̃L�[���͂ɉ����ăL�����N�^�[�̌�����ύX
        if (moveX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && isFacingRight)
        {
            Flip();
        }

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
            JumpTimeCounter = MaxJumpHoldTimm;
            rb.velocity = new Vector2(rb.velocity.x, MinJumpForce); // �ŏ��W�����v�͂�ݒ�
        }

        // �W�����v�{�^�������������Ă���ꍇ
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (JumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce); // �ő�W�����v�͂܂ŉ��Z
                //JumpTimeCounter -= Time.deltaTime; // �����Ă��鎞�Ԃ��v��
                JumpTimeCounter -= Time.deltaTime * 1.5f;  // �������x�𑬂߂ď㏸���Ԃ�Z�k
            }
            else
            {
                isJumping = false;//�z�o�����O�}�~
            }
        }

        // �W�����v�{�^���𗣂����ꍇ
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;//�z�o�����O�}�~
        }
    }

    // �L�����N�^�[�̌����𔽓]�����郁�\�b�h
    private void Flip()
    {
        isFacingRight = !isFacingRight; // �����𔽓]
        Vector3 theScale = transform.localScale; // ���݂̃X�P�[�����擾
        theScale.x *= -1; // X�������̃X�P�[���𔽓]
        transform.localScale = theScale; // ���]�����X�P�[����K�p
    }

    // �W�����v�֘A�̒n�ʔ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // �n�ʂɐڐG������isGround��true�ɂ���
        {
            isGround = true;
        }

        // Enemy�^�O�����I�u�W�F�N�g�ɏՓ˂����Ƃ�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();  // �v���C���[���_���[�W���󂯂�
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // �n�ʂ��痣�ꂽ��isGround��false�ɂ���
        {
            isGround = false;
        }
    }

    ////Enemy�U������
    //private void OnTriggerEnter2D(Collider2D Collision)
    //{
    //    // AttackCollider ���L���ŁA�Փ˂����I�u�W�F�N�g�� "Enemy" �^�O�������Ă���ꍇ�̂ݏ�������
    //    if (AttackCollider.enabled && Collision.CompareTag("Enemy") && Collision.gameObject.tag != "Enemy_Search")
    //    {
    //        Enemy enemy = Collision.GetComponent<Enemy>();
    //        if (enemy != null)
    //        {
    //            enemy.TakeDamage(PlayerPower); // EnemyHp��player�̍U���͕��̃_���[�W��^����
    //            Debug.Log("Enemy Hp:" + enemy.EnemyHp); // EnemyHp�̊m�F�p
    //        }
    //    }
    //}


    
    // �v���C���[���_���[�W���󂯂鏈��
    public void TakeDamage()
    {

        if (isInvincible)//���G��ԂȂ�_���[�W���󂯂Ȃ�
        {
            return;
        }


        PlayerHp--;  // HP�����炷
        Debug.Log("Player HP: " + PlayerHp);

        // �_���[�W���󂯂��ۂɈ�u�Ԃ�����
        //StartCoroutine(FlashDamage());

        //���G���Ԃ�t�^����i���̏ꍇ�͈��̃_���[�W�ŕ�����_���[�W���󂯂�̂�h�~���邽�߁j
        StartCoroutine(InvincibleCoroutine());

        if (PlayerHp <= 0)
        {
            Debug.Log("Player�����ꂽ�I");
            // �Q�[���I�[�o�[�����Ȃǂ������Œǉ��\
        }
    }
}
