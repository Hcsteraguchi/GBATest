using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    //敵の共通処理にマージする
    [SerializeField] private bool _istypoon = true;
    public bool _hit = default;
    //プレイヤー
    private GameObject _player;
    Test _test;
    private void Start()
    {
        //プレイヤーを探す
        _player = GameObject.Find("Player");
        _test = _player.GetComponent<Test>();
    }
    private void FixedUpdate()
    {
        //竜巻の効果を喰らう
        if (_istypoon&&_hit)
        {
            _test.Typoon(this.gameObject);
        }
    }
}
