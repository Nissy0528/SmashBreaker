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
    private GameManager gameManager;
    private List<GameObject> spEffectObjcts;

    // Use this for initialization
    void Start()
    {
        slider = GetComponent<Slider>();
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
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
            gameManager.SetTimeScale(gameManager.stopTime, 1.0f);
            //FindObjectOfType<GameManager>().Game(true);
            maxBG.SetActive(false);
            FindObjectOfType<Canvas>().sortingLayerName = "High";
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

        gameManager.SetTimeScale(100, 0.0f);
        maxBG.SetActive(true);
        FindObjectOfType<Canvas>().sortingLayerName = "Middle";

        spEffectObjcts.Add(Instantiate(spEffect));//エフェクト生成
        int num = spEffectObjcts.Count - 1;

        //生成位置設定（SPゲージの位置）
        Vector3 pos = FindObjectOfType<Camera>().ScreenToWorldPoint(transform.position);
        pos.y += 4;
        spEffectObjcts[num].transform.position = new Vector2(-pos.x, pos.y);

        //向かう座標を設定（プレイヤーの位置）
        SPEffect spEffect_class = spEffectObjcts[num].GetComponent<SPEffect>();
        spEffect_class.SetEndPoint(player.transform.position);

        //二つ目は左右反転させる
        if (num == 1)
        {
            spEffect_class.radius *= -1;
            //FindObjectOfType<GameManager>().Game(false);
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

	public float Sp
	{
		get
		{
			return sp;
		}
	}

	public float Max
	{
		get
		{
			return max;
		}
	}
}
