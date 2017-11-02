using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashText : MonoBehaviour
{
    public float shakeTime;//振動時間（設定用）
    public float shakeRange;//振動の幅

    private RectTransform rect;
    private Vector3 savePosition;//振動する前の座標
    //振動する幅
    private Vector2 lowRange;
    private Vector2 maxRange;
    private float lifeTime;//振動時間

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        savePosition = rect.position;//振動前の座標設定
        //振動の範囲設定
        lowRange = new Vector2(savePosition.x - shakeRange, savePosition.y - shakeRange);
        maxRange = new Vector2(savePosition.x + shakeRange, savePosition.y + shakeRange);
        lifeTime = shakeTime;

    }

    // Update is called once per frame
    void Update()
    {
        Shake();//振動
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
            lifeTime = shakeTime;
            gameObject.SetActive(false);
        }

        //振動時間が設定されたら振動
        if (lifeTime > 0.0f)
        {
            lifeTime -= 0.1f;
            float x_val = Random.Range(lowRange.x, maxRange.x);
            float y_val = Random.Range(lowRange.y, maxRange.y);
            rect.position = new Vector3(x_val, y_val, rect.position.z);
        }
    }
}
