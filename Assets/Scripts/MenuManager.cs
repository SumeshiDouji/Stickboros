using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void TapToStart()
    {
        if (UnityEngine.Rendering.SplashScreen.isFinished)
        {
            GameManager.Instance.ChangeScene("Scene/StageSelect");
        }
    }
    public void TapToTitle()
    {
        // BGMを停める
        AudioManager.Instance.StopBGM();
        Time.timeScale = 1f;
        GameManager.Instance.ChangeScene("Scene/Title");
    }
    public void OPTION()
    {
        if (UnityEngine.Rendering.SplashScreen.isFinished)
        {
            GameManager.Instance.ChangeScene("Scene/OPTION");
        }
    }
    // ゲームオーバー時のリトライなど
    public void BackToPreviousScene()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ChangeScene(GameManager.Instance.PreviousScene);
    }
    // ポーズ時のリトライなど
    public void Retry()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().name);
    }
    public void Stage11()
    {
        GameManager.Instance.ChangeScene("Stage1-1");
    }
    public void Stage12()
    {
        GameManager.Instance.ChangeScene("Stage1-2");
    }
    public void Stage13()
    {
        GameManager.Instance.ChangeScene("Stage1-3");
    }
    public void Continue()
    {
        // 再開時の音
        AudioManager.Instance.PlaySE(AUDIO.SE_PAUSEEXIT);
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("Scene/Pause");
    }
}
