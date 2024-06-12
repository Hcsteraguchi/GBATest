using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    //インベントリの配列[0]＝１はソードを格納
    private int[] _inventory = { 1, 0, 0 };
    [SerializeField] int item = default;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WeaponGet(Collision2D weapon)
    {
      
        if(_inventory[1]==0)
        if(Input.GetKeyDown(KeyCode.F))
        {

        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Item"))
        {
            WeaponGet(collision);
        }
    }
}
