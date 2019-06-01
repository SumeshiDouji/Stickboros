using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPTIONManager : MonoBehaviour {
    private Slider Slider1;
    private Slider Slider2;
    private float BGMVol=0.5f;
    private float SEVol=0.5f;
    private float timeElapsed;
    private float timeOut=0.6f;
    //ボリューム保存用
    private string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private string SE_VOLUME_KEY = "SE_VOLUME_KEY";

    // Use this for initialization
    void Start () {
        Slider1 = GameObject.Find("BGMSlider").GetComponent<Slider>();
        Slider2 = GameObject.Find("SESlider").GetComponent<Slider>();

        Debug.Log("BGMの音量"+BGMVol);
        Debug.Log("SEの音量" + SEVol);
        // ボリュームをロード
        BGMVol = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGMVol);
        SEVol = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SEVol);
        Slider1.value = BGMVol;
        Slider2.value = SEVol;
        //BGM再生。AUDIO.BGM_BATTLEがBGMのファイル名
        AudioManager.Instance.PlayBGM(AUDIO.BGM_OPTION, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timeElapsed += Time.deltaTime;
        if (Slider2.value != SEVol)
        {
            if (timeElapsed >= timeOut)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_STUPID);

                timeElapsed = 0.0f;
            }
        }
        BGMVol =Slider1.value;
        SEVol = Slider2.value;
        AudioManager.Instance.ChangeVolume(BGMVol, SEVol);
    }
    public void TapToTitle()
    {
        // BGMを停める
        AudioManager.Instance.StopBGM();
        // ボリュームをセーブ
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVol);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVol);
        PlayerPrefs.Save();
        GameManager.Instance.ChangeScene("Scene/Title");
    }
}
