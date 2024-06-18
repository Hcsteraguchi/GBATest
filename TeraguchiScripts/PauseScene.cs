using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScene : MonoBehaviour
{
    [SerializeField]private GameObject _pauseCanvas = default;
    public bool _isPause = default;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectPause()
    {
        if (!_isPause) //ポーズ画面ではないとき
        {
            PauseGame();
        }
        else //ポーズ画面のとき
        {
            RestartGame();
        }
    }
    private void PauseGame()
    {
        _pauseCanvas.SetActive(true); //ポーズ画面に移行
        Time.timeScale = 0f;
        _isPause = true;
    }
    private void RestartGame()
    {
        _pauseCanvas.SetActive(false);　//ポーズ画面解除
        Time.timeScale = 1f;
        _isPause = false;
    }
}
