using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("攻撃した敵or敵攻撃判定・何も入れるな")] public Collider2D　_attackEnemy; //公開必要
    [Header("Playerオブジェクトを格納")] public GameObject _playerObj; //playerオブジェクトを格納、公開必要
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyAttack")//EnemyAttackとの接触
        {
            _attackEnemy = col;//敵攻撃判定取得
            _playerObj.GetComponent<PlayerMove_KM>().ShowLog();//playerMove_KMのダメージ時の行動の起動        
        }
        if (col.gameObject.tag == "Enemy")//Enemyとの接触
        {    
            _attackEnemy = col;//攻撃した敵取得
            _playerObj.GetComponent<PlayerMove_KM>().ShowLog();//playerMove_KMのダメージ時の行動の起動 
        }
    }
    


}
