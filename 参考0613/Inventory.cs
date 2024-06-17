using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<SubWeapon.WeaponSelect> _itemBoxList =
       new List<SubWeapon.WeaponSelect>();
    [SerializeField] public int[] _inventory = { 0, 1, 0 };
  
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //List<SubWeapon.WeaponSelect>
    //List<SubWeapon.WeaponSelect> selects,int weaponNo
    //SubWeapon.WeaponSelect.aaaa
    public void InventBox(SubWeapon.WeaponSelect select)
    {
        print(select);
        print("ŒðŠ·‚µ‚È‚³‚¢");
    }

    //internal void InventBox(SubWeapon.WeaponSelect weaponSelect, int item)
    //{
    //    print(weaponSelect);
    //    throw new NotImplementedException();
    //}
}
