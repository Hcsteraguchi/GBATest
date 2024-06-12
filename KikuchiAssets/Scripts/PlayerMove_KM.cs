using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour {
    //ctrl+F�Ō���
    #region  
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
    private bool _isLeft = false;//���A���������Ă��邩�i�ۂȂ�E�������Ă���j
    private bool _canPlayerMove = true;//�W�����v�Ɖ��ړ��̋���//���O����?



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
    [Header("�ʏ큨�A���ԁ@�g�p�Ԋu")] [SerializeField] private float _mashAttackInterval = 0.5f;//�ʏ큨�A���̐ڑ�
    [Header("�������E�U����̌㌄�i���̑��j�������ׂ��H")] [SerializeField] private float _attackDelay = 0.8f;//�v����!!  �������ׂ��H  �U����̌㌄ 
    [Header("�A���U����t����")] [SerializeField] float _mashTime = 1.0f;//�A���U����t����
    [Header("�n��ʏ�U����������")] [SerializeField] private float _normalTime = 1.0f;//�n��ʏ�U���@nAttackNow�������ԁ@
    [Header("�n��ʏ�U����̌㌄")] [SerializeField] private float _normalDelay = 1.0f;//�n��ʏ�U��  �U����̌㌄
    private float _normalAttackIntervalTimer = 0.0f;//�U�� �n���~ �Ďg�p���ԗp 
    private float _dashAttackIntervalTimer = 0.0f;//�U�� �n��ړ� �Ďg�p���ԗp 
    private float _airAttackIntervalTimer = 0.0f;//�U���@�󒆁@�@�Ďg�p���ԗp 
    private float _mashAttackIntervalTimer = 0.0f;//�U���@�A���@�@�Ďg�p���ԗp

    //�U������
    [Header("�ʏ�U������")] [SerializeField] private GameObject _normalAttackObject;
    [Header("�_�b�V���U������")] [SerializeField] private GameObject _dashAttackObject;
    [Header("�󒆍U������")] [SerializeField] private GameObject _airAttackObject;
    [Header("�A���U�����蔻��")] [SerializeField] private GameObject _mashAttackObject;
    [Header("�ŏI�A���U������")] [SerializeField] private GameObject _lastMashAttackObject;

    //�W�����v�p
    [Header("�W�����v�����x")] [SerializeField] private float _jumpSpeed = 16.0f;
    [Header("�d�͉����x")] [SerializeField] private float _gravityPower = 30.0f;
    [Header("�Œ�W�����v����")] [SerializeField] private float _jumpLowerTime = 0.05f;
    [Header("�������E���S��~�����̊")] [SerializeField] private float _stopMovePoint = 1f;//���S��~�����̊�@x�������ł�낵�H//���O����?
    [Header("�W�����v�\�ȃ��C���[")] [SerializeField] private LayerMask _groundLayer;
    private float _jumpTimer = 0f; // �W�����v�o�ߎ���
    private bool _isJumpKey = false; // �W�����v�L�[ 
    private bool _keyLook = false; // �W�����v�L�[������ 
    private bool _isGround = false;//���C�L���X�g�̐ݒu����
    private Status _playerStatus = Status.DOWN;//OnCollision�̐ݒu����
    enum Status//�v���C���[��� �㉺
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3,
    }

    //�_���[�W��
    [Header("�m�b�N�o�b�N�������")] [SerializeField] public float _knockBackPower = 1000;
    [Header("�_���[�W���̓_�ł̊Ԋu")] [SerializeField] float _flashInterval = 0.02f;
    [Header("�_�ł�����Ƃ��̃��[�v�J�E���g")] [SerializeField] int _loopCount = 60;
    private SpriteRenderer _mySpriteRenderer;//�_�ł����邽�߂�SpriteRenderer
    private STATE _damageState = STATE.NOMAL;//�v���C���[�̃_���[�W��� //���O����?
    private Damage _damageScript;//�_���[�W����p�X�N���v�g�ڑ� //���O����?
    private float _knockBackDirection;//�m�b�N�o�b�N���� //���O����?

    //�v���C���[�̏�ԃ_���[�W�p�i�m�[�}���A�_���[�W�A���G�j
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

    [Header("�U�����葶�ݎ���")] [SerializeField] float _attckEreaTime = 0.3f;

    void Start() {
        //Rigidbody2D���擾
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //�v���C���[���W�擾
        _transform = transform;
        _prevPosition = _transform.position;


        //SpriteRenderer�i�[
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
        //�ڒn���背�C�L���X�g
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
        // �W�����v�L�[���͎擾
        if (Input.GetButton("Jump") && _canPlayerMove /*&& state != STATE.DAMAGED*/) {
            _isJumpKey = !_keyLook;
        } else {
            _isJumpKey = false;
            _keyLook = false;
        }

        //���E���]
        Vector3 langle = this.transform.eulerAngles;

        if (langle.y == 0 && _isLeft == true) {
            langle.y = 180;
        } else if (langle.y == 180 && _isLeft == false) {
            langle.y = 0;
        }
        this.transform.eulerAngles = new Vector3(0, langle.y, 0);







        //�A���U���̘A�Ŕ���p
        if (Input.GetButtonDown("Attack") && _isMashAttack) {
            _mashCount++;//�����`����ς���
        }

        //�A���U���@mashAttack
        else if (Input.GetButtonDown("Attack") && _playerStatus == Status.GROUND && _horizontalPower == 0 && _isNormalAttack && _mashAttackIntervalTimer <= 0.0f && !_isDashAttack)//�n���~�U����
        {



            //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
            _isMashAttack = true;
            Invoke("MashAttackSecondNext", _mashTime);

            //GameObject childObject = Instantiate(_mashAttackObject, transform);
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;
            Invoke("MashAttackObjectDestroy", _attckEreaTime);//�U������j��x��

            Debug.Log("�A");

        }

        //�_�b�V���A�^�b�N�@dashAttack
        else if (Input.GetButtonDown("Attack") && _dashAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND && _horizontalPower != 0)//�U���n��ړ�
        {
            //�U�����̈ړ���~
            _canPlayerMove = false;
            Invoke("PlayerMoveTrue", _attackDelay);

            //�U�����ł��邱��
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
            //�Q�[���I�u�W�F�N�g��\�����\��
            _dashAttackObjectCollider.enabled = true;
            _dashAttackObjectSpriteRenderer.enabled = true;
            Invoke("DashAttackObjectDestroy", _attckEreaTime);//�U������j��x��



            _normalAttackIntervalTimer = _normalAttackInterval;//?���邩�A�_�b�V�����ʏ�̊�
            _dashAttackIntervalTimer = _dashAttackInterval;
        }


        //�ʏ�U�� normalAttack
        else if (Input.GetButtonDown("Attack") && _normalAttackIntervalTimer <= 0.0f && _playerStatus == Status.GROUND && _horizontalPower == 0 && !_isMashAttack)//�n���~�U����
        {
            //�U�����̈ړ���~
            _canPlayerMove = false;
            Invoke("NormalAttackPlayerMoveTrue", _normalDelay);

            //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
            _isNormalAttack = true;
            Invoke("NormalAttackBoolFalse", _normalTime);

            //GameObject childObject = Instantiate(_normalAttackObject, transform);
            //�Q�[���I�u�W�F�N�g��\�����\��
            //_ktext.SetActive(true);
            _normalAttackObjectCollider.enabled = true;
            _normalAttackObjectSpriteRenderer.enabled = true;
            Invoke("NormalAttackObjectDestroy", _attckEreaTime);//�U������j��x��



            Debug.Log("�ʏ�");
            _normalAttackIntervalTimer = _normalAttackInterval;
            _mashAttackIntervalTimer = _mashAttackInterval;


        }

        //�󒆍U�� airAttack
        else if (Input.GetButtonDown("Attack") && _airAttackIntervalTimer <= 0.0f && (_playerStatus == Status.UP || _playerStatus == Status.DOWN) && _canAirAttack == true)//�󒆍U����
        {
            _canAirAttack = false;
            //GameObject childObject = Instantiate(_airAttackObject, transform);
            _airAttackObjectCollider.enabled = true;
            _airAttackObjectSpriteRenderer.enabled = true;
            Invoke("AirAttackObjectDestroy", _attckEreaTime);//�U������j��x��
            Debug.Log("��");
            _normalAttackIntervalTimer = _normalAttackInterval;//?���邩�A�󒆁��ʏ�̊�
            _airAttackIntervalTimer = _airAttackInterval;

        }




    }

    private void FixedUpdate() {
        //jump
        Vector2 newvec = new Vector2(_rigidbody2D.velocity.x, 0);
        switch (_playerStatus) {
            // �ڒn��
            case Status.GROUND:
                if (_isJumpKey) {
                    _playerStatus = Status.UP;
                }
                break;

            // �㏸��
            case Status.UP:
                _jumpTimer += Time.deltaTime;

                if (_isJumpKey || _jumpLowerTime > _jumpTimer) {
                    newvec.y = _jumpSpeed;
                    newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));//jumpTimer^2
                } else {
                    _jumpTimer += Time.deltaTime; // �����𑁂߂�
                    newvec.y = _jumpSpeed;
                    newvec.y -= (_gravityPower * Mathf.Pow(_jumpTimer, 2));
                }

                if (0f > newvec.y) {
                    _playerStatus = Status.DOWN;
                    newvec.y = 0f;
                    _jumpTimer = 0.1f;
                }
                break;

            // ������
            case Status.DOWN:
                _jumpTimer += Time.deltaTime;

                newvec.y = 0f;
                newvec.y -= (_gravityPower * _jumpTimer);//�����K�v�@�����
                //newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                break;

            default:
                break;
        }
        _rigidbody2D.velocity = newvec;

        // �E�E��
        _horizontalPower = Input.GetAxisRaw("Horizontal");
        if (_canPlayerMove) {


            //�E �ړ�    
            if (Input.GetKey(KeyCode.D) || _horizontalPower > 0) {
                _isLeft = false;


                _rigidbody2D.AddForce(Vector2.right * ((_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));
                //this.rb.velocity = new Vector2(_speed, rb.velocity.y);//����




            }

            //��  �ړ�     
            if (Input.GetKey(KeyCode.A) || _horizontalPower < 0) {
                _isLeft = true;

                _rigidbody2D.AddForce(Vector2.right * ((-_sideSpeed - _rigidbody2D.velocity.x) * _sidePower));
                //this.rb.velocity = new Vector2(-_speed, rb.velocity.y);//����

            }
        }

        //��~�y�уW�����v�ϗp

        // ���݃t���[���̃��[���h�ʒu
        Vector2 position = _transform.position;
        // �ړ��ʂ��v�Z
        Vector2 delta = _prevPosition;
        // ����Update�Ŏg�����߂̑O�t���[���ʒu�X�V
        _prevPosition = position;

        float delX = position.x - delta.x;
        float delY = position.y - delta.y;

        //�v���C���[���x(X & Y)
        _speedX = Mathf.Abs(this._rigidbody2D.velocity.x);
        _speedY = Mathf.Abs(this._rigidbody2D.velocity.y);

        //���������Ɋւ��鏈��
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
        ////���S��~����
        if (_speedX + _speedY <= _stopMovePoint) {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }

    }
    void MashAttackBoolFalse()//�A���U�������~
    {
        _isMashAttack = false;
    }
    void MashAttackSecondNext()//�A���U���񌂖ڈȍ~
    {
        Debug.Log("�A2");
        if (_mashCount > _mashCountLimit && _mashAttackSecondNextCount < _mashAttackSecondNextCountLimit) {
            Invoke("MashAttackSecondNext", _mashTime);
            _mashCount = 0;
            _mashAttackSecondNextCount++;
            //GameObject childObject = Instantiate(_mashAttackObject, transform);
            _mashAttackObjectCollider.enabled = true;
            _mashAttackObjectSpriteRenderer.enabled = true;
            Invoke("MashAttackObjectDestroy", _attckEreaTime);//�U������j��x��

            Debug.Log("�A3");
        } else {
            _mashAttackSecondNextCount = 0;
            _mashCount = 0;
            Invoke("MashAttackBoolFalse", _mashTime);
            //GameObject childObject = Instantiate(_lastMashAttackObject, transform);
            _lastMashAttackObjectCollider.enabled = true;
            _lastMashAttackObjectSpriteRenderer.enabled = true;
            Invoke("LastMashAttackObjectDestroy", _attckEreaTime);//�U������j��x��
            Debug.Log("�A4");
            Invoke("PlayerMoveTrue", _attackDelay);
        }
    }
    //void AGR()//�U���n��A���p
    //{
    //    if (!_isNormalAttack || !_isMashAttack) {
    //        _canPlayerMove = true;//�_�b�V���U�����Ȃ�ړ��𔭓����Ȃ�����ׂ�
    //    }

    //}

    void NormalAttackPlayerMoveTrue()//�ʏ�U�����v���C���[�c�S�����@
    {
        if (!_isDashAttack && !_isMashAttack) {
            _canPlayerMove = true;//�_�b�V���U�����Ȃ�ړ��𔭓����Ȃ�����ׂ�
        }

    }
    void NormalAttackBoolFalse() {//�ʏ�U�������~
        _isNormalAttack = false;
    }

    void PlayerMoveTrue()//�U�����v���C���[�c�S����
    {
        _canPlayerMove = true;
    }
    void DasshAttackBoolFalse() //�_�b�V���U�������~
        {
        _isDashAttack = false;
    }



    void OnCollisionStay2D(Collision2D collision) {
        if (_isGround && _playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            _playerStatus = Status.GROUND;
            _jumpTimer = 0f;
            _keyLook = true; // �L�[��������b�N����
            _canAirAttack = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision) {
        if (_playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            _playerStatus = Status.DOWN;
            _canAirAttack = true;
        }
    }
    public void ShowLog() {
        // ���O��\�����܂��B
        _playerStatus = Status.DOWN;

        //�ύX �m�[�}������Ȃ��i�_���[�W���A���G���j�Ƃ��̓��^�[��
        if (_damageState != STATE.NOMAL) {
            return;
        }
        _canPlayerMove = false;
        Invoke("PlayerMoveTrue", _attackDelay);
        _rigidbody2D.velocity = Vector2.zero;

        // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
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
            if (i > 10)//�H�H�H
            {
                _damageState = STATE.MUTEKI;
                _mySpriteRenderer.color = Color.green;
            }
        }
        _damageState = STATE.NOMAL;
        _mySpriteRenderer.color = Color.white;
    }
    #region//�U���������
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

    #endregion//�U���������


}
