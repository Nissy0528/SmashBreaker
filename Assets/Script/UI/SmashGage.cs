using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashGage : MonoBehaviour
{
    public Image backGround;
    public GameObject spEffect;
    public GameObject maxBG;

    private float sp;//スマッシュポイント
    private float max;//最大スマッシュポイント
    private bool isMax;//ワンパンフラグ
    private bool isSpawn;//エフェクト生成フラグ
    private Slider slider;
    private Player player;
    private List<GameObject> spEffectObjcts;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        player = FindObjectOfType<Player>();
        spEffectObjcts = new List<GameObject>();
        max = player.GetParam.maxSP;
        isMax = false;
        isSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        SmashPoint();
        GageColor();
        SpawnEffect();
        spEffectObjcts.RemoveAll(x => x == null);
        if (isSpawn && spEffectObjcts.Count == 0)
        {
            FindObjectOfType<GameManager>().Game(true);
            maxBG.SetActive(false);
        }
    }

    /// <summary>
    /// プレイヤーのスマッシュポイントを表示
    /// </summary>
    private void SmashPoint()
    {
        sp = player.GetParam.sp;
        slider.value = sp / max;

        if (sp >= player.GetParam.maxSP)
        {
            isMax = true;
        }

        if (isMax && sp <= 0.0f)
        {
            isMax = false;
            isSpawn = false;
        }
    }

    /// <summary>
    /// ゲージの色変更
    /// </summary>
    private void GageColor()
    {
        if (isMax)
        {
            backGround.color = Color.red;
        }
        else
        {
            backGround.color = Color.blue;
        }
    }

    /// <summary>
    /// エフェクト生成
    /// </summary>
    private void SpawnEffect()
    {
        if (isSpawn || !isMax) return;

        spEffectObjcts.Add(Instantiate(spEffect));
        int num = spEffectObjcts.Count - 1;
        RectTransform rect = GetComponent<RectTransform>();
        Vector3 pos = FindObjectOfType<Camera>().ScreenToWorldPoint(rect.position);
        spEffectObjcts[num].transform.position = new Vector2(-pos.x, pos.y);
        SPEffect spEffect_class = spEffectObjcts[num].GetComponent<SPEffect>();
        spEffect_class.SetEndPoint(player.transform.position);
        if (num == 1)
        {
            spEffect_class.radius *= -1;
            FindObjectOfType<GameManager>().Game(false);
            maxBG.SetActive(true);
            isSpawn = true;
        }
    }

    /// <summary>
    /// ワンパンフラグ取得
    /// </summary>
    public bool IsMax
    {
        get { return isMax; }
        set { isMax = value; }
    }
}
