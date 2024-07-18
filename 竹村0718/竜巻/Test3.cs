using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    //竜巻の当たり判定を持つ本体にアタッチする
    Test2 _test2;
    Rigidbody2D _rd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _test2 = collision.GetComponent<Test2>();
            _rd = collision.GetComponent<Rigidbody2D>();
            _test2._hit = true;
            _rd.gravityScale = 0.1f;
            //竜巻内部の重力を弱める
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            _test2 = collision.GetComponent<Test2>();
            _test2._hit = false;
            _rd = collision.GetComponent<Rigidbody2D>();
            _rd.gravityScale = 1;
            //竜巻から出たら重力を戻す
        }
    }
}
