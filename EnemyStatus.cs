using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class EnemyStatus : MonoBehaviour
{
    Transform playerTr; // プレイヤーのTransform
    Rigidbody2D _rigidbody;
    private float airStayTime = -1;　//滞空可能時間
    private GameObject _player;
    private int direction = 0;
    private bool _LaunchCool = true;
    private float _LaunchCoolStayTime;
    private bool _dashCool = true;
    private float _dashCoolStayTime;
    public bool _Damage;
    public bool _enemyStatusAllStop = false;

    [Header("必殺技ゲージを入れる")] [SerializeField] private AddNumUI _addNumUI;
    [Header("StateCanvaアタッチ")] [SerializeField] private GameObject _stateCanvas;
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
            airStayTime -= Time.deltaTime; //Dragの値が0でなければairStayTimeの値を減らしていく
        }

        if (airStayTime < 0)
        {
            OnGrvity();  //落下
            airStayTime = 0.7f; //airStayTimeの値をリセット
        }
        if (_LaunchCool)
        {
            _LaunchCoolStayTime -= Time.deltaTime; //Dragの値が0でなければairStayTimeの値を減らしていく
        }

        if (_LaunchCoolStayTime < 0)
        {

            _LaunchCoolStayTime = 0.4f; //airStayTimeの値をリセット
            _LaunchCool = false;
        }
        if (_dashCool)
        {
            _dashCoolStayTime -= Time.deltaTime; //Dragの値が0でなければairStayTimeの値を減らしていく
        }

        if (_dashCoolStayTime < 0)
        {

            _dashCoolStayTime = 0.4f; //airStayTimeの値をリセット
            _dashCool = false;
        }

    }
    public void OnGrvity()
    {
        if (_rigidbody.drag != 0)
        {
            _rigidbody.drag = 0; //Dragの値を0に戻して落下
        }
    }
    public void OffGrvity()
    {
        _rigidbody.drag = 1000;　//RigidBodyのDragの数値を弄る
    }

    public void Launch()
    {
        _addNumUI.AddNums();
        if (!_LaunchCool && !_enemyStatusAllStop)
        {
            _rigidbody.DOMoveY(transform.position.y + 9f, 0.3f);　//攻撃を受けた敵を打ち上げます。
            _LaunchCool = true;
        }

    }
    public void DownFall()
    {
        _addNumUI.AddNums();
        if (!_enemyStatusAllStop)
        {
            _rigidbody.DOMoveY(transform.position.y - 9f, 0.3f);　//攻撃を受けた敵を打ち上げます。
        }

    }
    public void airStayExtend()
    {
        _addNumUI.AddNums();
        airStayTime = 0.7f;　//滞空時間を延長
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
        //攻撃が入った時に様々な数値を加算させる
         
            //UI用の全変数を参照
            _addNumUI.AddNums();
        
    }

}
