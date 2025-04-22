using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] private Animator[] EnemyAnims;  // 複数の敵キャラクターのアニメーターを格納する配列

    // 敵がIdle状態（停止）になるアニメーションを実行するメソッド
    public void Animation_1_Idle()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)  // すべての敵キャラクターをチェック
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)  // アクティブな敵だけに対して処理を行う
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is Idling");  // デバッグログを表示
            }
        }
    }

    // 敵がRun（走る）状態になるアニメーションを実行するメソッド
    public void Animation_2_Run()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", true);  // Runアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is Running");
            }
        }
    }

    // 敵がHit（被弾）アニメーションを実行するメソッド
    public void Animation_3_Hit()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetTrigger("Hit");  // Hitアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is being Hit");
            }
        }
    }

    // 敵がDeath（死亡）アニメーションを実行するメソッド
    public void Animation_4_Death()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetTrigger("Death");  // Deathアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " has died");
            }
        }
    }

    // 敵がAbility（アビリティ発動）アニメーションを実行するメソッド
    public void Animation_5_Ability()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetBool("Ability", true);  // Abilityアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its First Ability");
            }
        }
    }

    // 敵がAbility 2（第2アビリティ発動）アニメーションを実行するメソッド
    public void Animation_5_Ability2()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetBool("Ability 2", true);  // 第2アビリティアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its Second Ability");
            }
        }
    }

    // 敵がAbility 3（第3アビリティ発動）アニメーションを実行するメソッド
    public void Animation_5_Ability3()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetBool("Ability 3", true);  // 第3アビリティアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its Third Ability");
            }
        }
    }

    // 敵がAttack（攻撃）アニメーションを実行するメソッド
    public void Animation_6_Attack()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetTrigger("Attack");  // Attackアニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its Primary Attack");
            }
        }
    }

    // 敵がAttack 2（第2攻撃）アニメーションを実行するメソッド
    public void Animation_7_Attack2()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetTrigger("Attack 2");  // 第2攻撃アニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its Secondary Attack");
            }
        }
    }

    // 敵がAttack 3（第3攻撃）アニメーションを実行するメソッド
    public void Animation_8_Attack3()
    {
        for (int i = 0; i < EnemyAnims.Length; i++)
        {
            if (EnemyAnims[i].gameObject.activeSelf == true)
            {
                EnemyAnims[i].SetBool("Run", false);  // Runアニメーションを停止
                EnemyAnims[i].SetTrigger("Attack 3");  // 第3攻撃アニメーションを再生
                Debug.Log("The enemy " + EnemyAnims[i].gameObject.name + " is using its Tertiary Attack");
            }
        }
    }
}