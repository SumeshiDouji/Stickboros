using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour {

    [SerializeField]
    private Camera camera_object; //カメラを取得
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private GameObject Sphere;
    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物
    private float speedx=0,speedy=0;
    Ray mRay, cRay;

    void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            cursor.transform.Translate(speedx * Time.deltaTime, speedy * Time.deltaTime, 0);
        }
        cursor.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 100, Input.GetAxis("Vertical") * Time.deltaTime * 100, 0);
        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, new Vector3(0, 0, 1), 100);

            // 可視化
            Debug.DrawRay(pos, new Vector3(0, 0, 100), Color.blue, 1);

            if (hit)
            {
                // コンソールにhitしたGameObjectを出力
                Debug.Log(hit.collider);
            }
        }
    }
    public void OnCursorLeftButton()
    {
        speedx += -100;
    }
    public void OnCursorRightButton()
    {
        speedx += 100;
    }
    public void OnCursorUpButton()
    {
        speedy += 100;
    }
    public void OnCursorDownButton()
    {
        speedy += -100;
    }
    public void CursorXSpeedReset()
    {
        speedx = 0;
    }
    public void CursorYSpeedReset()
    {
        speedy = 0;
    }
}
