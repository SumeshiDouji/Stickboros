using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSManager : MonoBehaviour {
    [SerializeField]
    int StageNumber;
    string BGMName;
    // リトライする前のコイン
    public static int Prevcoin;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "GameUI")
        {
            SceneManager.LoadSceneAsync("Scene/GameUI", LoadSceneMode.Additive);
        }
        switch (StageNumber)
        {
            case 0:
                BGMName = AUDIO.BGM_GRSTAGEBATTLE;
                break;
            case 2:
                BGMName = AUDIO.BGM_MOUNTAINSTAGE;
                break;
        }
        //BGM再生。AUDIO.BGM_BATTLEがBGMのファイル名
        AudioManager.Instance.PlayBGM(BGMName, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
    }

    void Start()
    {
        Prevcoin = GameManager.Instance.coin;
    }
}
