using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSManager : MonoBehaviour {

    // リトライする前のコイン
    public static int Prevcoin;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "GameUI")
        {
            SceneManager.LoadSceneAsync("Scene/GameUI", LoadSceneMode.Additive);
        }
        //BGM再生。AUDIO.BGM_BATTLEがBGMのファイル名
        AudioManager.Instance.PlayBGM(AUDIO.BGM_FORESTBATTLE, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
    }

    void Start()
    {
        Prevcoin = GameManager.Instance.coin;
        TouchEventHandler.Instance.onPress += OnPress;
    }
    private void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            Debug.Log("タップされた！");
        }
        else
        {
            Debug.Log("タップ終わった！");
        }
    }
}
