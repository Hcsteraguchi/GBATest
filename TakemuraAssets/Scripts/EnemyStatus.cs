using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    SubWeapon subWeapon;
    [SerializeField]int _enemyHp = 500;
    void Start()
    {
        subWeapon=playerObj.gameObject.GetComponent<SubWeapon>();
    }

   
    void Update()
    {
        
    }
    private void DamageCtrl(Collider2D weapon)
    {
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit");
        DamageCtrl(collision);
        int damage = subWeapon._damage;
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
