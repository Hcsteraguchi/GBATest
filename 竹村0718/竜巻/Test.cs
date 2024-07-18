using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Test : MonoBehaviour
{
    [Header("+-*/")][SerializeField] private int skillNum; //スキル番号+-*/
    //[SerializeField] private int skillKindNum = 4; //スキルの種類数 
    [SerializeField] private Action[] _skill=new Action[5]; //スキルの関数を格納する配列
   
    private const float _speed = 0.15f;//竜巻の吸引スピード
    private const float _speedTyphoon = 0.04f;//竜巻の移動スピード
    private const float _typoonTime = 7f;// 竜巻持続時間
    private const float _coolTime = 10f;//竜巻持続後のクールタイム
    [Header("プレイヤーの外・竜巻本体")] [SerializeField] private GameObject _typhoon;
    [Header("プレイヤー配下・竜巻の初期地点")] [SerializeField] private GameObject _typoonStart;
    [Header("プレイヤー配下・竜巻の最終移動地点")] [SerializeField] private GameObject _typhoonTarget;
    [Header("プレイヤーの外・位置の固定に使うため表示されないオブジェクト")] [SerializeField]private GameObject _typoonStayPos;
  
    //[SerializeField] private List<GameObject> _enemys;
    //[SerializeField]private List<bool> _enemyTyhoon;
    private bool _isTypoon = default;
  
    public void SkillSet()
    {
        _skill[1] = S1TypoonSet;
    }

    private void Start()
    {
        //関数の配列
        SkillSet();
        //座標を入れておいてそこに台風を出現させる
        _typhoon.SetActive(false);
    }

    private void Update()
    {
        //スキル番号から関数を呼び出す
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("Test");
            _skill[skillNum]();
        }

    }
    private void FixedUpdate()
    {
        //竜巻の移動
        if (_isTypoon)
        {
            _typhoon.transform.position = Vector2.MoveTowards(_typhoon.transform.position, _typoonStayPos.transform.position, _speedTyphoon);
        }
    }
    private async void S1TypoonSet()
    {
        //非同期処理
        if(!_isTypoon)
        {
            _typhoon.SetActive(true);
            _typhoon.transform.position = _typoonStart.transform.position;
            _typoonStayPos.transform.position = _typhoonTarget.transform.position;
            print(_typoonStayPos);
            _isTypoon = true;
            //持続時間
            await UniTask.Delay(TimeSpan.FromSeconds(7f));
            _typhoon.SetActive(false);
            //持続時間終了後のクールタイム
            await UniTask.Delay(TimeSpan.FromSeconds(7f));
            _isTypoon = false;
        }
            
        
    }
    public void Typoon(GameObject enemy)
    { 
        //竜巻に巻き込まれた敵を竜巻の中心に移動させる
      enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, _typhoon.transform.position, _speed);

    }
   
}
