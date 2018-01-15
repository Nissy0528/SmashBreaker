using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParameterMenu
{
    /// <summary>
    /// 横向きの壁用パラメータ
    /// </summary>
    private struct Wall_H_Param
    {
        public float y;
        public float width;
        public float colSizeX;
    }

    /// <summary>
    /// 縦向きの壁用パラメータ
    /// </summary>
    private struct Wall_V_Param
    {
        public float x;
        public float width;
        public float colSizeX;
    }

    private PlayerParameter player_param;//プレイヤー設定ウィンドウ
    private BossParameter boss_param;//ボス設定ウィンドウ
    private Wall_H_Param h_param;//横向きの壁用パラメータ
    private Wall_V_Param v_param;//縦向きの壁用パラメータ
    private SpriteRenderer map;//床
    private GameManager gameManager;//ゲーム管理クラス
    private List<GameObject> stages = new List<GameObject>();//ステージの配列
    private Object stageFile;//ステージオブジェクトが入ってるファイル
    private float mapSize;//ステージサイズ
    private float cameraZoom;//カメラのズーム値
    private bool isDebug;//デバックフラグ

    //シーン上に表示
    public ParameterMenu()
    {
        SceneView.onSceneGUIDelegate += MenuOnSceneGUI;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    //ウィンドウを作成
    private void MenuOnSceneGUI(SceneView sceneView)
    {
        //ウィンドウの初期位置
        Rect windowRect = new Rect(10, 30, 120, 30);

        Handles.BeginGUI();
        windowRect = GUILayout.Window(1, windowRect, GUI, "設定メニュー");
        Handles.EndGUI();
    }

    /// <summary>
    /// ウィンドウにGUIを配置
    /// </summary>
    /// <param name="id"></param>
    private void GUI(int id)
    {
        //ボタンが押されたらプレイヤーの設定を開く
        if (GUILayout.Button("プレイヤーの設定を開く"))
        {
            player_param = new PlayerParameter();
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();

        //ボタンが押されたらボスの設定を開く
        if (GUILayout.Button("ボスの設定を開く"))
        {
            boss_param = new BossParameter();
            SceneView.onSceneGUIDelegate -= MenuOnSceneGUI;
        }
        EditorGUILayout.Space();

        float width = 80f;

        //ステージの大きさ設定
        GUILayout.BeginHorizontal();
        GUILayout.Label("ステージサイズ : ");
        mapSize = EditorGUILayout.FloatField(mapSize, GUILayout.Width(width));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //カメラのズーム設定
        GUILayout.BeginHorizontal();
        GUILayout.Label("カメラズーム : ");
        cameraZoom = EditorGUILayout.FloatField(cameraZoom, GUILayout.Width(width));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //ボタンが押されたらステージ情報設定
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("ステージ読み込み"))
        {
            SetMap();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //ボタンが押されたらステージの大きさ変更
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("設定"))
        {
            SetMapSize();
            SetCameraZoom();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //ステージファイル読み込み
        GUILayout.BeginHorizontal();
        GUILayout.Label("ステージファイル : ");
        stageFile = EditorGUILayout.ObjectField(stageFile, typeof(object), true);
        GUILayout.EndHorizontal();

        //ボタンが押されたらステージの大きさ変更
        GUILayout.BeginHorizontal();
        for (int i = 0; i < stages.Count; i++)
        {
            if (GUILayout.Button("ステージ" + i))
            {

            }
            EditorGUILayout.Space();
        }
        GUILayout.EndHorizontal();

        //デバッグフラグ設定
        GUILayout.BeginHorizontal();
        GUILayout.Label("デバッグモード : ");
        isDebug = EditorGUILayout.Toggle(isDebug, GUILayout.Width(width));
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        gameManager.IsDebug = isDebug;
    }

    /// <summary>
    /// ステージ情報設定
    /// </summary>
    private void SetMap()
    {
        GameObject stage = GameObject.Find("Stage");
        if (stage != null)
        {
            map = stage.GetComponent<SpriteRenderer>();
        }
        if (map != null)
        {
            mapSize = map.size.x / 1.92f;
        }

        Camera camera = GameObject.FindObjectOfType<Camera>();
        cameraZoom = camera.orthographicSize;

        h_param.y = 11.8f;
        h_param.width = 36.2f;
        h_param.colSizeX = 35.2f;

        v_param.x = -18.3f;
        v_param.width = 23.4f;
        v_param.colSizeX = 22.5f;
    }

    /// <summary>
    /// ステージの大きさ変更
    /// </summary>
    private void SetMapSize()
    {
        if (map == null) return;

        //床の大きさ変更
        map.size = new Vector2(1.92f * mapSize, 1.28f * mapSize);

        //横向きの壁
        GameObject[] walls_H = new GameObject[]
        {
            GameObject.Find("Top"),
            GameObject.Find("Under")
        };

        //縦向きの壁
        GameObject[] walls_V = new GameObject[]
        {
            GameObject.Find("Right"),
            GameObject.Find("Left")
        };

        //角の壁
        GameObject[] corners = new GameObject[]
        {
            GameObject.Find("Wall_LT"),
            GameObject.Find("Wall_RT"),
            GameObject.Find("Wall_LB"),
            GameObject.Find("Wall_RB")
        };

        foreach (var h in walls_H)
        {
            WallSet(h, new Vector2(0, h_param.y), h_param.width, h_param.colSizeX);
            h_param.y *= -1;
        }

        foreach (var v in walls_V)
        {
            WallSet(v, new Vector2(v_param.x, 0), v_param.width, v_param.colSizeX);
            v_param.x *= -1;
        }

        int num = 0;
        foreach (var c in corners)
        {
            num++;
            ConerSet(c, num);
        }

        h_param.y = 11.8f;
        v_param.x = -18.3f;
    }

    /// <summary>
    /// 垂直な壁設定
    /// </summary>
    private void WallSet(GameObject obj, Vector2 pos, float spriteSize, float colSize)
    {
        Vector2 position = Vector2.zero;
        Vector2 s_Size = Vector2.zero;
        Vector2 colliderSize = Vector2.zero;

        position = obj.transform.position;
        if (pos.x != 0.0f)
        {
            position.x = pos.x * mapSize;
        }
        if (pos.y != 0.0f)
        {
            position.y = pos.y * mapSize;
        }
        obj.transform.position = position;

        s_Size = obj.GetComponent<SpriteRenderer>().size;
        s_Size.x = spriteSize * mapSize;
        obj.GetComponent<SpriteRenderer>().size = s_Size;

        colliderSize = obj.GetComponent<BoxCollider2D>().size;
        colliderSize.x = colSize * mapSize;
        obj.GetComponent<BoxCollider2D>().size = colliderSize;
    }

    /// <summary>
    /// 角の壁設定
    /// </summary>
    private void ConerSet(GameObject obj, int num)
    {
        obj.transform.position = new Vector2(v_param.x * mapSize, h_param.y * mapSize);

        v_param.x *= -1;
        if (num == 2)
        {
            h_param.y *= -1;
        }
    }

    /// <summary>
    /// カメラのズーム設定
    /// </summary>
    private void SetCameraZoom()
    {
        Camera camera = GameObject.FindObjectOfType<Camera>();
        camera.orthographicSize = cameraZoom;
    }

    /// <summary>
    /// ステージ読み込み
    /// </summary>
    private void ReadStages()
    {
        if (stageFile == null) return;

        var so = new SerializedObject(stageFile);
        var sp = so.GetIterator();
        //while(sp.Next(true))
        //{
        //    stages.Add(new GameObject(sp.pro))
        //}
    }
}
