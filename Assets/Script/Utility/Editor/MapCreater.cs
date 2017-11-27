using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MapCreater : EditorWindow
{
    private GameObject[] mapTip;//配置するオブジェクト
    private GameObject map;//マップの親オブジェクト
    private Object txtFile;//マップデータファイル
    private Object mapTipDirectry;//配置するオブジェクトファイル
    private MapReader reader = new MapReader();//マップデータ読み込み
    private Vector2 screenPos;//画面の右上の座標
    private string path;//マップデータのパス
    private int mapSize;//マップのサイズ
    private float size = 32.0f;//オブジェクトのサイズ

    [MenuItem("Window/MapCreater")]
    static void ShowMainWindow()
    {
        GetWindow(typeof(MapCreater));
    }

    /// <summary>
    /// GUI作成
    /// </summary>
    private void OnGUI()
    {
        //テキストファイル指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("テキストファイル : ", GUILayout.Width(110));
        txtFile = EditorGUILayout.ObjectField(txtFile, typeof(Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //配置するオブジェクトのファイル指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("マップ上のオブジェクト : ", GUILayout.Width(110));
        mapTipDirectry = EditorGUILayout.ObjectField(mapTipDirectry, typeof(Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //サイズを指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("画像サイズ : ", GUILayout.Width(110));
        size = EditorGUILayout.FloatField(size);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //ボタンが押されたらマップを生成
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("マップを生成"))
        {
            ErrorDialog();
            Create();
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// エラー表示
    /// </summary>
    private void ErrorDialog()
    {
        if (txtFile == null)
        {
            EditorUtility.DisplayDialog("MapCreater", "読み込むファイルを指定してください。", "OK");
        }

        if (mapTipDirectry == null)
        {
            EditorUtility.DisplayDialog("MapCreater", "マップ上のオブジェクトが入ったファイルを指定してください。", "OK");
        }
    }

    /// <summary>
    /// マップデータをもとにオブジェクト配置
    /// </summary>
    private void Create()
    {
        if (txtFile == null || mapTipDirectry == null) return;

        LoadMapTip();
        MapLoad();
        CreateBase();
        CreateHeight();

        //完了ポップアップ
        EditorUtility.DisplayDialog("MapCreater", "マップを生成しました。", "OK");
    }

    /// <summary>
    /// 配置するオブジェクトを読み込む
    /// </summary>
    private void LoadMapTip()
    {
        string path = AssetDatabase.GetAssetPath(mapTipDirectry);//オブジェクトが入ってるファイルのパス

        //パスのファイルの中のプレハブを読み込む
        string[] names = Directory.GetFiles(path, "*.prefab");
        mapTip = new GameObject[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            mapTip[i] = (GameObject)AssetDatabase.LoadAssetAtPath(names[i], typeof(GameObject));
        }
    }

    /// <summary>
    /// マップデータを読み込む
    /// </summary>
    private void MapLoad()
    {
        this.path = AssetDatabase.GetAssetPath(txtFile);
        string p = this.path.Replace("Assets/Resources/", "");
        string path = p.Replace(".txt", "");

        reader.Load(path);

        mapSize = reader.GetSize - 1;
    }

    /// <summary>
    /// 基盤設定
    /// </summary>
    private void CreateBase()
    {
        size /= 100f;//配置するオブジェクトのサイズ指定

        //初期配置位置を画面の右上に設定
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenPos = cam.ViewportToWorldPoint(new Vector2(0.0f, 1.0f));
        screenPos += new Vector2(size / 2f, -size / 2f);

        //マップの親オブジェクト生成（空のオブジェクト）
        map = new GameObject
        {
            name = "NewMap"
        };
    }

    /// <summary>
    /// 行部分のマップを生成
    /// </summary>
    private void CreateHeight()
    {
        int num = 0;

        for (int i = 0; i < mapSize; i++)
        {
            CreateWidth(i, num);
        }
    }

    /// <summary>
    /// 列部分のマップを生成
    /// </summary>
    /// <param name="i">行</param>
    private void CreateWidth(int i, int num)
    {
        for (int j = 0; j < mapSize; j++)
        {
            num = reader.GetInt(i, j);
            CreateTip(i, j, num);
        }
    }

    /// <summary>
    /// オブジェクト配置
    /// </summary>
    /// <param name="i">行</param>
    /// <param name="j">列</param>
    /// <param name="num">オブジェクト番号</param>
    private void CreateTip(int i, int j, int num)
    {
        if (num == 0) return;

        GameObject obj = GameObject.Instantiate(mapTip[num - 1]);
        obj.transform.position = screenPos + new Vector2(size * j, -size * i);
        obj.transform.parent = map.transform;
    }
}
