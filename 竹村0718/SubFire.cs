using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _player;
    SubWeapon _subWeapon;
    private void Start()
    {
        _player = transform.parent.gameObject;
       _subWeapon= _player.GetComponent<SubWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            Debug.Log("è’ìÀëäéËÇÃñºëOÇÕÅA" + collision.gameObject.name);
            _subWeapon._fireTreget = collision.gameObject.transform.position;
            _subWeapon._isfire=true;
        }
    }
}
