using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMove : MonoBehaviour
{
    Transform playerTr; // プレイヤーのTransform
    [SerializeField] float speed ; // 敵の動くスピード
    [SerializeField] float erea ; //移動開始距離
    [SerializeField] float attack ; //攻撃距離
    [SerializeField] private bool coolTime;//攻撃間隔
    [SerializeField] private int moveSelect;//ランダム行動
    [SerializeField] private int moveCount;//フェイント攻撃発生条件
    [SerializeField] private int powerMove = 0;//フェイント攻撃の為のカウント
    //ランダム行動の最大、最小値
    private int min = 1;
    private int max = 4;
    //カウントのリセット
    private int zero = 0;
    //向き、大きさの調整スクリプト
    [SerializeField] private bool isBack;
    [SerializeField] Vector3 frontMove;
    [SerializeField] Vector3 backMove;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのTransformを取得（プレイヤーのタグをPlayerに設定）
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolTime)//アニメーションがbossStand以外の時に実行されるスクリプト
        {
            //プレイヤーがattackより近くにいると追尾する
            if (Vector2.Distance(transform.position, playerTr.position) < attack)
            {
                moveSelect = Random.Range(min, max); 
                if (powerMove == moveCount)
        {
            animator.Play("bossAtack4");
            powerMove =　zero;
        }
        else if (moveSelect == 1)
        {
            animator.Play("bossAtack");
            powerMove++;
        }
        else if (moveSelect == 2)
        {
            animator.Play("bossAtack2");
            powerMove++;
        }
        else if (moveSelect == 3)
        {
            animator.Play("bossAtack3");
            powerMove++;
        }
            }
            else if (Vector2.Distance(transform.position, playerTr.position) < erea)
            {
                // プレイヤーに向けて進む;
                transform.position = Vector2.MoveTowards(
                    transform.position,
               new Vector2(playerTr.position.x, transform.position.y),
                    speed * Time.deltaTime);
                animator.Play("bossWalk");
            }
            else
            {
                animator.Play("bossStand");
            }

            //プレイヤーの方向を見る
            if (playerTr.position.x < transform.position.x)
            {
                transform.localScale = frontMove;
            }
            else if (playerTr.position.x > transform.position.x)
            {
                transform.localScale = backMove;
            }       
 

        }
    }
    }
