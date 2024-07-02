using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMain : MonoBehaviour
{
    SubWeapon SubWeapon;
    //•Ší‚É‚Â‚¯‚é
    [Header("‚±‚Ì•Ší‚ÌUŒ‚—Í")] [SerializeField] public int _Damage;
    [SerializeField] private SliderMP _sliderMP;
    [SerializeField] private GameObject _sliderObject = default;
    private void Start()
    {
        SubWeapon = gameObject.GetComponentInParent<SubWeapon>();
        _sliderMP = _sliderObject.GetComponent<SliderMP>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ƒvƒŒƒCƒ„[‚ÌUŒ‚‚ª–½’†‚µ‚½Û‚É‚»‚Ì•Ší‚ÌUŒ‚—Í‚ğQÆ‚µ‚ÄHP‚ğŒ¸‚ç‚·
        if (collision.gameObject.CompareTag("Enemy") && (SubWeapon.MP + _Damage) <= 100)
        {
            SubWeapon.MP += _Damage;
            _sliderMP.MPSlider();
        }
        if (collision.gameObject.CompareTag("Enemy") && (SubWeapon.MP + _Damage) > 100 && SubWeapon.MP <= 99)
        {
            SubWeapon.MP = 100;
            _sliderMP.MPSlider();
        }
    }
}
