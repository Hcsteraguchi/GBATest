using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSub : MonoBehaviour
{
    //����ɂ���
    [Header("���̕���̍U����")] [SerializeField]public int _Damage;

    public int _getDamage
    {
        get { return _Damage; }
    }
}
