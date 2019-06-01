using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UIBulletImage : MonoBehaviour {
    Image Img;
    GameObject playerob;
    Player playersc;
    [SerializeField]
    private Sprite AP;
    [SerializeField]
    private Sprite HE;
    // Use this for initialization
    void Start () {
        playerob = GameObject.FindGameObjectWithTag("Player");
        playersc = playerob.GetComponent<Player>();
        Img = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (playersc.ShotSwitch && Img.sprite != AP)
        {
            Img.sprite = AP;
        }
        else if(playersc.ShotSwitch==false && Img.sprite != HE)
        {
            Img.sprite = HE;
        }
    }
}
