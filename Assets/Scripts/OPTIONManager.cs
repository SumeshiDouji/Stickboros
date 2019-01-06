using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPTIONManager : MonoBehaviour {
    private Slider Slider1;
    private Slider Slider2;
    private float BGMVol;
    private float SEVol;
    private float timeElapsed;
    private float timeOut=0.6f;
	// Use this for initialization
	void Start () {
        Slider1 = GameObject.Find("BGMSlider").GetComponent<Slider>();
        Slider2 = GameObject.Find("SESlider").GetComponent<Slider>();
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
}
