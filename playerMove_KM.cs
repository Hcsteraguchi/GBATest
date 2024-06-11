using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove_KM : MonoBehaviour
{
    //ctrl+F�Ō���


    [Header("�ڒn����")] [SerializeField] Status playerStatus = Status.DOWN;

    //Rigidbory2D
    private Rigidbody2D rb;

    //�����p
    private float speedx;//X���̃v���C���[�̑��x�̐�Βl
    private float speedy;//Y���̃v���C���[�̑��x�̐�Βl
    private float horizontalPower;//�W���C�X�e�B�b�N���E���͂̐��l���i�E�����j
    private Transform _transform;//transform���擾
    private Vector3 _prevPosition;//�O�t���[���̈ʒu�擾

    //���ړ��p    
    [Header("���ړ����x")] [SerializeField] float _speed = 3.0f;
    [Header("���ړ������x")] [SerializeField] float power = 20.0f;
    private bool leftNow = false;//���A���������Ă��邩�i�ۂȂ�E�������Ă���j
    private bool playerMove = true;//�W�����v�Ɖ��ړ��̋���



    // �U���p    
    [Header("�_�b�V���U�����x")] [SerializeField] float dashForce = 400f;
    [Header("�A���U�����s�A�ŉ�")] [SerializeField] float rCountLimit = 2.0f;
    [Header("�A�Ń��[�V������")] [SerializeField] int AGRSCountLimit = 4;
    private bool airAttackCount = true;//�󒆍U���\
    private bool nAttackNow = false;//�U�����@�n���~
    private bool dAttackNow = false;//�U�����@�n��ړ�
    private bool rAttackNow = false;//�U�����@�n��A��
    private float rCount = 0.0f;//�A�ŉ񐔃J�E���g
    private int AGRSCount = 0;//�A�Ń��[�V�����J�E���g

    //�U���Ԋu����p
    [Header("�U�� �n���~ �Ďg�p����")] [SerializeField] float interval1 = 0.75f;
    [Header("�U�� �n��ړ� �Ďg�p����")] [SerializeField] float interval2 = 0.5f;
    [Header("�U���@�󒆁@�@�Ďg�p����")] [SerializeField] float interval3 = 0.5f;
    [Header("������")] [SerializeField] float interval4 = 0.5f;
    [Header("������")] [SerializeField] float attackDelay = 0.2f;//�v����!!  �������ׂ��H  �U����̌㌄
    [Header("������")] [SerializeField] float attackTime = 1.0f;//�A���U����t����
    [Header("������")] [SerializeField] float attackTime1 = 2.0f;//�n��ʏ�U���@nAttackNow�������ԁ@
    [Header("������")] [SerializeField] float attackDelay1 = 0.2f;//�n��ʏ�U��  �U����̌㌄
    private float timer1 = 0.0f;//�U�� �n���~ �Ďg�p���ԗp
    private float timer2 = 0.0f;//�U�� �n��ړ� �Ďg�p���ԗp
    private float timer3 = 0.0f;//�U���@�󒆁@�@�Ďg�p���ԗp
    private float timer4 = 0.0f;//�U���@�A���@�@�Ďg�p���ԗp�@������

    //�U������
    [Header("�ʏ�U������")] [SerializeField] GameObject normalObject;
    [Header("�󒆍U������")] [SerializeField] GameObject airObject;
    [Header("�_�b�V���U������")] [SerializeField] GameObject dashObject;
    [Header("�A���U�����蔻��")] [SerializeField] GameObject mashObject;
    [Header("�ŏI�A���U������")] [SerializeField] GameObject lastObject;

    //�W�����v�p
    [Header("�W�����v�����x")] [SerializeField] float jumpSpeed = 16.0f;
    [Header("�d�͉����x")] [SerializeField] float gravity = 30.0f;
    [Header("�Œ�W�����v����")] [SerializeField] float jumpLowerLimit = 0.1f;
    [Header("������")] [SerializeField] float stopMovePoint = 0.3f;//���S��~�����̊�@x�������ł�낵�H
    private float jumpTimer = 0f; // �W�����v�o�ߎ���
    private bool jumpKey = false; // �W�����v�L�[
    private bool keyLook = false; // �W�����v�L�[���͕s�\

    enum Status//�v���C���[��� �㉺
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3,      
    }
    //�ړ��X�s�[�h�Ɠ_�ł̊Ԋu
    [SerializeField] float speed, flashInterval;
    //�_�ł�����Ƃ��̃��[�v�J�E���g
    [SerializeField] int loopCount;
    //�_�ł����邽�߂�SpriteRenderer
    SpriteRenderer sp;
    private bool isGround = false;


    //�v���C���[�̏�ԗp�񋓌^�i�m�[�}���A�_���[�W�A���G��3��ށj
    enum STATE
    {
        NOMAL,
        DAMAGED,
        MUTEKI
    }
    //STATE�^�̕ϐ�
    STATE state = STATE.NOMAL;

    [SerializeField] private LayerMask GroundLayer;
    public float knockBackPower;   // �m�b�N�o�b�N�������
    private Damage PL;
    float puramai;
    void Start()
    {
        //Rigidbody2D���擾
        rb = GetComponent<Rigidbody2D>();
        //�v���C���[���W�擾
        _transform = transform;
        _prevPosition = _transform.position;


        //SpriteRenderer�i�[
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
        //�ڒn���背�C�L���X�g
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
        // �W�����v�L�[���͎擾
        if (Input.GetButton("Jump") && playerMove /*&& state != STATE.DAMAGED*/)
        {
            jumpKey = !keyLook;
        }
        else
        {
            jumpKey = false;
            keyLook = false;
        }

        //���E���]
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







        //�A���U���̘A�Ŕ���p
        if (Input.GetButtonDown("Attack") && rAttackNow)
        {
            rCount++;//�����`����ς���
        }

        //�U���n��A���@AGR
        else if (Input.GetButtonDown("Attack") && playerStatus == Status.GROUND && horizontalPower == 0 && nAttackNow && timer4 <= 0.0f && !dAttackNow)//�n���~�U����
        {



            //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
            rAttackNow = true;
            Invoke("AGRS", attackTime);

            GameObject childObject = Instantiate(mashObject, transform);

            Debug.Log("�A");

        }

        //�U���n��ړ��@AGM
        else if (Input.GetButtonDown("Attack") && timer2 <= 0.0f && playerStatus == Status.GROUND && horizontalPower != 0)//�U���n��ړ�
        {
            //�U�����̈ړ���~
            playerMove = false;
            Invoke("AGM", attackDelay);

            //�U�����ł��邱��
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
            timer1 = interval1;//?���邩�A�_�b�V�����ʏ�̊�
            timer2 = interval2;
        }


        //�U���n���~ AGS
        else if (Input.GetButtonDown("Attack") && timer1 <= 0.0f && playerStatus == Status.GROUND && horizontalPower == 0 && !rAttackNow)//�n���~�U����
        {
            //�U�����̈ړ���~
            playerMove = false;
            Invoke("AGS", attackDelay1);

            //�U�����ł��邱�Ɓ@����������ɘA���U���֔h���H
            nAttackNow = true;
            Invoke("AGSS", attackTime1);

            GameObject childObject = Instantiate(normalObject, transform);

            Debug.Log("�ʏ�");
            timer1 = interval1;
            timer4 = interval4;

        }

        //�󒆍U��
        else if (Input.GetButtonDown("Attack") && timer3 <= 0.0f && (playerStatus == Status.UP || playerStatus == Status.DOWN) && airAttackCount == true)//�󒆍U����
        {
            airAttackCount = false;
            GameObject childObject = Instantiate(airObject, transform);
            Debug.Log("��");
            timer1 = interval1;//?���邩�A�󒆁��ʏ�̊�
            timer3 = interval3;

        }




    }

    private void FixedUpdate()
    {
        //jump
        Vector2 newvec = new Vector2(rb.velocity.x, 0);
        switch (playerStatus)
        {
            // �ڒn��
            case Status.GROUND:
                if (jumpKey)
                {
                    playerStatus = Status.UP;
                }
                break;

            // �㏸��
            case Status.UP:
                jumpTimer += Time.deltaTime;

                if (jumpKey || jumpLowerLimit > jumpTimer)
                {
                    newvec.y = jumpSpeed;
                    newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));//jumpTimer^2
                }

                else
                {
                    jumpTimer += Time.deltaTime; // �����𑁂߂�
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

            // ������
            case Status.DOWN:
                jumpTimer += Time.deltaTime;

                newvec.y = 0f;
                newvec.y -= (gravity * jumpTimer);//�����K�v�@�����
                //newvec.y -= (gravity * Mathf.Pow(jumpTimer, 2));
                break;            

            default:
                break;
        }
        rb.velocity = newvec;

        // �E�E��
        horizontalPower = Input.GetAxisRaw("Horizontal");
        if (playerMove)
        {


            //�E �ړ�    
            if (Input.GetKey(KeyCode.D) || horizontalPower > 0)
            {
                leftNow = false;


                rb.AddForce(Vector2.right * ((_speed - rb.velocity.x) * power));
                //this.rb.velocity = new Vector2(_speed, rb.velocity.y);//����




            }

            //��  �ړ�     
            if (Input.GetKey(KeyCode.A) || horizontalPower < 0)
            {
                leftNow = true;

                rb.AddForce(Vector2.right * ((-_speed - rb.velocity.x) * power));
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
        speedx = Mathf.Abs(this.rb.velocity.x);
        speedy = Mathf.Abs(this.rb.velocity.y);

        //���������Ɋւ��鏈��
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
        ////���S��~����
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
        Debug.Log("�A2");
        if (rCount > rCountLimit && AGRSCount < AGRSCountLimit)
        {
            Invoke("AGRS", attackTime);
            rCount = 0;
            AGRSCount++;
            GameObject childObject = Instantiate(mashObject, transform);

            Debug.Log("�A3");
        }
        else
        {
            AGRSCount = 0;
            rCount = 0;
            Invoke("AGRSS", attackTime);
            GameObject childObject = Instantiate(lastObject, transform);
            Debug.Log("�A4");
            Invoke("AGM", attackDelay);
        }
    }
    void AGR()//�U���n��A���p
    {
        if (!nAttackNow || !rAttackNow)
            playerMove = true;//�_�b�V���U�����Ȃ�ړ��𔭓����Ȃ�����ׂ�
    }

    void AGS()//�U���n���~�p
    {
        if (!dAttackNow && !rAttackNow)
            playerMove = true;//�_�b�V���U�����Ȃ�ړ��𔭓����Ȃ�����ׂ�
    }
    void AGSS()
    {
        nAttackNow = false;
    }

    void AGM()//�U���n��ړ��p
    {
        playerMove = true;
    }
    void AGMS()
    {
        dAttackNow = false;
    }
    


    void OnCollisionStay2D(Collision2D collision)
    {
        if (isGround && playerStatus == Status.DOWN && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            playerStatus = Status.GROUND;
            jumpTimer = 0f;
            keyLook = true; // �L�[��������b�N����
            airAttackCount = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (playerStatus == Status.GROUND && collision.gameObject.name.Contains("Ground"))//�n�ʔ���ύX��
        {
            playerStatus = Status.DOWN;
            airAttackCount = true;
        }
    }
    public void ShowLog()
    {
        // ���O��\�����܂��B
        playerStatus = Status.DOWN;

        //�ύX �m�[�}������Ȃ��i�_���[�W���A���G���j�Ƃ��̓��^�[��
        if (state != STATE.NOMAL)
        {
            return;
        }
        playerMove = false;
        Invoke("AGM", attackDelay);       
        rb.velocity = Vector2.zero;

        // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
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
            if (i > 10)//�H�H�H
            {
                state = STATE.MUTEKI;
                sp.color = Color.green;
            }
        }
        state = STATE.NOMAL;
        sp.color = Color.white;
    }


}
