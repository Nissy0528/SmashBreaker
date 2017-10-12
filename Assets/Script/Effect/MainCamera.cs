using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float shakeTime;//振動時間（設定用）
    public float shakeRange;//振動の幅

    private Camera camera;//カメラ
    private Vector3 savePosition;//振動する前の座標
    private Vector3 screenMinPos;//画面の左下
    private Vector3 screenMaxPos;//画面の右上
    //振動する幅
    private Vector2 lowRange;
    private Vector2 maxRange;
    private float lifeTime;//振動時間
    private bool isShake;//trueなら振動開始

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
        screenMinPos = camera.ScreenToWorldPoint(Vector3.zero);//画面の左下の座標
        screenMaxPos = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1));//画面の右下の座標
        savePosition = transform.position;//振動前の座標取得
        lifeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Shake();//振動

        //振動開始
        if(isShake)
        {
            SetShake();
            isShake = false;
        }
    }

    /// <summary>
    /// 振動開始
    /// </summary>
    public void SetShake()
    {
        //振動の範囲設定
        lowRange = new Vector2(savePosition.x - shakeRange, savePosition.y - shakeRange);
        maxRange = new Vector2(savePosition.x + shakeRange, savePosition.y + shakeRange);
        lifeTime = shakeTime;
    }

    /// <summary>
    /// 振動
    /// </summary>
    private void Shake()
    {
        //振動時間が0になったら振動終了
        if (lifeTime < 0.0f)
        {
            transform.position = savePosition;
            lifeTime = 0.0f;
        }

        //振動時間が設定されたら振動
        if (lifeTime > 0.0f)
        {
            lifeTime -= Time.deltaTime;
            float x_val = Random.Range(lowRange.x, maxRange.x);
            float y_val = Random.Range(lowRange.y, maxRange.y);
            transform.position = new Vector3(x_val, y_val, transform.position.z);
        }
    }

    /// <summary>
    /// 画面の左下
    /// </summary>
    /// <returns></returns>
    public Vector3 ScreenMin
    {
        get { return screenMinPos; }
    }

    /// <summary>
    /// 画面の右下
    /// </summary>
    /// <returns></returns>
    public Vector3 ScreenMax
    {
        get { return screenMaxPos; }
    }
}
