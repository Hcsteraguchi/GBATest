using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSub : MonoBehaviour
{
    //•Ší‚É‚Â‚¯‚é
    [Header("‚±‚Ì•Ší‚ÌUŒ‚—Í")] [SerializeField]public int _Damage;

    public int _getDamage
    {
        get { return _Damage; }
    }
}
