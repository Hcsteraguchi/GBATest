using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour
{
    //ctrl+Fで検索


    [Header("接地判定")] [SerializeField] Status playerStatus = Status.DOWN;

    //Rigidbory2D
    private Rigidbody2D rb;

    //制動用
    private float speedx;//X軸のプレイヤーの速度の絶対値
    private float speedy;//Y軸のプレイヤーの速度の絶対値
    private float horizontalPower;//ジョイスティック左右入力の数値化（右が正）
    private Transform _transform;//transformを取得
    private Vector3 _prevPosition;//前フレームの位置取得

    //横移動用    
    [Header("横移動速度")] [SerializeField] float _speed = 3.0f;
    [Header("横移動加速度")] [SerializeField] float power = 20.0f;
    private bool leftNow = false;//今、左を向いているか（否なら右を向いている）
    private bool playerMove = true;//ジャンプと横移動の許可



    // 攻撃用    
    [Header("ダッシュ攻撃速度")] [SerializeField] float dashForce = 400f;
    [Header("連続攻撃続行連打回数")] [SerializeField] float rCountLimit = 2.0f;
    [Header("連打モーション回数")] [SerializeField] int AGRSCountLimit = 4;
    private bool airAttackCount = true;//空中攻撃可能
    private bool nAttackNow = false;//攻撃中　地上停止
    private bool dAttackNow = false;//攻撃中　地上移動
    private bool rAttackNow = false;//攻撃中　地上連続
    private float rCount = 0.0f;//連打回数カウント
    private int AGRSCount = 0;//連打モーションカウント

    //攻撃間隔制御用
    [Header("攻撃 地上停止 再使用時間")] [SerializeField] float interval1 = 0.75f;
    [Header("攻撃 地上移動 再使用時間")] [SerializeField] float interval2 = 0.5f;
    [Header("攻撃　空中　　再使用時間")] [SerializeField] float interval3 = 0.5f;
    [Header("調整中")] [SerializeField] float interval4 = 0.5f;
    [Header("調整中")] [SerializeField] float attackDelay = 0.2f;//要調節!!  分割すべき？  攻撃後の後隙
    [Header("調整中")] [SerializeField] float attackTime = 1.0f;//連続攻撃受付時間
    [Header("調整中")] [SerializeField] float attackTime1 = 2.0f;//地上通常攻撃　nAttackNow持続時間　
    [Header("調整中")] [SerializeField] float attackDelay1 = 0.2f;//地上通常攻撃  攻撃後の後隙
    private float timer1 = 0.0f;//攻撃 地上停止 再使用時間用
    private float timer2 = 0.0f;//攻撃 地上移動 再使用時間用
    private float timer3 = 0.0f;//攻撃　空中　　再使用時間用
    private float timer4 = 0.0f;//攻撃　連続　　再使用時間用　調整中

    //攻撃判定
    [Header("通常攻撃判定")] [SerializeField] GameObject normalObject;
    [Header("空中攻撃判定")] [SerializeField] GameObject airObject;
    [Header("ダッシュ攻撃判定")] [SerializeField] GameObject dashObject;
    [Header("連続攻撃判定判定")] [SerializeField] GameObject mashObject;
    [Header("最終連続攻撃判定")] [SerializeField] GameObject lastObject;

    //ジャンプ用
    [Header("ジャンプ初速度")] [SerializeField] float jumpSpeed = 16.0f;
    [Header("重力加速度")] [SerializeField] float gravity = 30.0f;
    [Header("最低ジャンプ時間")] [SerializeField] float jumpLowerLimit = 0.1f;
    [Header("調整中")] [SerializeField] float stopMovePoint = 0.3f;//完全停止処理の基準　x軸だけでよろし？
    private float jumpTimer = 0f; // ジャンプ経過時間
    private bool jumpKey = false; // ジャンプキー
    private bool keyLook = false; // ジャンプキー入力不能

    enum Status//プレイヤー状態 上下
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3,      
    }
    //移動スピードと点滅の間隔
    [SerializeField] float speed, flashInterval;
    //点滅させるときのループカウント
    [SerializeField] int loopCount;
    //点滅させるためのSpriteRenderer
    SpriteRenderer sp;
    private bool isGround = false;


    //プレイヤーの状態用列挙型（ノーマル、ダメージ、無敵の3種類）
    enum STATE
    {
        NOMAL,
        DAMAGED,
        MUTEKI
    }
    //STATE型の変数
    STATE state = STATE.NOMAL;

    [SerializeField] private LayerMask GroundLayer;
    public float knockBackPower;   // ノックバックさせる力
    private Damage PL;
    float puramai;
    void Start()
    {
        //Rigidbody2Dを取得
        rb = GetComponent<Rigidbody2D>();
        //プレイヤー座標取得
        _transform = transform;
        _prevPosition = _transform.position;


        //SpriteRenderer格納
        sp = GetComponent<SpriteRenderer>();

        PL = GameObject.Find("PlayerBody").GetComponent<Damage>();
    }

    void Update()
    {
        if (timer1 > 0.0f)
        {
            timer1 -= Time.deltaTime;
        }
        if (timer2 > 0.0f)
        {
            timer2 -= Time.deltaTime;
        }
        if (timer3 > 0.0f)
        {
            timer3 -= Time.deltaTime;
        }
        if (timer4 > 0.0f)
        {
            timer4 -= Time.deltaTime;
        }
        //接地判定レイキャスト
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, GroundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 2f, Color.red);

        if (hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        if (state == STATE.DAMAGED)
        {
            return;
        }
        // ジャンプキー入力取得
        if (Input.GetButton("Jump") && playerMove /*&& state != STATE.DAMAGED*/)
        {
            jumpKey = !keyLook;
        }
        else
        {
            jumpKey = false;
            keyLook = false;
        }

        //左右反転
        Vector3 langle = this.transform.eulerAngles;

        if (langle.y == 0 && leftNow == true)
        {
            langle.y = 180;
        }
        else if (langle.y == 180 && leftNow == false)
        {
            langle.y = 0;
        }
        this.transform.eulerAngles = new Vector3(0, langle.y, 0);







        //連続攻撃の連打判定用
        if (Input.GetButtonDown("Attack") && rAttackNow)
        {
            rCount++;//足す形式を変える
        }

        //攻撃地上連続　AGR
        else if (Input.GetButtonDown("Attack") && playerStatus == Status.GROUND && horizontalPower == 0 && nAttackNow && timer4 <= 0.0f && !dAttackNow)//地上停止攻撃左
        {



            //攻撃中であること　これを条件に連続攻撃へ派生？
            rAttackNow = true;
            Invoke("AGRS", attackTime);

            GameObject childObject = Instantiate(mashObject, transform);

            Debug.Log("連");

        }

        //攻撃地上移動　AGM
        else if (Input.GetButtonDown("Attack") && timer2 <= 0.0f && playerStatus == Status.GROUND && horizontalPower != 0)//攻撃地上移動
        {
            //攻撃中の移動停止
            playerMove = false;
            Invoke("AGM", attackDelay);

            //攻撃中であること
            dAttackNow = true;
            Invoke("AGMS", attackTime);

            if (leftNow)
            {
                Vector2 force = new Vector2(-dashForce, 0);
                rb.AddForce(force);
            }
            else if (!leftNow)
            {
                Vector2 force = new Vector2(dashForce, 0);
                rb.AddForce(force);
            }


            GameObject childObject = Instantiate(dashObject, transform);

            Debug.Log("dash");
            timer1 = interval1;//?いるか、ダッシュ→通常の間
            timer2 = interval2;
        }


        //攻撃地上停止 AGS
        else if (Input.GetButtonDown("Attack") && timer1 <= 0.0f && playerStatus == Status.GROUND && horizontalPower == 0 && !rAttackNow)//地上停止攻撃左
        {
            //攻撃中の移動停止
            playerMove = false;
            Invoke("AGS", attackDelay1);

            //攻撃中であること　これを条件に連続攻撃へ派生？
            nAttackNow = true;
            Invoke("AGSS", attackTime1);

            GameObject childObject = Instantiate(normalObject, transform);

            Debug.Log("通常");
            timer1 = interval1;
            timer4 = interval4;

        }

        //空中攻撃
        else if (Input.GetButtonDown("Attack") && timer3 <= 0.0f && (playerStatus == Status.UP || playerStatus == Status.DOWN) && airAttackCount == true)//空中攻撃左
        {
            airAttackCount = false;
            GameObject childObject = Instantiate(airObject, transform);
            Debug.Log("空");
            timer1 = interval1;//?いるか、空中→通常の間
            timer3 = interval3;

        }




    }

    private void FixedUpdate()
    {
        //jump
        Vector2 newvec = new Vector2(rb.velocity.x, 0);
        switch (playerStatus)
        {
            // 接地時
            case Status.GROUND:
                if (jumpKey)
                {
                    playerStatus = Status.UP;
                }
                break;

            // 上昇時
            case Status.UP:
                jumpTimer += Time.deltaTime;

                if (jumpKey || jumpLowerLimit > jumpTimer)
                {
                    newvec.y = jumpSpeed;
                    newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));//jumpTimer^2
                }

                else
                {
                    jumpTimer += Time.deltaTime; // 落下を早める
                    newvec.y = jumpSpeed;
                    newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                }

                if (0f > newvec.y)
                {
                    playerStatus = Status.DOWN;
                    newvec.y = 0f;
                    jumpTimer = 0.1f;
                }
                break;

            // 落下時
            case Status.DOWN:
                jumpTimer += Time.deltaTime;

                newvec.y = 0f;
                newvec.y -= (gravity * jumpTimer);//調整必要　下候補
                //newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                break;            

            default:
                break;
        }
        rb.velocity = newvec;

        // 右・左
        horizontalPower = Input.GetAxisRaw("Horizontal");
        if (playerMove)
        {


            //右 移動    
            if (Input.GetKey(KeyCode.D) || horizontalPower > 0)
            {
                leftNow = false;


                rb.AddForce(Vector2.right * ((_speed - rb.velocity.x) * power));
                //this.rb.velocity = new Vector2(_speed, rb.velocity.y);//試作




            }

            //左  移動     
            if (Input.GetKey(KeyCode.A) || horizontalPower < 0)
            {
                leftNow = true;

                rb.AddForce(Vector2.right * ((-_speed - rb.velocity.x) * power));
                //this.rb.velocity = new Vector2(-_speed, rb.velocity.y);//試作

            }
        }

        //停止及びジャンプ可変用

        // 現在フレームのワールド位置
        Vector2 position = _transform.position;
        // 移動量を計算
        Vector2 delta = _prevPosition;
        // 次のUpdateで使うための前フレーム位置更新
        _prevPosition = position;

        float delX = position.x - delta.x;
        float delY = position.y - delta.y;

        //プレイヤー速度(X & Y)
        speedx = Mathf.Abs(this.rb.velocity.x);
        speedy = Mathf.Abs(this.rb.velocity.y);

        //制動距離に関する処理
        if (speedx > 0.1f)
        {
            this.rb.AddForce(new Vector2(delX * -200, 0));//?
        }
        //if (speedy > 0.1f && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && delY > 0)
        //{
        //    this.rb.AddForce(new Vector3(0, delY * -200, 0));
        //}

        //if (delY < 0.0f)
        //{

        //}
        ////完全停止処理
        if (speedx + speedy <= stopMovePoint)
        {
            rb.velocity = new Vector2(0, 0);
        }

    }
    void AGRSS()
    {
        rAttackNow = false;
    }
    void AGRS()
    {
        Debug.Log("連2");
        if (rCount > rCountLimit && AGRSCount < AGRSCountLimit)
        {
            Invoke("AGRS", attackTime);
            rCount = 0;
            AGRSCount++;
            GameObject childObject = Instantiate(mashObject, transform);

            Debug.Log("連3");
        }
        else
        {
            AGRSCount = 0;
            rCount = 0;
            Invoke("AGRSS", attackTime);
            GameObject childObject = Instantiate(lastObject, transform);
            Debug.Log("連4");
            Invoke("AGM", attackDelay);
        }
    }
    void AGR()//攻撃地上連続用
    {
        if (!nAttackNow || !rAttackNow)
            playerMove = true;//ダッシュ攻撃中なら移動を発動しなくするべき
    }

    void AGS()//攻撃地上停止用
    {
        if (!dAttackNow && !rAttackNow)
            playerMove = true;//ダッシュ攻撃中なら移動を発動しなくするべき
    }
    void AGSS()
    {
        nAttackNow = false;
    }

    void AGM()//攻撃地上移動用
    {
        playerMove = true;
    }
    void AGMS()
    {
        dAttackNow = false;
    }
    


    void OnCollisionStay2D(Collision2D collision)
    {
        if (isGround && playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            playerStatus = Status.GROUND;
            jumpTimer = 0f;
            keyLook = true; // キー操作をロックする
            airAttackCount = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            playerStatus = Status.DOWN;
            airAttackCount = true;
        }
    }
    public void ShowLog()
    {
        // ログを表示します。
        playerStatus = Status.DOWN;

        //変更 ノーマルじゃない（ダメージ中、無敵中）ときはリターン
        if (state != STATE.NOMAL)
        {
            return;
        }
        playerMove = false;
        Invoke("AGM", attackDelay);       
        rb.velocity = Vector2.zero;

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        //Vector2 distination = (transform.position - PL.Enemy.transform.position).normalized;
        //rb.AddForce(distination * knockBackPower);
        float dist = transform.position.x - PL.Enemy.transform.position.x;
        if(dist >= 0)
        {
             puramai = 1;
        }
        else
        {
            puramai = -1;
        }
        //rb.AddForce(Vector2.right * puramai * knockBackPower,0);
        rb.AddForce((new Vector2 (1* puramai,5))  * knockBackPower, 0);

        state = STATE.DAMAGED;
        StartCoroutine(_hit());

    }

    IEnumerator _hit()
    {

        sp.color = Color.black;
        for (int i = 0; i < loopCount; i++)
        {

            yield return new WaitForSeconds(flashInterval);
            sp.enabled = false;
            yield return new WaitForSeconds(flashInterval);
            sp.enabled = true;
            if (i > 10)//？？？
            {
                state = STATE.MUTEKI;
                sp.color = Color.green;
            }
        }
        state = STATE.NOMAL;
        sp.color = Color.white;
    }


}
