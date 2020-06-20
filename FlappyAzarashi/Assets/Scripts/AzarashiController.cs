/*
Unity5 3D/2Dゲーム開発 実践入門
作りながら覚えるスマートフォンゲーム制作
https://www.socym.co.jp/book/967
のFlappyAzarashiをベースにしている
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;
	public SerialHandler serialHandler;
    //あざらしが上昇するときの加速度センサのz軸の値
    public const int Z_THREAD = 10000;

    private bool GetButton = false;
    public bool IsDead()
    {
        return isDead;
    }
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //最高高度に達していない場合に限りタップの入力を受け付ける
        //加速度センサから上昇を検知したとき
        if(GetButton && transform.position.y < maxHeight)
        {
            Flap();
        }

        //角度を反映
        ApplyAngle();

        //angleが水平以上だったら，アニメーターのflapフラグをtrueにする
        animator.SetBool("flap", angle >= 0.0f);
    }

    public void Flap()
    {
        //死んだら羽ばたかない
        if(isDead) return;
        //重力が聞いていないときは操作しない
        if(rb2d.isKinematic) return;

       //Velocityを直接書き換えて上方向に加速
       rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    void ApplyAngle()
    {
        //現在の速度，相対速度から進んでいる角度を求める
        float targetAngle;
        //死亡したら常にしたを向く
        if(isDead)
        {
            targetAngle = -90.0f;
        }
        else{
            targetAngle = 
                Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }

        //回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        //Rotationの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if(isDead) return;
        //何かにぶつかったら死亡フラグを建てる
        isDead = true;
    }

    public void SetSteerActive(bool active)
    {
        //Rigidbodyのオン，オフを切り替える
        rb2d.isKinematic = !active;
    }
	void Start () {
		//信号を受信したときに、そのメッセージの処理を行う
		serialHandler.OnDataReceived += OnDataReceived;
	}
	void OnDataReceived(string message) {
    int i = int.Parse(message);
	if(i > Z_THREAD){
		GetButton = true;
	}
	else{
		GetButton = false;
	}
	}
}
