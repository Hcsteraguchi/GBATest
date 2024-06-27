using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour
{
    #region //�ϐ� 
    //Rigidbory2D
    private Rigidbody2D _rigidbody2D;

    //�����p
    private float _speedX;//X���̃v���C���[�̑��x�̐�Βl
    private float _speedY;//Y���̃v���C���[�̑��x�̐�Βl
    private float _horizontalPower;//�W���C�X�e�B�b�N���E���͂̐��l���i�E�����j
    private float _verticalPower;//�W���C�X�e�B�b�N�㉺���͂̐��l���i�������j
    private Transform _transform;//transform���擾
    private Vector3 _prevPosition;//�O�t���[���̈ʒu�擾

    //���ړ��p    
    [Header("���ړ����x")] [SerializeField] private float _sideSpeed = 10.0f;
    [Header("���ړ������x")] [SerializeField] private float _sidePower = 20.0f;
    [Header("���ړ��������x")] [SerializeField] private float _decelerationSpeed = 200.0f;
    public bool _isLeft = false;//���A���������Ă��邩�i�ۂȂ�E�������Ă���j
    private bool _canPlayerMove = true;//�W�����v�Ɖ��ړ��̋���



    // �U���p    
    [Header("�_�b�V���U�����x")] [SerializeField] private float _dashForce = 600f;
    [Header("�A���U�����s�A�ŉ�")] [SerializeField] private float _mashCountLimit = 2.0f;
    [Header("�A�Ń��[�V������")] [SerializeField] private int _mashAttackSecondNextCountLimit = 4;
    private bool _canAirAttack = true;//�󒆍U���\
    private bool _canAirDownAttack = true;//�󒆗����U���\
    private bool _canAirUpAttack = true;//�󒆗����U���\
    private bool _isNormalAttack = default;//�U�����@�n���~
    private bool _isDashAttack = default;//�U�����@�n��ړ�
    private bool _isMashAttack = default;//�U�����@�n��A��
    private int _mashCount = 0;//�A�ŉ񐔃J�E���g 
    private int _mashAttackSecondNextCount = 0;//�A�Ń��[�V�����J�E���g 

    //�U���Ԋu����p
    //[Header("�ʏ�U�� �Ďg�p����")][SerializeField] private float _normalAttackInterval = 0.75f;    [Header("�_�b�V���U�� �Ďg�p����")][SerializeField] private float _dashAttackInterval = 0.5f;    [Header("�󒆍U���@�@�Ďg�p����")][SerializeField] private float _airAttackInterval = 0.75f;   [Header("�ʏ큨�A���ԁ@�g�p�Ԋu")][SerializeField] private float _mashAttackInterval = 0.5f;
    [Header("�������E�U����̌㌄�i���̑��j�������ׂ��H")] [SerializeField] private float _attackDelay = 0.8f;//�v����!!  �������ׂ��H  �U����̌㌄ 
    [Header("�A���U����t����")] [SerializeField] float _mashTime = 1.0f;
    [Header("�n��ʏ�U����������")] [SerializeField] private float _normalTime = 1.0f;
    [Header("�n��ʏ�U����̌㌄")] [SerializeField] private float _normalDelay = 1.0f;
    //    private float _normalAttackIntervalTimer = 0.0f;//�U�� �n���~ �Ďg�p���ԗp    private float _dashAttackIntervalTimer = 0.0f;//�U�� �n��ړ� �Ďg�p���ԗp    private float _airAttackIntervalTimer = 0.0f;//�U���@�󒆁@�@�Ďg�p���ԗp   private float _mashAttackIntervalTimer = 0.0f;//�U���@�A���@�@�Ďg�p���ԗp

    [Header("�U�� �Ďg�p����")] [SerializeField] private float[] _attackInterval = new float[] { 1, 1, 0.75f, 0.5f };
    private float[] _attackIntervalTimer = new float[] { 0, 0, 0, 0 };//�U��  �Ďg�p���ԗp

    //�U������
    [Header("�U�����葶�ݎ���")] [SerializeField] float _attckEreaTime = 0.3f;
    //[Header("�ʏ�U������")] [SerializeField] private GameObject _normalAttackObject;   [Header("�_�b�V���U������")] [SerializeField] private GameObject _dashAttackObject;    [Header("�󒆍U������")] [SerializeField] private GameObject _airAttackObject;   [Header("�A���U�����蔻��")] [SerializeField] private GameObject _mashAttackObject;[Header("�ŏI�A���U������")] [SerializeField] private GameObject _lastMashAttackObject;
    [Header("�U������")] [SerializeField] private GameObject[] _attackObject = new GameObject[5];

    //�U�������Collider��SpriteRenderer
    // private Collider2D _normalAttackObjectCollider;  private SpriteRenderer _normalAttackObjectSpriteRenderer;  private Collider2D _dashAttackObjectCollider;  private SpriteRenderer _dashAttackObjectSpriteRenderer;   private Collider2D _airAttackObjectCollider;   private SpriteRenderer _airAttackObjectSpriteRenderer;   private Collider2D _mashAttackObjectCollider;   private SpriteRenderer _mashAttackObjectSpriteRenderer;   private Collider2D _lastMashAttackObjectCollider;    private SpriteRenderer _lastMashAttackObjectSpriteRenderer;

    private Collider2D[] _attackObjectCollider = new Collider2D[5];
    private SpriteRenderer[] _attackObjectSpriteRenderer = new SpriteRenderer[5];

    //�W�����v�p
    [Header("�W�����v�����x")] [SerializeField] private float _jumpSpeed = 16.0f;
    [Header("�d�͉����x")] [SerializeField] private float _gravityPower = 30.0f;
    [Header("�Œ�W�����v����")] [SerializeField] private float _jumpLowerTime = 0.05f;
    [Header("�������E���S��~�����̊")] [SerializeField] private float _stopMovePoint = 1f;//���S��~�����̊�@x�������ł�낵�H//���O����?
    [Header("�W�����v�\�ȃ��C���[")] [SerializeField] private LayerMask _groundLayer;
    private float _jumpTimer = 0f; // �W�����v�o�ߎ���
    [SerializeField] private bool _isJumpKey = false; // �W�����v�L�[ 
    [SerializeField] private bool _keyLook = false; // �W�����v�L�[������ 
    private bool _isGround = false;//���C�L���X�g�̐ݒu����
    [SerializeField] public Status _playerStatus = Status.DOWN;//OnCollision�̐ݒu����
    private bool _isJump = false;//�W�����v��

    //�v���C���[��� �㉺
    public enum Status
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3,
    }

    //�_���[�W��
    [Header("�m�b�N�o�b�N�������")] [SerializeField] public float _knockBackPower = 1000;
    [Header("�_���[�W���̓_�ł̊Ԋu")] [SerializeField] float _flashInterval = 0.02f;
    [Header("�_�ł�����Ƃ��̃��[�v�J�E���g")] [SerializeField] int _loopCount = 60;
    [Header("���G�J�E���g")] [SerializeField] private int _mutekiCount = 5;
    private SpriteRenderer _mySpriteRenderer;//�_�ł����邽�߂�SpriteRenderer
    [SerializeField] private STATE _damageState = STATE.NOMAL;//�v���C���[�̃_���[�W��� //���O����?
    private DamagePlayer _damageScript;//�_���[�W����p�X�N���v�g�ڑ� //���O����?
    private float _knockBackDirection;//�m�b�N�o�b�N���� //���O����?

    //�v���C���[�̏�ԃ_���[�W�p�i�m�[�}���A�_���[�W�A���G�j
    enum STATE
    {
        NOMAL,
        DAMAGED,
        MUTEKI
    }
    //�A�j���[�V�����p
    Animator _animator;
    //SE�p
    SEScript _objectSE;
    #endregion

    private Attack _attack = Attack.AIR;
    enum Attack
    {
        NOMALDASHMASH,
        AIR,
        MASHPUSH,
        DAMAGE,
    }
    private Collider2D _myCollider;
    bool _isAirAttack = false;
    float _airTime = 0.35f;

    //��������ǉ��I�I�I�I�I�I

    private SubWeapon _subWeapon;

    //�󔠂̔���

    public bool _isPlayeropenBox = default;

    bool _isAttack = false;

    private int HP = 100;
    public int PlayerHP
    {
        get { return HP; }
        //set { HP = value; }
    }
    [Header("�{�X�̍U����")] [SerializeField] private int _bossPower = default;
    [Header("�G���G�̍U����")] [SerializeField] private int _enemyAttack = default;
    // �X���C�_�[�̐��l�ύX
    [Header("�Q�[���I�[�o�[�V�[���Ɉڍs����܂ł̎���")]
    [SerializeField] private float _changeSceneTime = default;
    [Header("�X���C�_�[�Q��")] [SerializeField] private SliderUI _sliderUI = default;
    [Header("�Ή��X���C�_�[")] [SerializeField] private GameObject _sliderObject = default;

    private bool _isMenu = false;
    public bool IsMenu
    {
        get { return _isMenu; }
        set { _isMenu = value; }
    }
    private bool _isDead = false;

    DashBack _dashBack;

    bool _stopMash = false;

    int x = 0;

    
    private float _joySticLimit = 0.8f;

    void Start()
    {

        //Rigidbody2D���擾
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //�v���C���[���W�擾
        _transform = transform;
        _prevPosition = _transform.position;

        //SpriteRenderer�i�[
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Damage�X�N���v�g�擾
        _damageScript = gameObject.transform.Find("PlayerBody").GetComponent<DamagePlayer>();

        //SEScript�̎擾
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();

        //�A�j���[�^�[�̎擾
        _animator = GetComponent<Animator>();

        //�U�������Collider��SpriteRenderer�@���[�v�ōs����H�����R���C�_�[���Ⴄ
        _attackObjectCollider[0] = _attackObject[0].GetComponent<CapsuleCollider2D>();
        _attackObjectCollider[1] = _attackObject[1].GetComponent<BoxCollider2D>();
        _attackObjectCollider[2] = _attackObject[2].GetComponent<CircleCollider2D>();
        _attackObjectCollider[3] = _attackObject[3].GetComponent<BoxCollider2D>();
        _attackObjectCollider[4] = _attackObject[4].GetComponent<BoxCollider2D>();
        for (int i = 0; i < 5; i++)
        {
            _attackObjectSpriteRenderer[i] = _attackObject[i].GetComponent<SpriteRenderer>();
        }

        _myCollider = GetComponent<CapsuleCollider2D>();

        // ��������ǉ��I�I�I�I�I�I�I�I�I
        _subWeapon = gameObject.GetComponent<SubWeapon>();

        _sliderUI = _sliderObject.GetComponent<SliderUI>();

        _dashBack = transform.Find("DashAttack").gameObject.GetComponent<DashBack>();


    }
    void Update()
    {
        if (!_isPlayeropenBox && !_isMenu && !_isDead)
        {
            #region//�펞����
            //�U���Ԋu�v���@���[�v�C�P���H
            if (_attackIntervalTimer[0] > 0.0f)
            {
                _attackIntervalTimer[0] -= Time.deltaTime;
            }

            if (_attackIntervalTimer[1] > 0.0f)
            {
                _attackIntervalTimer[1] -= Time.deltaTime;
            }

            if (_attackIntervalTimer[2] > 0.0f)
            {
                _attackIntervalTimer[2] -= Time.deltaTime;
            }

            if (_attackIntervalTimer[3] > 0.0f)
            {
                _attackIntervalTimer[3] -= Time.deltaTime;
            }

            //�ڒn���背�C�L���X�g
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, _groundLayer);
            Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);
            if (hit)
            {
                _isGround = true;
            }
            else
            {
                _isGround = false;
            }

            //���E���]
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
            //            //SE�ƃA�j���[�V����
            //            _objectSE.SwordSE();
            //            _animator.Play("playerSkySlash");

            //            //�󒆍U���񐔐����p
            //            _canAirAttack = false;

            //            //�U������̏o�� 
            //            _airAttackObjectCollider.enabled = true;
            //            _airAttackObjectSpriteRenderer.enabled = true;

            //            //�U������j��x��
            //            Invoke("AirAttackObjectDestroy", _attckEreaTime);

            //            //�C���^�[�o���J�n
            //            _normalAttackIntervalTimer = _normalAttackInterval;//?���邩�A�󒆁��ʏ�̊�
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
            // �W�����v�L�[���͎擾
            if (Input.GetButton("Jump") && _canPlayerMove /*&& state != STATE.DAMAGED*/)
            {
                _isJumpKey = !_keyLook;
            }
            else
            {
                _isJumpKey = false;
                _keyLook = false;
            }
            #region //�U��
            //�_���[�W���̑���s�\
            if (_damageState == STATE.DAMAGED)
            {
                return;
            }


            //�A���U���̘A�œ��͗p
            

            //�_�b�V���U���@dashAttack
            if (Input.GetButtonDown("Attack") && _attackIntervalTimer[1] <= 0.0f && _playerStatus == Status.GROUND
                && (_horizontalPower > _joySticLimit || _horizontalPower < -_joySticLimit))
            {
                x = 1;
                AllAttack();
                //_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;

                //�A�j���[�V����            
                _animator.Play("playerHiAttack");

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("PlayerMoveTrue", _attackDelay);

                //�U�����ł��邱��
                _isDashAttack = true;
                Invoke("DasshAttackBoolFalse", _mashTime);

                //�_�b�V���ړ�
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

                //�C���^�[�o���J�n
                _attackIntervalTimer[0] = _attackInterval[0];
                _attackIntervalTimer[1] = _attackInterval[1];
                //_attackIntervalTimer[2] = _attackInterval[2];

                //_myCollider.isTrigger = true;

            }
            //�؂�グ��
            else if (Input.GetButtonDown("Attack") && _attackIntervalTimer[2] <= 0.0f
                && _verticalPower < -_joySticLimit
                && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && !_isGround && _canAirUpAttack == true)
            {
                _playerStatus = Status.UP;
                x = 3;
                AllAttack();

                //�A�j���[�V����              
                _animator.Play("playerAttack");

                //�U��������
                _isMashAttack = true;


                //�U�����ł��邱�Ƃ̒�~�Ƃ��̒x��
                Invoke("MashAttackBoolFalse", _mashTime);

                //�󒆍U���񐔐����p
                _canAirUpAttack = false;

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("PlayerMoveTrue", _attackDelay);

                

                //�_�b�V���ړ�
                if (_isLeft)
                {
                    Vector2 force = new Vector2(-500, 8000);
                    _rigidbody2D.AddForce(force);
                }
                else if (!_isLeft)
                {
                    Vector2 force = new Vector2(500, 8000);
                    _rigidbody2D.AddForce(force);
                }

                //�C���^�[�o���J�n
                _attackIntervalTimer[2] = _attackInterval[2];
            }
            //�؂�グ
            else if(Input.GetButtonDown("Attack") && _attackIntervalTimer[1] <= 0.0f 
                && _verticalPower < -_joySticLimit && _playerStatus == Status.GROUND)
                
            {
                _playerStatus = Status.UP;
                x = 3;
                AllAttack();

                //�A�j���[�V����              
                _animator.Play("playerAttack");

                //�U��������
                _isMashAttack = true;
                

                //�U�����ł��邱�Ƃ̒�~�Ƃ��̒x��
                Invoke("MashAttackBoolFalse", _mashTime);

                //�󒆍U���񐔐����p
                _canAirUpAttack = false;

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("PlayerMoveTrue", _attackDelay);

                

                //�_�b�V���ړ�
                if (_isLeft)
                {
                    Vector2 force = new Vector2(-500, 8000);
                    _rigidbody2D.AddForce(force);
                }
                else if (!_isLeft)
                {
                    Vector2 force = new Vector2(500, 8000);
                    _rigidbody2D.AddForce(force);
                }

                //�C���^�[�o���J�n


                _attackIntervalTimer[2] = _attackInterval[2];
            }
            //�؂艺��
            else if (Input.GetButtonDown("Attack") && _attackIntervalTimer[2] <= 0.0f
                && ( _verticalPower > _joySticLimit) 
                && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && !_isGround && _canAirDownAttack == true)
            {
                _playerStatus = Status.DOWN;
                x = 3;
                AllAttack();

                //�A�j���[�V����              
                _animator.Play("playerAttack");

                //�U��������
                _isMashAttack = true;
                

            

            //�󒆍U���񐔐����p
            _canAirDownAttack = false;

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("PlayerMoveTrue", _attackDelay);

                Invoke("MashAttackBoolFalse", _mashTime);

                //�_�b�V���ړ�
                if (_isLeft)
                {
                    Vector2 force = new Vector2(-500, -6000);
                    _rigidbody2D.AddForce(force);
                }
                else if (!_isLeft)
                {
                    Vector2 force = new Vector2(500, -6000);
                    _rigidbody2D.AddForce(force);
                }

                //�C���^�[�o���J�n
                _attackIntervalTimer[0] = _attackInterval[0];
                _attackIntervalTimer[1] = _attackInterval[1];
                _attackIntervalTimer[2] = _attackInterval[2];
            }
            //�󒆍U�� airAttack
            else if (Input.GetButtonDown("Attack") && _attackIntervalTimer[2] <= 0.0f
                && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && _canAirAttack == true && !_isGround)//�󒆍U����
            {
                x = 2;
                AllAttack();

                //�A�j���[�V����               
                _animator.Play("playerSkySlash");

                //�󒆍U���񐔐����p
                _canAirAttack = false;

                //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
                _isAirAttack = true;
                Invoke("AirAttackBoolFalse", _airTime);

                //�C���^�[�o���J�n
                _attackIntervalTimer[0] = _attackInterval[0];//?���邩�A�󒆁��ʏ�̊�
                _attackIntervalTimer[1] = _attackInterval[1];
                _attackIntervalTimer[2] = _attackInterval[2];
                
            }

            //�ʏ�U�� normalAttack
            else if (Input.GetButtonDown("Attack") && _attackIntervalTimer[0] <= 0.0f && _playerStatus == Status.GROUND
                && _horizontalPower == 0 && !_isMashAttack)//�n���~�U����
            {
                x = 0;
                AllAttack();

                //�A�j���[�V����              
                _animator.Play("playerAttack");

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("NormalAttackPlayerMoveTrue", _normalDelay);

                //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
                _isNormalAttack = true;
                Invoke("NormalAttackBoolFalse", _normalTime);

                //�C���^�[�o���J�n
                _attackIntervalTimer[0] = _attackInterval[0];
                _attackIntervalTimer[3] = _attackInterval[3];

            }

            ////�A���U���@mashAttack
            //else if (Input.GetButtonDown("Attack") && _playerStatus == Status.GROUND && _horizontalPower == 0
            //    && _isNormalAttack && _attackIntervalTimer[3] <= 0.0f && !_isDashAttack)//�n���~�U����
            //{
            //    x = 3;
            //    AllAttack();

            //    //�A�j���[�V����              
            //    _animator.Play("playerAttack");

            //    //�U��������
            //    _isMashAttack = true;
            //    Invoke("MashAttackSecondNext", _mashTime);

            //}
            else if (!_isAttack)
            {
                _subWeapon.SubWeaponUpdate();
            }
        }
        #endregion
        if (HP <= 0)
        {
            _isDead = true;
            _animator.Play("playerDie");
            _mySpriteRenderer.color = Color.black;
            _rigidbody2D.simulated = false;
            PlayerDead();
        }
    }

    private void FixedUpdate()
    {

        if (!_isPlayeropenBox && !_isDead)
        {
            #region//�v���C���[����
            // ���݈ʒu
            Vector2 position = _transform.position;

            // �O�t���[���ʒu
            Vector2 delta = _prevPosition;

            // �O�t���[���ʒu�X�V
            _prevPosition = position;
            float delX = position.x - delta.x;

            //�v���C���[���x
            _speedX = Mathf.Abs(this._rigidbody2D.velocity.x);
            _speedY = Mathf.Abs(this._rigidbody2D.velocity.y);

            //���ړ�����
            if (_speedX > 0.1f)
            {
                this._rigidbody2D.AddForce(new Vector2(delX * -_decelerationSpeed, 0));
            }

            //���S��~����
            if (_speedX + _speedY <= _stopMovePoint)
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
            #endregion

            #region//�W�����v����
            //�W�����v�p�ϐ�
            Vector2 newvec = new Vector2(_rigidbody2D.velocity.x, 0);

            //�v���C���[���Y��
            switch (_playerStatus)
            {

                // �ڒn��
                case Status.GROUND:

                    // �ڒn�����㏸��
                    if (_isJumpKey)
                    {
                        _playerStatus = Status.UP;
                    }
                    break;

                // �㏸��
                case Status.UP:
                    //�W�����v�J�n
                    if (!_isJump)
                    {
                        _objectSE.JumpSE();
                        _isJump = true;
                        _animator.Play("playerJump");
                        _animator.SetBool("Jump", true);
                        _attack = Attack.AIR;
                    }

                    //�W�����v�o�ߎ��ԏ���
                    _jumpTimer += Time.deltaTime;

                    //�㏸
                    if (_isJumpKey || _jumpLowerTime > _jumpTimer)
                    {
                        newvec.y = _jumpSpeed;
                        newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));//jumpTimer^2
                    }
                    else
                    {// �����𑁂߂�
                        _jumpTimer += Time.deltaTime;
                        newvec.y = _jumpSpeed;
                        newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));
                    }
                    // �㏸����������
                    if (0f > newvec.y)
                    {
                        _playerStatus = Status.DOWN;
                        newvec.y = 0f;
                        _jumpTimer = 0.1f;
                    }
                    break;

                // ������
                case Status.DOWN:

                    //����
                    if (_attack != Attack.AIR)
                    {
                        _attack = Attack.AIR;
                    }

                    _jumpTimer += Time.deltaTime;
                    newvec.y = 0f;
                    newvec.y -= (_gravityPower * _jumpTimer);//�����K�v�@�����//newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                    break;

                default:
                    break;
            }

            //Y���ړ����K�p
            _rigidbody2D.velocity = newvec;
            #endregion

            _verticalPower = Input.GetAxisRaw("Vertical");
            
            
                #region//���ړ�����
                // �W���C�X�e�B�b�N�E�E���̐��l��
                _horizontalPower = Input.GetAxisRaw("Horizontal");


            //���ړ�
            if (_canPlayerMove)
            {//�v���C���[���쐧��

                //�E �ړ�    
                if (Input.GetKey(KeyCode.D) || _horizontalPower > 0)
                {
                    _isLeft = false;
                    _rigidbody2D.AddForce(Vector2.right * ((_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));

                    //�A�j���[�V����
                    if (_isGround && !_isAirAttack)
                    {
                        _animator.Play("playerWalk");
                    }

                }

                //��  �ړ�     
                if (Input.GetKey(KeyCode.A) || _horizontalPower < 0)
                {
                    _isLeft = true;
                    _rigidbody2D.AddForce(Vector2.right * ((-_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));

                    //�A�j���[�V����
                    if (_isGround && !_isAirAttack)
                    {
                        _animator.Play("playerWalk");
                    }
                }

            }
        }
        #endregion
    }

    //�_���[�W���̍s��
    public void DamageSet()
    {

        //�ύX �m�[�}������Ȃ��i�_���[�W���A���G���j�Ƃ��̓��^�[��
        if (_damageState != STATE.NOMAL /*|| _isDashAttack*/)
        {
            return;
        }
        StartCoroutine(DamageKnockBack());

    }

    public void DashBackSet()
    {

        _rigidbody2D.velocity = Vector2.zero;

        if (_isLeft)
        {
            _knockBackDirection = 1;
        }
        else
        {
            _knockBackDirection = -1;
        }
        _rigidbody2D.AddForce(new Vector2(1.5f * _knockBackDirection, 3) * _knockBackPower, 0);//?
        Invoke("DashBackAfter", 0.2f);
        //_myCollider.isTrigger = false; ;
        //_playerStatus = Status.DOWN;
        _objectSE.JumpSE();
        _canPlayerMove = false;
        Invoke(nameof(PlayerMoveTrue), _attackDelay);
    }

    IEnumerator DamageKnockBack()
    {//�v�����H
        _rigidbody2D.velocity = Vector2.zero;
        float dist = transform.position.x - _damageScript._attackEnemy.transform.position.x;

        if (dist >= 0)
        {
            _knockBackDirection = 1;
        }
        else
        {
            _knockBackDirection = -1;
        }

        if (_isPlayeropenBox)
        {

        }
        else if (_isAirAttack || _isDashAttack)
        {
            //�m�b�N�o�b�N�@�ϐ��ɂ���
            _rigidbody2D.AddForce(new Vector2(1.5f * _knockBackDirection, 7) * _knockBackPower, 0);//?
            _objectSE.JumpSE();
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);
        }


        else
        {
            Debug.Log("EA6");
            //�m�b�N�o�b�N�@�ϐ��ɂ���
            _rigidbody2D.AddForce(new Vector2(0.5f * _knockBackDirection, 2) * _knockBackPower, 0);
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);
            //if (_isMashAttack)
            //{
            //    Debug.Log("ren");
            //    _stopMash = true;
            //    _isMashAttack = false;
            //    _mashCount = 0;
            //    _mashAttackSecondNextCount = 0;
            //    Invoke("_stopMashFalse", _attackDelay);

            //}
            _damageState = STATE.DAMAGED;
            //_playerStatus = Status.DOWN;
            if (_damageScript._attackEnemy.tag == "BossAttack")
            {
                HP -= _bossPower;
                _sliderUI.HPSlider();
                print(HP);
            }
            else if (_damageScript._attackEnemy.tag == "Enemy")
            {
                HP -= _enemyAttack;
                _sliderUI.HPSlider();
                print(HP);
            }
            else if (_damageScript._attackEnemy.tag == "EnemyAttack")
            {
                HP -= _enemyAttack;
                _sliderUI.HPSlider();
                print(HP);
            }


            _objectSE.KnockBackSE();
            for (int i = 0; i < _loopCount; i++)
            {

                yield return new WaitForSeconds(_flashInterval);
                _mySpriteRenderer.enabled = false;
                yield return new WaitForSeconds(_flashInterval);
                _mySpriteRenderer.enabled = true;

                if (i > _mutekiCount)//�H�H�H
                {
                    _damageState = STATE.MUTEKI;

                }

            }

            _damageState = STATE.NOMAL;
        }



    }
    /// <summary>
    /// ���S����
    /// </summary>
    private void PlayerDead()
    {
        _changeSceneTime -= Time.deltaTime;
        print(_changeSceneTime);
        if (_changeSceneTime <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }

    }
    void DashBackAfter()
    {

        _playerStatus = Status.DOWN;
    }
    #region//��������

    //�A���U���񌂖ڈȍ~Trigg
    //void MashAttackSecondNext()
    //{

    //    if (_mashCount > _mashCountLimit && _mashAttackSecondNextCount < _mashAttackSecondNextCountLimit && !_stopMash)
    //    {
    //        x = 3;
    //        AllAttack();
    //        //�A�j���[�V����
    //        _animator.Play("playerAttack");

    //        //�A���U���񌂖ڈȍ~�x���Ăяo��
    //        Invoke("MashAttackSecondNext", _mashTime);

    //        //�A���U���̘A�œ��͉񐔂̃��Z�b�g
    //        _mashCount = 0;

    //        //�A���U���̉񐔃J�E���g����
    //        _mashAttackSecondNextCount++;

    //    }
    //    else if (!_stopMash)
    //    {
    //        x = 4;
    //        AllAttack();
    //        //�A�j���[�V����
    //        _animator.Play("playerAttack");

    //        //�A���U���̘A�œ��͉񐔂̃��Z�b�g
    //        _mashCount = 0;

    //        //�A���U���̉񐔂̃��Z�b�g
    //        _mashAttackSecondNextCount = 0;

    //        //�U�����ł��邱�Ƃ̒�~�Ƃ��̒x��
    //        Invoke("MashAttackBoolFalse", _mashTime);

    //        Invoke("PlayerMoveTrue", _attackDelay);

    //        //�C���^�[�o���J�n
    //        _attackIntervalTimer[0] = _attackInterval[0];
    //    }
    //}

    //�n�ʂ̔F��
    void OnCollisionStay2D(Collision2D collision)
    {

        //���n���̏���
        if (_isGround && _playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            //���ڐG��
            _playerStatus = Status.GROUND;

            //SE�ƃA�j���[�V����
            _isJump = false;//se
            _animator.SetBool("Jump", false);

            //�W�����v�o�ߎ��ԃ��Z�b�g
            _jumpTimer = 0f;
            _keyLook = true; // �L�[��������b�N����

            //�󒆍U���񐔉�
            _canAirAttack = true;
            _canAirDownAttack = true;
            _canAirUpAttack = true;
        }
    }
    //�n�ʂ��痣�ꂽ�ꍇ�̏���
    void OnCollisionExit2D(Collision2D collision)
    {
        //�W�����v�����ɒn�ʂ��痣�ꂽ�ꍇ
        if (_playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            //�ڒn����������
            _playerStatus = Status.DOWN;

            //�󒆍U���񐔉�
            _canAirAttack = true;
            _canAirDownAttack = true;
            _canAirUpAttack = true;

        }
    }

    //�U�����v���C���[���쐧������
    void PlayerMoveTrue()
    {
        _canPlayerMove = true;
        _isAttack = false;
    }
    void NormalAttackPlayerMoveTrue()//�ʏ�U�����v���C���[���쐧�������@
    {
        if (!_isDashAttack && !_isMashAttack)
        {//�_�b�V���U���A�A���U���ɔh�������ꍇ�͉������Ȃ�
            _canPlayerMove = true;
            _isAttack = false;
        }
    }

    void _stopMashFalse()
    {
        _stopMash = false;
    }


    //�U�������~
    void NormalAttackBoolFalse()
    {//�ʏ�U�������~
        _isNormalAttack = false;
    }
    void DasshAttackBoolFalse() //�_�b�V���U�������~
    {
        //_rigidbody2D.constraints = RigidbodyConstraints2D.None;
        _isDashAttack = false;


    }
    void MashAttackBoolFalse()//�A���U�������~
    {
        _isMashAttack = false;
    }
    void AirAttackBoolFalse()//�A���U�������~
    {
        _isAirAttack = false;
        _isAttack = false;
    }

    //�U���������
    void AttackObjectDestroy()
    {
        _attackObjectCollider[x].enabled = false;
        _attackObjectSpriteRenderer[x].enabled = false;
    }
    #endregion

    private void AllAttack()
    {
        _isAttack = true;

        //SE
        _objectSE.SwordSE();
        //�U������̏o��
        _attackObjectCollider[x].enabled = true;
        _attackObjectSpriteRenderer[x].enabled = true;
        //�U������j��x��               
        Invoke("AttackObjectDestroy", _attckEreaTime);

    }

}
