using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureAnimator : MonoBehaviour
{
    public Object imagDirectory;//画像ファイル

    private Sprite[] textures;//ファイル内の画像
    private Sprite currentTex;//現在の画像
    private int texNum;//現在の画像番号
    private float time;//再生間隔（設定用）
    private float cnt;//再生間隔
    private bool isPlay;//再生フラグ
    private bool isLoop;//ループ再生フラグ

    // Use this for initialization
    void Awake()
    {
        ReadImage();//ファイル内の画像を読み込む
    }

    /// <summary>
    /// ファイル内の画像を読み込む
    /// </summary>
    private void ReadImage()
    {
        if (imagDirectory == null) return;

        string path = AssetDatabase.GetAssetPath(imagDirectory);
        string[] names = Directory.GetFiles(path, "*.png");
        textures = new Sprite[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            textures[i] = (Sprite)AssetDatabase.LoadAssetAtPath(names[i], typeof(Sprite));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Animation();//アニメーション
        SetTexture();//画像更新
    }

    /// <summary>
    /// アニメーション再生
    /// </summary>
    private void Animation()
    {
        if (!isPlay) return;//再生フラグがfalseなら何もしない

        cnt += Time.deltaTime;//カウント加算
        //指定した時間になったら次の画像にコマ送り
        if (cnt >= time)
        {
            if (texNum < textures.Length)
            {
                texNum++;
            }
            cnt = 0.0f;
        }
        Loop();//ループ再生
    }

    /// <summary>
    /// ロープ再生
    /// </summary>
    private void Loop()
    {
        if (!isLoop) return;

        if (texNum >= textures.Length)
        {
            texNum = 0;
        }
    }

    /// <summary>
    /// 画像更新
    /// </summary>
    private void SetTexture()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) return;

        currentTex = textures[texNum];//現在の画像を更新
        sprite.sprite = currentTex;
    }

    /// <summary>
    /// 動的にコマ送り
    /// </summary>
    public void NextTex()
    {
        if (texNum < textures.Length)
        {
            texNum++;
        }
        Loop();
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        isPlay = false;
        cnt = 0.0f;
    }

    /// <summary>
    /// 現在の画像
    /// </summary>
    public int CurrentTex
    {
        get { return texNum; }
        set { texNum = value; }
    }

    /// <summary>
    /// 再生速度設定
    /// </summary>
    public float SetTime
    {
        set { time = value; }
    }

    /// <summary>
    /// 再生フラグ
    /// </summary>
    public bool IsPlay
    {
        get { return isPlay; }
        set { isPlay = value; }
    }

    /// <summary>
    /// ループ再生フラグ
    /// </summary>
    public bool IsLoop
    {
        get { return isLoop; }
        set { isLoop = value; }
    }
}
