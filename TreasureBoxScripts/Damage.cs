using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    [Header("攻撃した敵or敵攻撃判定・何も入れるな")] public Collider2D _attackEnemy; //公開必要
    [Header("Playerオブジェクトを格納")] public GameObject _playerObj; //playerオブジェクトを格納、公開必要

    //ダメージ用接触判定
    private void OnTriggerEnter2D(Collider2D col) {
        //EnemyAttackとの接触
        if (col.gameObject.tag == "EnemyAttack") {

            //敵攻撃判定取得
            _attackEnemy = col;

            //playerMove_KMのダメージ時の行動の起動 
            _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
        }
        //Enemyとの接触
        if (col.gameObject.tag == "Enemy") {

            //攻撃した敵取得
            _attackEnemy = col;

            //playerMove_KMのダメージ時の行動の起動 
            _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
        }
    }
    //private void OnTriggerStay2D(Collider2D col) {
    //    //EnemyAttackとの接触
    //    if (col.gameObject.tag == "EnemyAttack") {

    //        //敵攻撃判定取得
    //        _attackEnemy = col;

    //        //playerMove_KMのダメージ時の行動の起動 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }
    //    //Enemyとの接触
    //    if (col.gameObject.tag == "Enemy") {

    //        //攻撃した敵取得
    //        _attackEnemy = col;

    //        //playerMove_KMのダメージ時の行動の起動 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }

    //}
    //private void OnTriggerExit2D(Collider2D col) {
    //    //EnemyAttackとの接触
    //    if (col.gameObject.tag == "EnemyAttack") {

    //        //敵攻撃判定取得
    //        _attackEnemy = col;

    //        //playerMove_KMのダメージ時の行動の起動 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }
    //    //Enemyとの接触
    //    if (col.gameObject.tag == "Enemy") {

    //        //攻撃した敵取得
    //        _attackEnemy = col;

    //        //playerMove_KMのダメージ時の行動の起動 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }

    //}


}
