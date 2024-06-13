using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    //[SerializeField] GameObject playerObj;
    Damage Damage;
   [Header("“G‚ÌHP‚ð“ü‚ê‚é")] [SerializeField]int _enemyHp = 500;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damage = collision.gameObject.GetComponent<Damage>();
        int damage = Damage._Damage;
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            _enemyHp = _enemyHp - damage;
            print(_enemyHp);
            if(_enemyHp<=0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
