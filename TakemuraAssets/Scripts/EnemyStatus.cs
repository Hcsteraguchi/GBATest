using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //[SerializeField] GameObject playerObj;
    Damage Damage;
   [Header("敵のHPを入れる")] [SerializeField]int _enemyHp = 500;
    private void OnTriggerEnter2D(Collider2D collision)
    {
      //プレイヤーの攻撃が命中した際にその武器の攻撃力を参照してHPを減らす
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            Damage = collision.gameObject.GetComponent<Damage>();
            int damage = Damage._Damage;
            _enemyHp = _enemyHp - damage;
            print(_enemyHp);

            //Hpが0を下回ったら処理をするデストロイはテスト
            if(_enemyHp<=0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
