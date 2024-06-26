using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour {
    #region //�ϐ� 
    //Rigidbory2D
    private Rigidbody2D _rigidbody2D;

    //�����p
    private float _speedX;//X���̃v���C���[�̑��x�̐�Βl
    private float _speedY;//Y���̃v���C���[�̑��x�̐�Βl
    private float _horizontalPower;//�W���C�X�e�B�b�N���E���͂̐��l���i�E�����j
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
    private bool _isNormalAttack = default;//�U�����@�n���~
    private bool _isDashAttack = default;//�U�����@�n��ړ�
    private bool _isMashAttack = default;//�U�����@�n��A��
    private int _mashCount = 0;//�A�ŉ񐔃J�E���g 
    private int _mashAttackSecondNextCount = 0;//�A�Ń��[�V�����J�E���g 

    //�U���Ԋu����p
    [Header("�ʏ�U�� �Ďg�p����")] [SerializeField] private float _normalAttackInterval = 0.75f;
    [Header("�_�b�V���U�� �Ďg�p����")] [SerializeField] private float _dashAttackInterval = 0.5f;
    [Header("�󒆍U���@�@�Ďg�p����")] [SerializeField] private float _airAttackInterval = 0.75f;
    [Header("�ʏ큨�A���ԁ@�g�p�Ԋu")] [SerializeField] private float _mashAttackInterval = 0.5f;
    [Header("�������E�U����̌㌄�i���̑��j�������ׂ��H")] [SerializeField] private float _attackDelay = 0.8f;//�v����!!  �������ׂ��H  �U����̌㌄ 
    [Header("�A���U����t����")] [SerializeField] float _mashTime = 1.0f;
    [Header("�n��ʏ�U����������")] [SerializeField] private float _normalTime = 1.0f;
    [Header("�n��ʏ�U����̌㌄")] [SerializeField] private float _normalDelay = 1.0f;
    private float _normalAttackIntervalTimer = 0.0f;//�U�� �n���~ �Ďg�p���ԗp 
    private float _dashAttackIntervalTimer = 0.0f;//�U�� �n��ړ� �Ďg�p���ԗp 
    private float _airAttackIntervalTimer = 0.0f;//�U���@�󒆁@�@�Ďg�p���ԗp 
    private float _mashAttackIntervalTimer = 0.0f;//�U���@�A���@�@�Ďg�p���ԗp

    //�U������
    [Header("�U�����葶�ݎ���")] [SerializeField] float _attckEreaTime = 0.3f;
    [Header("�ʏ�U������")] [SerializeField] private GameObject _normalAttackObject;
    [Header("�_�b�V���U������")] [SerializeField] private GameObject _dashAttackObject;
    [Header("�󒆍U������")] [SerializeField] private GameObject _airAttackObject;
    [Header("�A���U�����蔻��")] [SerializeField] private GameObject _mashAttackObject;
    [Header("�ŏI�A���U������")] [SerializeField] private GameObject _lastMashAttackObject;

    //�U�������Collider��SpriteRenderer
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
    [SerializeField] private Status _playerStatus = Status.DOWN;//OnCollision�̐ݒu����
    private bool _isJump = false;//�W�����v��

    //�v���C���[��� �㉺
    enum Status {
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
    private Damage _damageScript;//�_���[�W����p�X�N���v�g�ڑ� //���O����?
    private float _knockBackDirection;//�m�b�N�o�b�N���� //���O����?

    //�v���C���[�̏�ԃ_���[�W�p�i�m�[�}���A�_���[�W�A���G�j
    enum STATE {
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
    enum Attack {
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

    void Start() {

        //Rigidbody2D���擾
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //�v���C���[���W�擾
        _transform = transform;
        _prevPosition = _transform.position;

        //SpriteRenderer�i�[
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        //Damage�X�N���v�g�擾
        _damageScript = gameObject.transform.Find("PlayerBody").GetComponent<Damage>();

        //SEScript�̎擾
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();

        //�A�j���[�^�[�̎擾
        _animator = GetComponent<Animator>();

        //�U�������Collider��SpriteRenderer�@�擾
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

        // ��������ǉ��I�I�I�I�I�I�I�I�I
        _subWeapon = gameObject.GetComponent<SubWeapon>();


    

}
    void Update() {
        if (!_isPlayeropenBox)
        {
            #region//�펞����
            //�U���Ԋu�v��
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

            //�ڒn���背�C�L���X�g
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
            if (Input.GetButtonDown("Attack") && _isMashAttack)
            {
                _mashCount++;
            }

            //�_�b�V���U���@dashAttack
            else if (Input.GetButtonDown("Attack") && _dashAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND
                && _horizontalPower != 0)
            {
                _isAttack = true;

                //SE�ƃA�j���[�V����
                _objectSE.SwordSE();
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

                //�U������̏o��
                _dashAttackObjectCollider.enabled = true;
                _dashAttackObjectSpriteRenderer.enabled = true;

                //�U������j��x��
                Invoke("DashAttackObjectDestroy", _attckEreaTime);

                //�C���^�[�o���J�n
                _normalAttackIntervalTimer = _normalAttackInterval;
                _dashAttackIntervalTimer = _dashAttackInterval;

                _myCollider.isTrigger = true;

            }

            //�󒆍U�� airAttack
            else if (Input.GetButtonDown("Attack") && _airAttackIntervalTimer <= 0.0f
                && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && _canAirAttack == true)//�󒆍U����
            {
                _isAttack = true;

                //SE�ƃA�j���[�V����
                _objectSE.SwordSE();
                _animator.Play("playerSkySlash");

                //�󒆍U���񐔐����p
                _canAirAttack = false;

                //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
                _isAirAttack = true;
                Invoke("AirAttackBoolFalse", _airTime);

                //�U������̏o�� 
                _airAttackObjectCollider.enabled = true;
                _airAttackObjectSpriteRenderer.enabled = true;

                //�U������j��x��
                Invoke("AirAttackObjectDestroy", _attckEreaTime);

                //�C���^�[�o���J�n
                _normalAttackIntervalTimer = _normalAttackInterval;//?���邩�A�󒆁��ʏ�̊�
                _airAttackIntervalTimer = _airAttackInterval;
            }

            //�ʏ�U�� normalAttack
            else if (Input.GetButtonDown("Attack") && _normalAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND
                && _horizontalPower == 0 && !_isMashAttack)//�n���~�U����
            {
                _isAttack = true;

                //SE�ƃA�j���[�V����
                _objectSE.SwordSE();
                _animator.Play("playerAttack");

                //�U�����̑����~
                _canPlayerMove = false;
                Invoke("NormalAttackPlayerMoveTrue", _normalDelay);

                //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
                _isNormalAttack = true;
                Invoke("NormalAttackBoolFalse", _normalTime);

                //�U������̏o��        
                _normalAttackObjectCollider.enabled = true;
                _normalAttackObjectSpriteRenderer.enabled = true;

                //�U������j��x��
                Invoke("NormalAttackObjectDestroy", _attckEreaTime);

                //�C���^�[�o���J�n
                _normalAttackIntervalTimer = _normalAttackInterval;
                _mashAttackIntervalTimer = _mashAttackInterval;

            }

            //�A���U���@mashAttack
            else if (Input.GetButtonDown("Attack") && _playerStatus == Status.GROUND && _horizontalPower == 0
                && _isNormalAttack && _mashAttackIntervalTimer <= 0.0f && !_isDashAttack)//�n���~�U����
            {
                _isAttack = true;

                //SE�ƃA�j���[�V����
                _objectSE.SwordSE();
                _animator.Play("playerAttack");

                //�U��������
                _isMashAttack = true;
                Invoke("MashAttackSecondNext", _mashTime);

                //�U������̏o��
                _mashAttackObjectCollider.enabled = true;
                _mashAttackObjectSpriteRenderer.enabled = true;

                //�U������j��x��
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

            ////���S��~����
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
                    if (!_isJump)
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
                    if (!_isJump)
                    {
                        _animator.Play("playerWalk");
                    }
                }

            }
        }
            #endregion
    }

    //�_���[�W���̍s��
    public void DamageSet() {
        Debug.Log("EA4");
        //�ύX �m�[�}������Ȃ��i�_���[�W���A���G���j�Ƃ��̓��^�[��
        if (_damageState != STATE.NOMAL || _isDashAttack) {
            return;
        }
        //if (_damageState != STATE.MUTEKI) {

        //}
        Debug.Log("EA5");
        StartCoroutine(DamageKnockBack());

    }

    IEnumerator DamageKnockBack() {//�v�����H
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
            //�m�b�N�o�b�N�@�ϐ��ɂ���
            _rigidbody2D.AddForce(new Vector2(1.5f * _knockBackDirection, 7) * _knockBackPower, 0);//?
            _objectSE.JumpSE();
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);
        } else {
            Debug.Log("EA6");
            //�m�b�N�o�b�N�@�ϐ��ɂ���
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

                if (i > _mutekiCount)//�H�H�H
                {
                    _damageState = STATE.MUTEKI;

                }

            }

            _damageState = STATE.NOMAL;
        }

        

    }
    #region//��������

    //�A���U���񌂖ڈȍ~
    void MashAttackSecondNext() {

        if (_mashCount > _mashCountLimit && _mashAttackSecondNextCount < _mashAttackSecondNextCountLimit) {
            //SE�ƃA�j���[�V����
            _objectSE.SwordSE();
            _animator.Play("playerAttack");

            //�A���U���񌂖ڈȍ~�x���Ăяo��
            Invoke("MashAttackSecondNext", _mashTime);

            //�A���U���̘A�œ��͉񐔂̃��Z�b�g
            _mashCount = 0;

            //�A���U���̉񐔃J�E���g����
            _mashAttackSecondNextCount++;

            //�U������̏o�� 
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;

            //�U������j��x��
            Invoke("MashAttackObjectDestroy", _attckEreaTime);

        } else {
            //SE�ƃA�j���[�V����
            _objectSE.SwordSE();
            _animator.Play("playerAttack");

            //�A���U���̘A�œ��͉񐔂̃��Z�b�g
            _mashCount = 0;

            //�A���U���̉񐔂̃��Z�b�g
            _mashAttackSecondNextCount = 0;

            //�U�����ł��邱�Ƃ̒�~�Ƃ��̒x��
            Invoke("MashAttackBoolFalse", _mashTime);

            //�U������̏o�� 
            _lastMashAttackObjectCollider.enabled = true;
            _lastMashAttackObjectSpriteRenderer.enabled = true;

            //�U������j��x��
            Invoke("LastMashAttackObjectDestroy", _attckEreaTime);
            Invoke("PlayerMoveTrue", _attackDelay);

            //�C���^�[�o���J�n
            _normalAttackIntervalTimer = _normalAttackInterval;
        }
    }

    //�n�ʂ̔F��
    void OnCollisionStay2D(Collision2D collision) {
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
        }
    }
    //�n�ʂ��痣�ꂽ�ꍇ�̏���
    void OnCollisionExit2D(Collision2D collision) {
        //�W�����v�����ɒn�ʂ��痣�ꂽ�ꍇ
        if (_playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            //�ڒn����������
            _playerStatus = Status.DOWN;

            //�󒆍U���񐔉�
            _canAirAttack = true;
        }
    }

    //�U�����v���C���[���쐧������
    void PlayerMoveTrue() {
        _canPlayerMove = true;
        _isAttack = false;
    }
    void NormalAttackPlayerMoveTrue()//�ʏ�U�����v���C���[���쐧�������@
    {
        if (!_isDashAttack && !_isMashAttack) {//�_�b�V���U���A�A���U���ɔh�������ꍇ�͉������Ȃ�
            _canPlayerMove = true;
            _isAttack = false;
        }
    }

    //�U�������~
    void NormalAttackBoolFalse() {//�ʏ�U�������~
        _isNormalAttack = false;
    }
    void DasshAttackBoolFalse() //�_�b�V���U�������~
        {
        _isDashAttack = false;
    }
    void MashAttackBoolFalse()//�A���U�������~
    {
        _isMashAttack = false;
    }
    void AirAttackBoolFalse()//�A���U�������~
    {
        _isAirAttack = false;
    }

    //�U���������
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
