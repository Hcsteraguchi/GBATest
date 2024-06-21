using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour {
    #region //変数 
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
    [Header("横移動減速速度")] [SerializeField] private float _decelerationSpeed = 200.0f;
    public bool _isLeft = false;//今、左を向いているか（否なら右を向いている）
    private bool _canPlayerMove = true;//ジャンプと横移動の許可



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
    [Header("通常→連続間　使用間隔")] [SerializeField] private float _mashAttackInterval = 0.5f;
    [Header("調整中・攻撃後の後隙（その他）分割すべき？")] [SerializeField] private float _attackDelay = 0.8f;//要調節!!  分割すべき？  攻撃後の後隙 
    [Header("連続攻撃受付時間")] [SerializeField] float _mashTime = 1.0f;
    [Header("地上通常攻撃持続時間")] [SerializeField] private float _normalTime = 1.0f;
    [Header("地上通常攻撃後の後隙")] [SerializeField] private float _normalDelay = 1.0f;
    private float _normalAttackIntervalTimer = 0.0f;//攻撃 地上停止 再使用時間用 
    private float _dashAttackIntervalTimer = 0.0f;//攻撃 地上移動 再使用時間用 
    private float _airAttackIntervalTimer = 0.0f;//攻撃　空中　　再使用時間用 
    private float _mashAttackIntervalTimer = 0.0f;//攻撃　連続　　再使用時間用

    //攻撃判定
    [Header("攻撃判定存在時間")] [SerializeField] float _attckEreaTime = 0.3f;
    [Header("通常攻撃判定")] [SerializeField] private GameObject _normalAttackObject;
    [Header("ダッシュ攻撃判定")] [SerializeField] private GameObject _dashAttackObject;
    [Header("空中攻撃判定")] [SerializeField] private GameObject _airAttackObject;
    [Header("連続攻撃判定判定")] [SerializeField] private GameObject _mashAttackObject;
    [Header("最終連続攻撃判定")] [SerializeField] private GameObject _lastMashAttackObject;

    //攻撃判定のColliderとSpriteRenderer
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

    //ジャンプ用
    [Header("ジャンプ初速度")] [SerializeField] private float _jumpSpeed = 16.0f;
    [Header("重力加速度")] [SerializeField] private float _gravityPower = 30.0f;
    [Header("最低ジャンプ時間")] [SerializeField] private float _jumpLowerTime = 0.05f;
    [Header("調整中・完全停止処理の基準")] [SerializeField] private float _stopMovePoint = 1f;//完全停止処理の基準　x軸だけでよろし？//名前訂正?
    [Header("ジャンプ可能なレイヤー")] [SerializeField] private LayerMask _groundLayer;
    private float _jumpTimer = 0f; // ジャンプ経過時間
    [SerializeField] private bool _isJumpKey = false; // ジャンプキー 
    [SerializeField] private bool _keyLook = false; // ジャンプキー入制限 
    private bool _isGround = false;//レイキャストの設置判定
    [SerializeField] private Status _playerStatus = Status.DOWN;//OnCollisionの設置判定
    private bool _isJump = false;//ジャンプ中

    //プレイヤー状態 上下
    enum Status {
        GROUND = 1,
        UP = 2,
        DOWN = 3,
    }

    //ダメージ時
    [Header("ノックバックさせる力")] [SerializeField] public float _knockBackPower = 1000;
    [Header("ダメージ時の点滅の間隔")] [SerializeField] float _flashInterval = 0.02f;
    [Header("点滅させるときのループカウント")] [SerializeField] int _loopCount = 60;
    [Header("無敵カウント")] [SerializeField] private int _mutekiCount = 5;
    private SpriteRenderer _mySpriteRenderer;//点滅させるためのSpriteRenderer
    [SerializeField] private STATE _damageState = STATE.NOMAL;//プレイヤーのダメージ状態 //名前訂正?
    private Damage _damageScript;//ダメージ判定用スクリプト接続 //名前訂正?
    private float _knockBackDirection;//ノックバック方向 //名前訂正?

    //プレイヤーの状態ダメージ用（ノーマル、ダメージ、無敵）
    enum STATE {
        NOMAL,
        DAMAGED,
        MUTEKI
    }
    //アニメーション用
    Animator _animator;
    //SE用
    SEScript _objectSE;
    #endregion

    private Attack _attack = Attack.AIR;
    enum Attack {
        NOMALDASHMASH,
        AIR,
        MASHPUSH,
        DAMAGE,
    }
    private Collider2D _myCollider;
    bool _isAirAttack = false;
    float _airTime = 0.35f;

    //ここから追加！！！！！！
    
    private SubWeapon _subWeapon;

    //宝箱の判定

    public bool _isPlayeropenBox = default;

    bool _isAttack = false;

    void Start() {

        //Rigidbody2Dを取得
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //プレイヤー座標取得
        _transform = transform;
        _prevPosition = _transform.position;

        //SpriteRenderer格納
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Damageスクリプト取得
        _damageScript = gameObject.transform.Find("PlayerBody").GetComponent<Damage>();

        //SEScriptの取得
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();

        //アニメーターの取得
        _animator = GetComponent<Animator>();

        //攻撃判定のColliderとSpriteRenderer　取得
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

        _myCollider = GetComponent<CapsuleCollider2D>();

        // ここから追加！！！！！！！！！
        _subWeapon = gameObject.GetComponent<SubWeapon>();


    

}
    void Update() {
        if (!_isPlayeropenBox)
        {
            #region//常時処理
            //攻撃間隔計測
            if (_normalAttackIntervalTimer > 0.0f)
            {
                _normalAttackIntervalTimer -= Time.deltaTime;
            }

            if (_dashAttackIntervalTimer > 0.0f)
            {
                _dashAttackIntervalTimer -= Time.deltaTime;
            }

            if (_airAttackIntervalTimer > 0.0f)
            {
                _airAttackIntervalTimer -= Time.deltaTime;
            }

            if (_mashAttackIntervalTimer > 0.0f)
            {
                _mashAttackIntervalTimer -= Time.deltaTime;
            }

            //接地判定レイキャスト
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, _groundLayer);
            Debug.DrawRay(transform.position, Vector2.down * 2f, Color.red);
            if (hit)
            {
                _isGround = true;
            }
            else
            {
                _isGround = false;
            }

            //左右反転
            Vector3 langle = this.transform.eulerAngles;
            if (langle.y == 0 && _isLeft == true)
            {
                langle.y = 180;
            }
            else if (langle.y == 180 && _isLeft == false)
            {
                langle.y = 0;
            }
            this.transform.eulerAngles = new Vector3(0, langle.y, 0);
            #endregion

            //switch (_attack) {
            //    case Attack.AIR:
            //        if (Input.GetButtonDown("Attack") && _airAttackIntervalTimer <= 0.0f && _canAirAttack == true) {
            //            //SEとアニメーション
            //            _objectSE.SwordSE();
            //            _animator.Play("playerSkySlash");

            //            //空中攻撃回数制限用
            //            _canAirAttack = false;

            //            //攻撃判定の出現 
            //            _airAttackObjectCollider.enabled = true;
            //            _airAttackObjectSpriteRenderer.enabled = true;

            //            //攻撃判定破壊遅延
            //            Invoke("AirAttackObjectDestroy", _attckEreaTime);

            //            //インターバル開始
            //            _normalAttackIntervalTimer = _normalAttackInterval;//?いるか、空中→通常の間
            //            _airAttackIntervalTimer = _airAttackInterval;
            //        }
            //        break;
            //    //case Attack.AIR:
            //    //    break;
            //    //case Attack.AIR:
            //    //    break;
            //    //case Attack.AIR:
            //    //    break;
            //    //case Attack.AIR:
            //    //    break;

            //    default:
            //        break;
            //}
            // ジャンプキー入力取得
            if (Input.GetButton("Jump") && _canPlayerMove /*&& state != STATE.DAMAGED*/)
            {
                _isJumpKey = !_keyLook;
            }
            else
            {
                _isJumpKey = false;
                _keyLook = false;
            }
            #region //攻撃
            //ダメージ時の操作不能
            if (_damageState == STATE.DAMAGED)
            {
                return;
            }


            //連続攻撃の連打入力用
            if (Input.GetButtonDown("Attack") && _isMashAttack)
            {
                _mashCount++;
            }

            //ダッシュ攻撃　dashAttack
            else if (Input.GetButtonDown("Attack") && _dashAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND
                && _horizontalPower != 0)
            {
                _isAttack = true;

                //SEとアニメーション
                _objectSE.SwordSE();
                _animator.Play("playerHiAttack");

                //攻撃中の操作停止
                _canPlayerMove = false;
                Invoke("PlayerMoveTrue", _attackDelay);

                //攻撃中であること
                _isDashAttack = true;
                Invoke("DasshAttackBoolFalse", _mashTime);

                //ダッシュ移動
                if (_isLeft)
                {
                    Vector2 force = new Vector2(-_dashForce, 0);
                    _rigidbody2D.AddForce(force);
                }
                else if (!_isLeft)
                {
                    Vector2 force = new Vector2(_dashForce, 0);
                    _rigidbody2D.AddForce(force);
                }

                //攻撃判定の出現
                _dashAttackObjectCollider.enabled = true;
                _dashAttackObjectSpriteRenderer.enabled = true;

                //攻撃判定破壊遅延
                Invoke("DashAttackObjectDestroy", _attckEreaTime);

                //インターバル開始
                _normalAttackIntervalTimer = _normalAttackInterval;
                _dashAttackIntervalTimer = _dashAttackInterval;

                _myCollider.isTrigger = true;

            }

            //空中攻撃 airAttack
            else if (Input.GetButtonDown("Attack") && _airAttackIntervalTimer <= 0.0f
                && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && _canAirAttack == true)//空中攻撃左
            {
                _isAttack = true;

                //SEとアニメーション
                _objectSE.SwordSE();
                _animator.Play("playerSkySlash");

                //空中攻撃回数制限用
                _canAirAttack = false;

                //攻撃中であること　これを条件に連続攻撃へ派生？
                _isAirAttack = true;
                Invoke("AirAttackBoolFalse", _airTime);

                //攻撃判定の出現 
                _airAttackObjectCollider.enabled = true;
                _airAttackObjectSpriteRenderer.enabled = true;

                //攻撃判定破壊遅延
                Invoke("AirAttackObjectDestroy", _attckEreaTime);

                //インターバル開始
                _normalAttackIntervalTimer = _normalAttackInterval;//?いるか、空中→通常の間
                _airAttackIntervalTimer = _airAttackInterval;
            }

            //通常攻撃 normalAttack
            else if (Input.GetButtonDown("Attack") && _normalAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND
                && _horizontalPower == 0 && !_isMashAttack)//地上停止攻撃左
            {
                _isAttack = true;

                //SEとアニメーション
                _objectSE.SwordSE();
                _animator.Play("playerAttack");

                //攻撃中の操作停止
                _canPlayerMove = false;
                Invoke("NormalAttackPlayerMoveTrue", _normalDelay);

                //攻撃中であること　これを条件に連続攻撃へ派生？
                _isNormalAttack = true;
                Invoke("NormalAttackBoolFalse", _normalTime);

                //攻撃判定の出現        
                _normalAttackObjectCollider.enabled = true;
                _normalAttackObjectSpriteRenderer.enabled = true;

                //攻撃判定破壊遅延
                Invoke("NormalAttackObjectDestroy", _attckEreaTime);

                //インターバル開始
                _normalAttackIntervalTimer = _normalAttackInterval;
                _mashAttackIntervalTimer = _mashAttackInterval;

            }

            //連続攻撃　mashAttack
            else if (Input.GetButtonDown("Attack") && _playerStatus == Status.GROUND && _horizontalPower == 0
                && _isNormalAttack && _mashAttackIntervalTimer <= 0.0f && !_isDashAttack)//地上停止攻撃左
            {
                _isAttack = true;

                //SEとアニメーション
                _objectSE.SwordSE();
                _animator.Play("playerAttack");

                //攻撃中判定
                _isMashAttack = true;
                Invoke("MashAttackSecondNext", _mashTime);

                //攻撃判定の出現
                _mashAttackObjectCollider.enabled = true;
                _mashAttackObjectSpriteRenderer.enabled = true;

                //攻撃判定破壊遅延
                Invoke("MashAttackObjectDestroy", _attckEreaTime);

            }
            else if (!_isAttack)
            {
                _subWeapon.SubWeaponUpdate();
            }
        }
            #endregion
    }

    private void FixedUpdate() {

        if (!_isPlayeropenBox)
        {
            #region//プレイヤー制動
            // 現在位置
            Vector2 position = _transform.position;

            // 前フレーム位置
            Vector2 delta = _prevPosition;

            // 前フレーム位置更新
            _prevPosition = position;
            float delX = position.x - delta.x;

            //プレイヤー速度
            _speedX = Mathf.Abs(this._rigidbody2D.velocity.x);
            _speedY = Mathf.Abs(this._rigidbody2D.velocity.y);

            //横移動減速
            if (_speedX > 0.1f)
            {
                this._rigidbody2D.AddForce(new Vector2(delX * -_decelerationSpeed, 0));
            }

            ////完全停止処理
            if (_speedX + _speedY <= _stopMovePoint)
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
            #endregion

            #region//ジャンプ処理
            //ジャンプ用変数
            Vector2 newvec = new Vector2(_rigidbody2D.velocity.x, 0);

            //プレイヤー状態Y軸
            switch (_playerStatus)
            {

                // 接地時
                case Status.GROUND:

                    // 接地時→上昇時
                    if (_isJumpKey)
                    {
                        _playerStatus = Status.UP;
                    }
                    break;

                // 上昇時
                case Status.UP:
                    //ジャンプ開始
                    if (!_isJump)
                    {
                        _objectSE.JumpSE();
                        _isJump = true;
                        _animator.Play("playerJump");
                        _animator.SetBool("Jump", true);
                        _attack = Attack.AIR;
                    }

                    //ジャンプ経過時間処理
                    _jumpTimer += Time.deltaTime;

                    //上昇
                    if (_isJumpKey || _jumpLowerTime > _jumpTimer)
                    {
                        newvec.y = _jumpSpeed;
                        newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));//jumpTimer^2
                    }
                    else
                    {// 落下を早める
                        _jumpTimer += Time.deltaTime;
                        newvec.y = _jumpSpeed;
                        newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));
                    }
                    // 上昇時→落下時
                    if (0f > newvec.y)
                    {
                        _playerStatus = Status.DOWN;
                        newvec.y = 0f;
                        _jumpTimer = 0.1f;
                    }
                    break;

                // 落下時
                case Status.DOWN:

                    //落下
                    if (_attack != Attack.AIR)
                    {
                        _attack = Attack.AIR;
                    }

                    _jumpTimer += Time.deltaTime;
                    newvec.y = 0f;
                    newvec.y -= (_gravityPower * _jumpTimer);//調整必要　下候補//newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                    break;

                default:
                    break;
            }

            //Y軸移動処適用
            _rigidbody2D.velocity = newvec;
            #endregion

            #region//横移動処理
            // ジョイスティック右・左の数値化
            _horizontalPower = Input.GetAxisRaw("Horizontal");

            //横移動
            if (_canPlayerMove)
            {//プレイヤー操作制限

                //右 移動    
                if (Input.GetKey(KeyCode.D) || _horizontalPower > 0)
                {
                    _isLeft = false;
                    _rigidbody2D.AddForce(Vector2.right * ((_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));

                    //アニメーション
                    if (!_isJump)
                    {
                        _animator.Play("playerWalk");
                    }

                }

                //左  移動     
                if (Input.GetKey(KeyCode.A) || _horizontalPower < 0)
                {
                    _isLeft = true;
                    _rigidbody2D.AddForce(Vector2.right * ((-_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));

                    //アニメーション
                    if (!_isJump)
                    {
                        _animator.Play("playerWalk");
                    }
                }

            }
        }
            #endregion
    }

    //ダメージ時の行動
    public void DamageSet() {
        Debug.Log("EA4");
        //変更 ノーマルじゃない（ダメージ中、無敵中）ときはリターン
        if (_damageState != STATE.NOMAL || _isDashAttack) {
            return;
        }
        //if (_damageState != STATE.MUTEKI) {

        //}
        Debug.Log("EA5");
        StartCoroutine(DamageKnockBack());

    }

    IEnumerator DamageKnockBack() {//要訂正？
        _rigidbody2D.velocity = Vector2.zero;
        float dist = transform.position.x - _damageScript._attackEnemy.transform.position.x;

        if (dist >= 0) {
            _knockBackDirection = 1;
        } else {
            _knockBackDirection = -1;
        }

        if (_isPlayeropenBox)
        {
            
        }
        else if (_isAirAttack) {
            //ノックバック　変数にしろ
            _rigidbody2D.AddForce(new Vector2(1.5f * _knockBackDirection, 7) * _knockBackPower, 0);//?
            _objectSE.JumpSE();
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);
        } else {
            Debug.Log("EA6");
            //ノックバック　変数にしろ
            _rigidbody2D.AddForce(new Vector2(1 * _knockBackDirection, 5) * _knockBackPower, 0);
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);

            _damageState = STATE.DAMAGED;
            //_playerStatus = Status.DOWN;


            _objectSE.KnockBackSE();
            for (int i = 0; i < _loopCount; i++) {

                yield return new WaitForSeconds(_flashInterval);
                _mySpriteRenderer.enabled = false;
                yield return new WaitForSeconds(_flashInterval);
                _mySpriteRenderer.enabled = true;

                if (i > _mutekiCount)//？？？
                {
                    _damageState = STATE.MUTEKI;

                }

            }

            _damageState = STATE.NOMAL;
        }

        

    }
    #region//訂正完了

    //連続攻撃二撃目以降
    void MashAttackSecondNext() {

        if (_mashCount > _mashCountLimit && _mashAttackSecondNextCount < _mashAttackSecondNextCountLimit) {
            //SEとアニメーション
            _objectSE.SwordSE();
            _animator.Play("playerAttack");

            //連続攻撃二撃目以降遅延呼び出し
            Invoke("MashAttackSecondNext", _mashTime);

            //連続攻撃の連打入力回数のリセット
            _mashCount = 0;

            //連続攻撃の回数カウント処理
            _mashAttackSecondNextCount++;

            //攻撃判定の出現 
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;

            //攻撃判定破壊遅延
            Invoke("MashAttackObjectDestroy", _attckEreaTime);

        } else {
            //SEとアニメーション
            _objectSE.SwordSE();
            _animator.Play("playerAttack");

            //連続攻撃の連打入力回数のリセット
            _mashCount = 0;

            //連続攻撃の回数のリセット
            _mashAttackSecondNextCount = 0;

            //攻撃中であることの停止とその遅延
            Invoke("MashAttackBoolFalse", _mashTime);

            //攻撃判定の出現 
            _lastMashAttackObjectCollider.enabled = true;
            _lastMashAttackObjectSpriteRenderer.enabled = true;

            //攻撃判定破壊遅延
            Invoke("LastMashAttackObjectDestroy", _attckEreaTime);
            Invoke("PlayerMoveTrue", _attackDelay);

            //インターバル開始
            _normalAttackIntervalTimer = _normalAttackInterval;
        }
    }

    //地面の認識
    void OnCollisionStay2D(Collision2D collision) {
        //着地時の処理
        if (_isGround && _playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            //→接触時
            _playerStatus = Status.GROUND;

            //SEとアニメーション
            _isJump = false;//se
            _animator.SetBool("Jump", false);

            //ジャンプ経過時間リセット
            _jumpTimer = 0f;
            _keyLook = true; // キー操作をロックする

            //空中攻撃回数回復
            _canAirAttack = true;
        }
    }
    //地面から離れた場合の処理
    void OnCollisionExit2D(Collision2D collision) {
        //ジャンプせずに地面から離れた場合
        if (_playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//地面判定変更可
        {
            //接地時→落下時
            _playerStatus = Status.DOWN;

            //空中攻撃回数回復
            _canAirAttack = true;
        }
    }

    //攻撃時プレイヤー操作制限解除
    void PlayerMoveTrue() {
        _canPlayerMove = true;
        _isAttack = false;
    }
    void NormalAttackPlayerMoveTrue()//通常攻撃時プレイヤー操作制限解除　
    {
        if (!_isDashAttack && !_isMashAttack) {//ダッシュ攻撃、連続攻撃に派生した場合は解除しない
            _canPlayerMove = true;
            _isAttack = false;
        }
    }

    //攻撃中を停止
    void NormalAttackBoolFalse() {//通常攻撃中を停止
        _isNormalAttack = false;
    }
    void DasshAttackBoolFalse() //ダッシュ攻撃中を停止
        {
        _isDashAttack = false;
    }
    void MashAttackBoolFalse()//連続攻撃中を停止
    {
        _isMashAttack = false;
    }
    void AirAttackBoolFalse()//連続攻撃中を停止
    {
        _isAirAttack = false;
    }

    //攻撃判定消去
    void NormalAttackObjectDestroy() {
        _normalAttackObjectCollider.enabled = false;
        _normalAttackObjectSpriteRenderer.enabled = false;
    }
    void DashAttackObjectDestroy() {
        _dashAttackObjectCollider.enabled = false;
        _dashAttackObjectSpriteRenderer.enabled = false;

        _myCollider.isTrigger = false;
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
    #endregion
}
