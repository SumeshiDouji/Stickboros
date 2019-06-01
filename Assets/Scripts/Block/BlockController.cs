using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {
    SpriteRenderer MainSpriteRenderer;
    // レイヤーマスク
    public LayerMask layerMask;
    // アイテムはあるかどうか
    public bool ItemIn;
    // 中身が空かどうか
    bool isEmpty=false;
    // その力で壊せるか
    bool brokenpower = false;
    // 飛べるかどうか
    public bool canFly;
    public float turnsecond;
    float flytime;
    float speed=2f;
    // 中身が空のときのスプライト
    public Sprite EmptySprite;
    public Sprite Fly1;
    public Sprite Fly2;
    public GameObject[] Item;
    GameObject brokenob;

    //元の位置
    Vector3 OriginPosition;

    int ItemNum;
    // Use this for initialization
    void Start () {
        OriginPosition = transform.position;
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        brokenob = Resources.Load("Prefab/BoxBroken")as GameObject;
        if (canFly)
        {
            StartCoroutine("Turn");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // 中身がなくなったとき
        if (isEmpty && MainSpriteRenderer.sprite != EmptySprite)
        {
            MainSpriteRenderer.sprite = EmptySprite;
        }
        // 中身がある羽ブロック
        if (canFly && isEmpty == false)
        {
            flytime += Time.deltaTime;
            if (flytime > 0.5f)
            {
                MainSpriteRenderer.sprite = Fly2;
            }
            else if(flytime >= 0)
            {
                MainSpriteRenderer.sprite = Fly1;
            }
            if(flytime > 1f)
            {
                flytime = 0;
            }
            transform.Translate(speed*Time.deltaTime, Mathf.Cos(Time.time*5) / 50, 0);
        }
        else if (canFly && isEmpty == true)
        {
            transform.Translate(0, 0, 0);
        }

    }
    IEnumerator Turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnsecond);
            speed *= -1;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("PlayerBullet"))
        {
            if (isEmpty == false && ItemIn)
            {
                ItemOut(-1.6f);
            }
            else if (ItemIn == false)
            {
                StartCoroutine("EmptyReac");
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().rigid2d.velocity.y >= 0)
            {
                if (ItemIn && isEmpty == false)
                {
                    ItemOut(0.6f);
                }
                if (ItemIn == false)
                {
                    brokenpower = collision.gameObject.GetComponent<Player>().Level > 0;
                    if (brokenpower)
                    {
                        BoxBroken();
                    }
                    else
                    {
                        StartCoroutine("EmptyReac");
                    }
                }
            }
        }
    }
    // アイテムがあるボックスの反応
    void ItemOut(float itemposition)
    {
        ItemNum = Random.Range(0, Item.Length);
        Vector3 ItemPosition = new Vector3(transform.position.x, transform.position.y + itemposition, transform.position.z);
        Instantiate(Item[ItemNum], ItemPosition, transform.rotation);
        if (Item[ItemNum].tag != ("Coin"))
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_ITEMOUT);
        }
        isEmpty = true;
    }
    // 空のボックスの反応(壊せないとき)
    IEnumerator EmptyReac()
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_EMPTYBOX);
        transform.Translate(0, 0.1f, 0);
        yield return new WaitForSeconds(0.1f);
        transform.position = OriginPosition;
    }
    // ブロックを破壊されるとき
    void BoxBroken()
    {
        AudioManager.Instance.PlaySE(AUDIO.SE_BOXBREAK);
        Instantiate(brokenob,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
