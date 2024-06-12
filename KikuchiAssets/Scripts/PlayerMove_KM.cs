using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour {
    //ctrl+Fで検索
    #region  
    //Rigidbory2D
    private Rigidbody2D _rigidbody2D;

    //制動用
    private float _speedX;//X軸のプレイヤーの速度の絶対値
    private float _speedY;//Y軸のプレイヤーの速度の絶対値
    private float _horizontalPower;//ジョイスティック左右入力の数値化（右が正）
    private Transform _transform;//transformを取得
    private Vector3 _prevPosition;//前フレームの位置取得

    //横移動用    
    [Header("横移動速度")] [SerializeField] private float _sideSpeed = 10.0f;
    [Header("横移動加速度")] [SerializeField] private float _sidePower = 20.0f;
    private bool _isLeft = false;//今、左を向いているか（否なら右を向いている）
    private bool _canPlayerMove = true;//ジャンプと横移動の許可//名前訂正?



    // 攻撃用    
    [Header("ダッシュ攻撃速度")] [SerializeField] private float _dashForce = 600f;
    [Header("連続攻撃続行連打回数")] [SerializeField] private float _mashCountLimit = 2.0f;
    [Header("連打モーション回数")] [SerializeField] private int _mashAttackSecondNextCountLimit = 4;
    private bool _canAirAttack = true;//空中攻撃可能
    private bool _isNormalAttack = default;//攻撃中　地上停止
    private bool _isDashAttack = default;//攻撃中　地上移動
    private bool _isMashAttack = default;//攻撃中　地上連続
    private int _mashCount = 0;//連打回数カウント 
    private int _mashAttackSecondNextCount = 0;//連打モーションカウント 

    //攻撃間隔制御用
    [Header("通常攻撃 再使用時間")] [SerializeField] private float _normalAttackInterval = 0.75f;
    [Header("ダッシュ攻撃 再使用時間")] [SerializeField] private float _dashAttackInterval = 0.5f;
    [Header("空中攻撃　　再使用時間")] [SerializeField] private float _airAttackInterval = 0.75f;
    [Header("通常→連続間　使用間隔")] [SerializeField] private float _mashAttackInterval = 0.5f;//通常→連続の接続
    [Header("調整中・攻撃後の後隙（その他）分割すべき？")] [SerializeField] private float _attackDelay = 0.8f;//要調節!!  分割すべき？  攻撃後の後隙 
    [Header("連続攻撃受付時間")] [SerializeField] float _mashTime = 1.0f;//連続攻撃受付時間
    [Header("地上通常攻撃持続時間")] [SerializeField] private float _normalTime = 1.0f;//地上通常攻撃　nAttackNow持続時間　
    [Header("地上通常攻撃後の後隙")] [SerializeField] private float _normalDelay = 1.0f;//地上通常攻撃  攻撃後の後隙
    private float _normalAttackIntervalTimer = 0.0f;//攻撃 地上停止 再使用時間用 
    private float _dashAttackIntervalTimer = 0.0f;//攻撃 地上移動 再使用時間用 
    private float _airAttackIntervalTimer = 0.0f;//攻撃　空中　　再使用時間用 
    private float _mashAttackIntervalTimer = 0.0f;//攻撃　連続　　再使用時間用

    //攻撃判定
    [Header("通常攻撃判定")] [SerializeField] private GameObject _normalAttackObject;
    [Header("ダッシュ攻撃判定")] [SerializeField] private GameObject _dashAttackObject;
    [Header("空中攻撃判定")] [SerializeField] private GameObject _airAttackObject;
    [Header("連続攻撃判定判定")] [SerializeField] private GameObject _mashAttackObject;
    [Header("最終連続攻撃判定")] [SerializeField] private GameObject _lastMashAttackObject;

    //ジャンプ用
    [Header("ジャンプ初速度")] [SerializeField] private float _jumpSpeed = 16.0f;
    [Header("重力加速度")] [SerializeField] private float _gravityPower = 30.0f;
    [Header("最低ジャンプ時間")] [SerializeField] private float _jumpLowerTime = 0.05f;
    [Header("調整中・完全停止処理の基準")] [SerializeField] private float _stopMovePoint = 1f;//完全停止処理の基準　x軸だけでよろし？//名前訂正?
    [Header("ジャンプ可能なレイヤー")] [SerializeField] private LayerMask _groundLayer;
    private float _jumpTimer = 0f; // ジャンプ経過時間
    private bool _isJumpKey = false; // ジャンプキー 
    private bool _keyLook = false; // ジャンプキー入制限 
    private bool _isGround = false;//レイキャストの設置判定
    private Status _playerStatus = Status.DOWN;//OnCollisionの設置判定
    enum Status//プレイヤー状態 上下
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3,
    }

    //ダメージ時
    [Header("ノックバックさせる力")] [SerializeField] public float _knockBackPower = 1000;
    [Header("ダメージ時の点滅の間隔")] [SerializeField] float _flashInterval = 0.02f;
    [Header("点滅させるときのループカウント")] [SerializeField] int _loopCount = 60;
    private SpriteRenderer _mySpriteRenderer;//点滅させるためのSpriteRenderer
    private STATE _damageState = STATE.NOMAL;//プレイヤーのダメージ状態 //名前訂正?
    private Damage _damageScript;//ダメージ判定用スクリプト接続 //名前訂正?
    private float _knockBackDirection;//ノックバック方向 //名前訂正?

    //プレイヤーの状態ダメージ用（ノーマル、ダメージ、無敵）
    enum STATE {
        NOMAL,
        DAMAGED,
        MUTEKI
    }
    #endregion



    private Collider2D _normalAttackObjectCollider;
    private SpriteRenderer _normalAttackObjectSpriteRenderer;
    private Collider2D _dashAttackObjectCollider;
    private SpriteRenderer _dashAttackObjectSpriteRenderer;
    private Collider2D _airAttackObjectCollider;
    private SpriteRenderer _airAttackObjectSpriteRenderer;
    private Collider2D _mashAttackObjectCollider;
    private SpriteRenderer _mashAttackObjectSpriteRenderer;
    private Collider2D _lastMashAttackObjectCollider;
    private SpriteRenderer _lastMashAttackObjectSpriteRenderer;

    [Header("攻撃判定存在時間")] [SerializeField] float _attckEreaTime = 0.3f;

    void Start() {
        //Rigidbody2Dを取得
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //プレイヤー座標取得
        _transform = transform;
        _prevPosition = _transform.position;


        //SpriteRenderer格納
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        _damageScript = GameObject.Find("PlayerBody").GetComponent<Damage>();


        //siken
        _normalAttackObjectCollider = _normalAttackObject.GetComponent<CapsuleCollider2D>();
        _normalAttackObjectSpriteRenderer = _normalAttackObject.GetComponent<SpriteRenderer>();
        _dashAttackObjectCollider = _dashAttackObject.GetComponent<BoxCollider2D>();
        _dashAttackObjectSpriteRenderer = _dashAttackObject.GetComponent<SpriteRenderer>();
        _airAttackObjectCollider = _airAttackObject.GetComponent<CircleCollider2D>();
        _airAttackObjectSpriteRenderer = _airAttackObject.GetComponent<SpriteRenderer>();
        _mashAttackObjectCollider = _mashAttackObject.GetComponent<BoxCollider2D>();
        _mashAttackObjectSpriteRenderer = _mashAttackObject.GetComponent<SpriteRenderer>();
        _lastMashAttackObjectCollider = _lastMashAttackObject.GetComponent<BoxCollider2D>();
        _lastMashAttackObjectSpriteRenderer = _lastMashAttackObject.GetComponent<SpriteRenderer>();

    }

    void Update() {
        if (_normalAttackIntervalTimer > 0.0f) {
            _normalAttackIntervalTimer -= Time.deltaTime;
        }
        if (_dashAttackIntervalTimer > 0.0f) {
            _dashAttackIntervalTimer -= Time.deltaTime;
        }
        if (_airAttackIntervalTimer > 0.0f) {
            _airAttackIntervalTimer -= Time.deltaTime;
        }
        if (_mashAttackIntervalTimer > 0.0f) {
            _mashAttackIntervalTimer -= Time.deltaTime;
        }
        //接地判定レイキャスト
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 2f, Color.red);

        if (hit) {
            _isGround = true;
        } else {
            _isGround = false;
        }
        if (_damageState == STATE.DAMAGED) {
            return;
        }
        // ジャンプキー入力取得
        if (Input.GetButton("Jump") && _canPlayerMove /*&& state != STATE.DAMAGED*/) {
            _isJumpKey = !_keyLook;
        } else {
            _isJumpKey = false;
            _keyLook = false;
        }

        //左右反転
        Vector3 langle = this.transform.eulerAngles;

        if (langle.y == 0 && _isLeft == true) {
            langle.y = 180;
        } else if (langle.y == 180 && _isLeft == false) {
            langle.y = 0;
        }
        this.transform.eulerAngles = new Vector3(0, langle.y, 0);







        //連続攻撃の連打判定用
        if (Input.GetButtonDown("Attack") && _isMashAttack) {
            _mashCount++;//足す形式を変える
        }

        //連続攻撃　mashAttack
        else if (Input.GetButtonDown("Attack") && _playerStatus == Status.GROUND && _horizontalPower == 0 && _isNormalAttack && _mashAttackIntervalTimer <= 0.0f && !_isDashAttack)//地上停止攻撃左
        {



            //攻撃中であること　これを条件に連続攻撃へ派生？
            _isMashAttack = true;
            Invoke("MashAttackSecondNext", _mashTime);

            //GameObject childObject = Instantiate(_mashAttackObject, transform);
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;
            Invoke("MashAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延

            Debug.Log("連");

        }

        //ダッシュアタック　dashAttack
        else if (Input.GetButtonDown("Attack") && _dashAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND && _horizontalPower != 0)//攻撃地上移動
        {
            //攻撃中の移動停止
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);

            //攻撃中であること
            _isDashAttack = true;
            Invoke("DasshAttackBoolFalse", _mashTime);

            if (_isLeft) {
                Vector2 force = new Vector2(-_dashForce, 0);
                _rigidbody2D.AddForce(force);
            } else if (!_isLeft) {
                Vector2 force = new Vector2(_dashForce, 0);
                _rigidbody2D.AddForce(force);
            }


            //GameObject childObject = Instantiate(_dashAttackObject, transform);
            //ゲームオブジェクト非表示→表示
            _dashAttackObjectCollider.enabled = true;
            _dashAttackObjectSpriteRenderer.enabled = true;
            Invoke("DashAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延



            _normalAttackIntervalTimer = _normalAttackInterval;//?いるか、ダッシュ→通常の間
            _dashAttackIntervalTimer = _dashAttackInterval;
        }


        //通常攻撃 normalAttack
        else if (Input.GetButtonDown("Attack") && _normalAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND && _horizontalPower == 0 && !_isMashAttack)//地上停止攻撃左
        {
            //攻撃中の移動停止
            _canPlayerMove = false;
            Invoke("NormalAttackPlayerMoveTrue", _normalDelay);

            //攻撃中であること　これを条件に連続攻撃へ派生？
            _isNormalAttack = true;
            Invoke("NormalAttackBoolFalse", _normalTime);

            //GameObject childObject = Instantiate(_normalAttackObject, transform);
            //ゲームオブジェクト非表示→表示
            //_ktext.SetActive(true);
            _normalAttackObjectCollider.enabled = true;
            _normalAttackObjectSpriteRenderer.enabled = true;
            Invoke("NormalAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延



            Debug.Log("通常");
            _normalAttackIntervalTimer = _normalAttackInterval;
            _mashAttackIntervalTimer = _mashAttackInterval;


        }

        //空中攻撃 airAttack
        else if (Input.GetButtonDown("Attack") && _airAttackIntervalTimer <= 0.0f && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && _canAirAttack == true)//空中攻撃左
        {
            _canAirAttack = false;
            //GameObject childObject = Instantiate(_airAttackObject, transform);
            _airAttackObjectCollider.enabled = true;
            _airAttackObjectSpriteRenderer.enabled = true;
            Invoke("AirAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延
            Debug.Log("空");
            _normalAttackIntervalTimer = _normalAttackInterval;//?いるか、空中→通常の間
            _airAttackIntervalTimer = _airAttackInterval;

        }




    }

    private void FixedUpdate() {
        //jump
        Vector2 newvec = new Vector2(_rigidbody2D.velocity.x, 0);
        switch (_playerStatus) {
            // 接地時
            case Status.GROUND:
                if (_isJumpKey) {
                    _playerStatus = Status.UP;
                }
                break;

            // 上昇時
            case Status.UP:
                _jumpTimer += Time.deltaTime;

                if (_isJumpKey || _jumpLowerTime > _jumpTimer) {
                    newvec.y = _jumpSpeed;
                    newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));//jumpTimer^2
                } else {
                    _jumpTimer += Time.deltaTime; // 落下を早める
                    newvec.y = _jumpSpeed;
                    newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));
                }

                if (0f > newvec.y) {
                    _playerStatus = Status.DOWN;
                    newvec.y = 0f;
                    _jumpTimer = 0.1f;
                }
                break;

            // 落下時
            case Status.DOWN:
                _jumpTimer += Time.deltaTime;

                newvec.y = 0f;
                newvec.y -= (_gravityPower * _jumpTimer);//調整必要　下候補
                //newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                break;

            default:
                break;
        }
        _rigidbody2D.velocity = newvec;

        // 右・左
        _horizontalPower = Input.GetAxisRaw("Horizontal");
        if (_canPlayerMove) {


            //右 移動    
            if (Input.GetKey(KeyCode.D) || _horizontalPower > 0) {
                _isLeft = false;


                _rigidbody2D.AddForce(Vector2.right * ((_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));
                //this.rb.velocity = new Vector2(_speed, rb.velocity.y);//試作




            }

            //左  移動     
            if (Input.GetKey(KeyCode.A) || _horizontalPower < 0) {
                _isLeft = true;

                _rigidbody2D.AddForce(Vector2.right * ((-_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));
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
        _speedX = Mathf.Abs(this._rigidbody2D.velocity.x);
        _speedY = Mathf.Abs(this._rigidbody2D.velocity.y);

        //制動距離に関する処理
        if (_speedX > 0.1f) {
            this._rigidbody2D.AddForce(new Vector2(delX * -200, 0));//?
        }
        //if (speedy > 0.1f && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && delY > 0)
        //{
        //    this.rb.AddForce(new Vector3(0, delY * -200, 0));
        //}

        //if (delY < 0.0f)
        //{

        //}
        ////完全停止処理
        if (_speedX + _speedY <= _stopMovePoint) {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }

    }
    void MashAttackBoolFalse()//連続攻撃中を停止
    {
        _isMashAttack = false;
    }
    void MashAttackSecondNext()//連続攻撃二撃目以降
    {
        Debug.Log("連2");
        if (_mashCount > _mashCountLimit && _mashAttackSecondNextCount < _mashAttackSecondNextCountLimit) {
            Invoke("MashAttackSecondNext", _mashTime);
            _mashCount = 0;
            _mashAttackSecondNextCount++;
            //GameObject childObject = Instantiate(_mashAttackObject, transform);
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;
            Invoke("MashAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延

            Debug.Log("連3");
        } else {
            _mashAttackSecondNextCount = 0;
            _mashCount = 0;
            Invoke("MashAttackBoolFalse", _mashTime);
            //GameObject childObject = Instantiate(_lastMashAttackObject, transform);
            _lastMashAttackObjectCollider.enabled = true;
            _lastMashAttackObjectSpriteRenderer.enabled = true;
            Invoke("LastMashAttackObjectDestroy", _attckEreaTime);//攻撃判定破壊遅延
            Debug.Log("連4");
            Invoke("PlayerMoveTrue", _attackDelay);
        }
    }
    //void AGR()//攻撃地上連続用
    //{
    //    if (!_isNormalAttack || !_isMashAttack) {
    //        _canPlayerMove = true;//ダッシュ攻撃中なら移動を発動しなくするべき
    //    }

    //}

    void NormalAttackPlayerMoveTrue()//通常攻撃時プレイヤー残心解除　
    {
        if (!_isDashAttack && !_isMashAttack) {
            _canPlayerMove = true;//ダッシュ攻撃中なら移動を発動しなくするべき
        }

    }
    void NormalAttackBoolFalse() {//通常攻撃中を停止
        _isNormalAttack = false;
    }

    void PlayerMoveTrue()//攻撃時プレイヤー残心解除
    {
        _canPlayerMove = true;
    }
    void DasshAttackBoolFalse() //ダッシュ攻撃中を停止
        {
        _isDashAttack = false;
    }



    void OnCollisionStay2D(Collision2D collision) {
        if (_isGround && _playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            _playerStatus = Status.GROUND;
            _jumpTimer = 0f;
            _keyLook = true; // キー操作をロックする
            _canAirAttack = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (_playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            _playerStatus = Status.DOWN;
            _canAirAttack = true;
        }
    }
    public void ShowLog() {
        // ログを表示します。
        _playerStatus = Status.DOWN;

        //変更 ノーマルじゃない（ダメージ中、無敵中）ときはリターン
        if (_damageState != STATE.NOMAL) {
            return;
        }
        _canPlayerMove = false;
        Invoke("PlayerMoveTrue", _attackDelay);
        _rigidbody2D.velocity = Vector2.zero;

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        //Vector2 distination = (transform.position - PL.Enemy.transform.position).normalized;
        //rb.AddForce(distination * knockBackPower);
        float dist = transform.position.x - _damageScript._attackEnemy.transform.position.x;
        if (dist >= 0) {
            _knockBackDirection = 1;
        } else {
            _knockBackDirection = -1;
        }
        //rb.AddForce(Vector2.right * puramai * knockBackPower,0);
        _rigidbody2D.AddForce(new Vector2(1 * _knockBackDirection, 5) * _knockBackPower, 0);

        _damageState = STATE.DAMAGED;
        StartCoroutine(DamageKnockBack());

    }

    IEnumerator DamageKnockBack() {

        _mySpriteRenderer.color = Color.black;
        for (int i = 0; i < _loopCount; i++) {

            yield return new WaitForSeconds(_flashInterval);
            _mySpriteRenderer.enabled = false;
            yield return new WaitForSeconds(_flashInterval);
            _mySpriteRenderer.enabled = true;
            if (i > 10)//？？？
            {
                _damageState = STATE.MUTEKI;
                _mySpriteRenderer.color = Color.green;
            }
        }
        _damageState = STATE.NOMAL;
        _mySpriteRenderer.color = Color.white;
    }
    #region//攻撃判定消去
    void NormalAttackObjectDestroy() {
        _normalAttackObjectCollider.enabled = false;
        _normalAttackObjectSpriteRenderer.enabled = false;
    }
    void DashAttackObjectDestroy() {
        _dashAttackObjectCollider.enabled = false;
        _dashAttackObjectSpriteRenderer.enabled = false;
    }
    void AirAttackObjectDestroy() {
        _airAttackObjectCollider.enabled = false;
        _airAttackObjectSpriteRenderer.enabled = false;
    }
    void MashAttackObjectDestroy() {
        _mashAttackObjectCollider.enabled = false;
        _mashAttackObjectSpriteRenderer.enabled = false;
    }
    void LastMashAttackObjectDestroy() {
        _lastMashAttackObjectCollider.enabled = false;
        _lastMashAttackObjectSpriteRenderer.enabled = false;
    }

    #endregion//攻撃判定消去


}
