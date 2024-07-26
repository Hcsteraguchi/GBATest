using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class EnemyStatus : MonoBehaviour
{
    Transform playerTr; // �v���C���[��Transform
    Rigidbody2D _rigidbody;
    private float airStayTime = -1;�@//�؋�\����
    private GameObject _player;
    private int direction = 0;
    private bool _LaunchCool = true;
    private float _LaunchCoolStayTime;
    private bool _dashCool = true;
    private float _dashCoolStayTime;
    public bool _Damage;
    public bool _enemyStatusAllStop = false;

    [Header("�K�E�Z�Q�[�W������")] [SerializeField] private AddNumUI _addNumUI;
    [Header("StateCanva�A�^�b�`")] [SerializeField] private GameObject _stateCanvas;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _addNumUI = _stateCanvas.GetComponent<AddNumUI>();
    }

    void Update()
    {

        if (_rigidbody.drag != 0)
        {
            airStayTime -= Time.deltaTime; //Drag�̒l��0�łȂ����airStayTime�̒l�����炵�Ă���
        }

        if (airStayTime < 0)
        {
            OnGrvity();  //����
            airStayTime = 0.7f; //airStayTime�̒l�����Z�b�g
        }
        if (_LaunchCool)
        {
            _LaunchCoolStayTime -= Time.deltaTime; //Drag�̒l��0�łȂ����airStayTime�̒l�����炵�Ă���
        }

        if (_LaunchCoolStayTime < 0)
        {

            _LaunchCoolStayTime = 0.4f; //airStayTime�̒l�����Z�b�g
            _LaunchCool = false;
        }
        if (_dashCool)
        {
            _dashCoolStayTime -= Time.deltaTime; //Drag�̒l��0�łȂ����airStayTime�̒l�����炵�Ă���
        }

        if (_dashCoolStayTime < 0)
        {

            _dashCoolStayTime = 0.4f; //airStayTime�̒l�����Z�b�g
            _dashCool = false;
        }

    }
    public void OnGrvity()
    {
        if (_rigidbody.drag != 0)
        {
            _rigidbody.drag = 0; //Drag�̒l��0�ɖ߂��ė���
        }
    }
    public void OffGrvity()
    {
        _rigidbody.drag = 1000;�@//RigidBody��Drag�̐��l��M��
    }

    public void Launch()
    {
        _addNumUI.AddNums();
        if (!_LaunchCool && !_enemyStatusAllStop)
        {
            _rigidbody.DOMoveY(transform.position.y + 9f, 0.3f);�@//�U�����󂯂��G��ł��グ�܂��B
            _LaunchCool = true;
        }

    }
    public void DownFall()
    {
        _addNumUI.AddNums();
        if (!_enemyStatusAllStop)
        {
            _rigidbody.DOMoveY(transform.position.y - 9f, 0.3f);�@//�U�����󂯂��G��ł��グ�܂��B
        }

    }
    public void airStayExtend()
    {
        _addNumUI.AddNums();
        airStayTime = 0.7f;�@//�؋󎞊Ԃ�����
    }
    public async void Dash()
    {
        _addNumUI.AddNums();
        if (!_dashCool && !_enemyStatusAllStop)
        {
            playerTr = _player.transform;
            if (playerTr.position.x < transform.position.x)
            {
                direction = 1;
            }
            else if (playerTr.position.x > transform.position.x)
            {
                direction = -1;
            }
            await Task.Delay(50);
            _rigidbody.DOMoveX(transform.position.x + 3f * direction, 0.2f);
            _dashCool = true;
        }

    }
    public void Combo()
    {
        //�U�������������ɗl�X�Ȑ��l�����Z������
         
            //UI�p�̑S�ϐ����Q��
            _addNumUI.AddNums();
        
    }

}
