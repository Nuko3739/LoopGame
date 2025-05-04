using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //  ��
    private int currentScore = 10000; // ���̃X�R�A�i��ŃX�R�A�V�X�e������擾����j




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

    [Header("����X�e�b�v�֘A")]
    public float StepDuration = 0.2f; // �X�e�b�v�̎�������
    public float StepCooldown = 0.5f; // �X�e�b�v�̃N�[���_�E��
    private bool isDodging = false;   // �X�e�b�v�����ǂ���
    private bool canDodge = true;     // �X�e�b�v�\���ǂ���

    [Header("Sound�ݒ�")]
    public AudioClip StepSE; �@�@�@�@ //���p��SE
    private AudioSource audioSource;  //Sound�Đ��pAudioSource


    private Rigidbody2D rb;           // 2D�����G���W����Rigidbody2D
    private SpriteRenderer spriteRenderer; // �X�v���C�g�����_���[�̎Q��
    private Animator animator; // �A�j���[�^�[�̎Q��
    private bool isSmallJump = false; // ���W�����v���ǂ������Ǘ�����


    private bool isGround;            // �W�����v�֘A�̒n�ʔ���t���O
    private bool isJumping;           // �W�����v�������肷��t���O
    private float JumpTimeCounter;    // �W�����v�{�^���������Ă��鎞��
    private bool isFacingRight = true; // �v���C���[�̌�����ǐՁi�f�t�H���g�͉E�����j
    private bool isInvincible = false; // ���G��Ԃ��ǂ����̃t���O
    //private bool isCanAttack = true;     // �U���ł��邩�ǂ����̃t���O


    [Header("���[�v�ݒ�")]
    public Transform RestartPosition;     //player���E�[�ɒ����Ă���߂�ꏊ
                                          //��Restart�ʒu��Transform���i�[����
                                          //[Header("�U���֘A�̐ݒ�")]
                                          //public Collider2D AttackCollider; // �U������̃R���C�_�[
                                          //public float AttackCooldown = 0.5f; // �U���̃N�[���_�E������

    private ScoreManager ScoreManager;

    [Header("�U����e�F���G���Ԃ̐ݒ�")]
    public float InvincibleDuration = 0.25f; // ���G���Ԃ̒���
    public float BlinkInterval = 0.000001f; // �_�ŊԊu

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Rigidbody2D�i2D�Łj�̎擾
        animator = GetComponent<Animator>();  // �A�j���[�^�[�̎擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        ScoreManager = FindObjectOfType<ScoreManager>(); //ScoreManager���擾

        rb.gravityScale = GravityScale;// Inspector�Őݒ�ł���d�͂�Rigidbody2D�ɔ��f

        audioSource = GetComponent<AudioSource>();//�����Sound�p
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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

        //�v���C���[�̉�������̌Ăяo��
        PlayerStep();

        //�󒆂ɂ���Ƃ��ɗ������x�𑬂߂�
        if (!isGround && rb.velocity.y < 0)
        {
            rb.gravityScale = GravityScale * AddGravityScale; // �������̏d�͂�����
        }
        else
        {
            rb.gravityScale = GravityScale; // �ʏ�̏d�͂ɖ߂�
        }

        
    }


    // ���G��Ԃ̊ԁA�v���C���[���_�ł���R���[�`��
    private IEnumerator InvincibleBlinkCoroutine()
    {
        while (isInvincible)
        {
            spriteRenderer.color = Color.clear; // �����i����������j
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
        if (isDodging) return; // �X�e�b�v���͉������Ȃ�

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
        //�@�󒆂ɂ���Ƃ��͌�����ς��Ȃ�
        if (isGround || Input.GetKey(KeyCode.J)) // �n�ʂɂ��邩�A�U�����Ă����甽�]����
        {
            if (moveX > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveX < 0 && isFacingRight)
            {
                Flip();
            }
        }


        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            isJumping = true;
            JumpTimeCounter = MaxJumpHoldTimm;
            rb.velocity = new Vector2(rb.velocity.x, MinJumpForce); // �ŏ��W�����v�͂�ݒ�

            isSmallJump = true; // �ŏ��͏��W�����v�Ɖ��肵�Ă���
        }

        // �W�����v�{�^�������������Ă���ꍇ
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (JumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce); // �ő�W�����v�͂܂ŉ��Z
                                                                     //JumpTimeCounter -= Time.deltaTime; // �����Ă��鎞�Ԃ��v��
                JumpTimeCounter -= Time.deltaTime * 1.5f;  // �������x�𑬂߂ď㏸���Ԃ�Z�k

                // ���������ꂽ�珬�W�����v�ł͂Ȃ��Ɣ���
                if (JumpTimeCounter < MaxJumpHoldTimm * 0.7f) // 70%�ȏ㉟�������W�����v����
                {
                    isSmallJump = false;
                }
            }
            else
            {
                isJumping = false; // �z�o�����O�}�~
            }
        }

        // �W�����v�{�^���𗣂����ꍇ
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false; // �z�o�����O�}�~
        }





        // --- ��������A�j���[�V�������� ---

        // �W�����v�̃A�j���[�V����
        if (!isGround)
        {
            if (isSmallJump)
            {
                animator.SetBool("isSmallJump", true); // ���W�����v�A�j�����Đ�
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true); // �ʏ�W�����v�A�j�����Đ�
                animator.SetBool("isSmallJump", false);
            }
        }
        else
        {
            // �n�ʂɒ������烊�Z�b�g
            animator.SetBool("isJumping", false);
            animator.SetBool("isSmallJump", false);
        }
    }

    //����X�e�b�v�̃��\�b�h
    private void PlayerStep()
    {
        // �X�e�b�v���͏����iL�L�[�j
        if (Input.GetKeyDown(KeyCode.L) && canDodge && !isDodging && isGround)
        {
            StartCoroutine(DodgeStep());
        }
    }

    //�X�e�b�v�����̃R���[�`��
    private IEnumerator DodgeStep()
    {
        animator.SetBool("isDodging", true);
        // �X�e�b�v�I����
        
        animator.Update(0); // �����ɃA�j���[�V������Ԃ𔽉f
        // �X�e�b�vSE�Đ�
        if (StepSE != null)
        {
            audioSource.PlayOneShot(StepSE);
        }

        isDodging = true;
        canDodge = false;
        isInvincible = true;

        float stepDirection = isFacingRight ? 1f : -1f;

        
        // �_�ŉ��o���X�^�[�g
        StartCoroutine(DodgeBlinkCoroutine());

        // �X�e�b�v�ړ�
        rb.velocity = new Vector2(stepDirection * MoveStep / StepDuration, 0f);

        yield return new WaitForSeconds(StepDuration);

        animator.SetBool("isDodging", false);

        // �I������
        rb.velocity = Vector2.zero;
        isDodging = false;
        isInvincible = false;

        yield return new WaitForSeconds(StepCooldown);
        canDodge = true;
    }

    //����X�e�b�v�̐_��
    private IEnumerator DodgeBlinkCoroutine()
    {
        while (isDodging)
        {
            // ���_�Łi�`��ON/OFF�ł͂Ȃ��F�ω��Ŗ����I�ɐn�Ɂj
            spriteRenderer.color = Color.cyan;
            yield return new WaitForSeconds(0.05f); // 50�~���b ON

            spriteRenderer.color = new Color(0.3f, 0.3f, 1f); // ����������
            yield return new WaitForSeconds(0.05f); // 50�~���b OFF���i�ʂ̐j
        }

        // �X�e�b�v���I�������F��߂�
        spriteRenderer.color = Color.white;
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

            // �X�R�A��ScoreManager����擾
            int finalScore = 0;
            if (ScoreManager != null)
            {
                finalScore = ScoreManager.GetScore(); // ScoreManager����X�R�A���擾
                //Debug.Log("ScoreManager����擾�����X�R�A: " + finalScore); // �����ŃX�R�A���m�F
            }

            // RankingManager��T���ăX�R�A��o�^
            RankingManager rankingManager = FindObjectOfType<RankingManager>();
            if (rankingManager != null)
            {
                Debug.Log("Current Score being added: " + currentScore); // �m�F�p���O
                rankingManager.AddNewScore(finalScore); // �X�R�A��ǉ�
            }
            else
            {
                Debug.LogError("RankingManager��������܂���ł����I"); // ������Ȃ��ꍇ�̃G���[���O
            }

            // �Q�[���I�[�o�[�V�[���ɑJ��
            SceneManager.LoadScene("GameOverScene");
        }
    }

    // �X�e�b�v�����ǂ����𑼃X�N���v�g����Q�Ƃ���p
    public bool IsDodging()
    {
        return isDodging;
    }
    // �v���C���[���E�������Ă��邩�𑼃X�N���v�g����Q�Ƃ���p
    public bool IsFacingRight()
    {
        return isFacingRight;
    }
}
