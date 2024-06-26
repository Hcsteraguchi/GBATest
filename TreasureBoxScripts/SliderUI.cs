using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliderUI : MonoBehaviour
{
    private Slider _slider = default;
    [Header("対応するスライダー")] [SerializeField] private GameObject _nowSlider = default;
    [Header("減少させる値")] [SerializeField] private int _downValue = default;
    // Start is called before the first frame update
    void Start()
    {
        _slider = _nowSlider.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //本来はプレイヤーのHPまたは武器エネルギーを参照させる
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            DownSlider();
        }
    }
    public void DownSlider()
    {
        // スライダーの中身を減少させる
        _slider.value -= _downValue;
    }
}
