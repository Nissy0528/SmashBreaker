using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MapEditor : EditorWindow
{
    private Object imgDirectory;//画像ディレクトリ
    private int mapSize = 10;//マップのマスの数
    private int selectedImageNum;//選択した画像の番号
    private float gridSize = 32.0f;//グリッドの大きさ
    private string outputFileName = "*";//出力ファイル名
    private string selectedImagePath;//選択した画像のパス
    private MapCreateWindow window;//マップ作製用ウィンドウ

    [MenuItem("Window/MapEditor")]
    static void ShowMainWindow()
    {
        GetWindow(typeof(MapEditor));
    }

    /// <summary>
    /// GUI作成
    /// </summary>
    void OnGUI()
    {
        //画像が入ってるファイルを指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("画像ファイル : ", GUILayout.Width(110));
        imgDirectory = EditorGUILayout.ObjectField(imgDirectory, typeof(Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //マップのサイズを指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("マップサイズ : ", GUILayout.Width(110));
        mapSize = EditorGUILayout.IntField(mapSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //グリッドサイズを指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("グリッドサイズ : ", GUILayout.Width(110));
        gridSize = EditorGUILayout.FloatField(gridSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //保存するファイルの名前を指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("保存するファイルの名前 : ", GUILayout.Width(110));
        outputFileName = (string)EditorGUILayout.TextField(outputFileName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        DrawImageParts();//画像一覧を表示
        DrawSelectedImage();//選択した画像表示
        DrawMapWndowButton();//マップウィンドウを開く
    }

    /// <summary>
    /// 画像一覧をボタンで選択できる形にして出力
    /// </summary>
    private void DrawImageParts()
    {
        if (imgDirectory != null)
        {
            Vector2 position = Vector2.zero;//ボタンの座標
            Vector2 size = new Vector2(50.0f, 50.0f);//ボタンのサイズ
            float maxW = 300.0f;//ボタンの最大X座標

            string path = AssetDatabase.GetAssetPath(imgDirectory);
            string[] names = Directory.GetFiles(path, "*.png");
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < names.Length; i++)
            {
                if (position.x > maxW)
                {
                    position.x = 0.0f;
                    position.y += size.y;
                    EditorGUILayout.EndHorizontal();
                }
                if (position.x == 0.0f)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                GUILayout.FlexibleSpace();
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(names[i], typeof(Texture2D));
                if (GUILayout.Button(tex, GUILayout.MaxWidth(size.x), GUILayout.MaxHeight(size.y), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    selectedImageNum = i + 1;
                    selectedImagePath = names[i];
                }
                GUILayout.FlexibleSpace();
                position.x += size.x;
            }
            EditorGUILayout.EndVertical();
        }
    }

    /// <summary>
    /// 選択した画像データを表示
    /// </summary>
    private void DrawSelectedImage()
    {
        if (selectedImagePath != null)
        {
            Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(selectedImagePath, typeof(Texture2D));
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("select : " + selectedImagePath);
            GUILayout.Box(tex);
            EditorGUILayout.EndVertical();
        }
    }

    /// <summary>
    /// マップウィンドウを開くボタンを作成
    /// </summary>
    private void DrawMapWndowButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("マップエディタを開く"))
        {
            if (window == null)
            {
                window = MapCreateWindow.WillAppear(this);
            }
            else
            {
                window.Focus();
            }
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 選択した画像のパス
    /// </summary>
    private string SelectedImagePath
    {
        get { return selectedImagePath; }
    }

    /// <summary>
    /// 選択した画像の番号
    /// </summary>
    private int SlectedImageNum
    {
        get { return selectedImageNum; }
    }

    /// <summary>
    /// マップサイズ
    /// </summary>
    public int MapSize
    {
        get { return mapSize; }
    }

    /// <summary>
    /// グリッドサイズ
    /// </summary>
    public float GridSize
    {
        get { return gridSize; }
    }

    /// <summary>
    /// 出力先パスを生成
    /// </summary>
    /// <returns></returns>
    public string OutputFilePath()
    {
        string resultPath = "Assets/Resources/MapData";

        return resultPath + "/" + outputFileName + ".txt";
    }

    /// <summary>
    /// マップ生成用のウィンドウ
    /// </summary>
    public class MapCreateWindow : EditorWindow
    {
        // マップウィンドウのサイズ
        const float WINDOW_W = 750.0f;
        const float WINDOW_H = 750.0f;
        // マップのグリッド数
        private int mapSize = 0;
        // グリッドサイズ。親から値をもらう
        private float gridSize = 0.0f;
        // マップデータ
        private string[,] map;
        //マップ表示用データ
        private string[,] mapNmae;
        // グリッドの四角
        private Rect[,] gridRect;
        // 親ウィンドウの参照を持つ
        private MapEditor parent;

        public static MapCreateWindow WillAppear(MapEditor _parent)
        {
            MapCreateWindow window = (MapCreateWindow)EditorWindow.GetWindow(typeof(MapCreateWindow), false);
            window.Show();
            window.minSize = new Vector2(WINDOW_W, WINDOW_H);
            window.SetParent(_parent);
            window.init();
            return window;
        }

        /// <summary>
        /// 親ウィンドウ設定
        /// </summary>
        /// <param name="_parent"></param>
        private void SetParent(MapEditor _parent)
        {
            parent = _parent;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void init()
        {
            mapSize = parent.MapSize;
            gridSize = parent.GridSize;

            //マップデータを初期化
            map = new string[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = "0";
                }
            }
            mapNmae = new string[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    mapNmae[i, j] = "";
                }
            }
            //グリッドデータを生成
            gridRect = CreateGrid(mapSize);
        }

        /// <summary>
        /// GUI作成
        /// </summary>
        void OnGUI()
        {
            //グリッド線を描画
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    DrawGridLine(gridRect[y, x]);
                }
            }

            //クリックされた位置に画像データを入れる
            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                Vector2 pos = Event.current.mousePosition;
                int x;
                //x位置を先に計算して、計算回数を減らす
                for (x = 0; x < mapSize; x++)
                {
                    Rect r = gridRect[0, x];
                    if (r.x <= pos.x && pos.x <= r.x + r.width)
                    {
                        break;
                    }
                }

                //後はy位置だけ探す
                for (int y = 0; y < mapSize; y++)
                {
                    if (gridRect[y, x].Contains(pos))
                    {
                        //消しゴムの時はデータを消す
                        if (parent.SelectedImagePath.IndexOf("eraser") > -1)
                        {
                            map[y, x] = "0";
                        }
                        else
                        {
                            mapNmae[y, x] = parent.selectedImagePath;
                            map[y, x] = parent.selectedImageNum.ToString();
                        }
                        Repaint();
                        break;
                    }
                }
            }

            //選択した画像を描画する
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (mapNmae[y, x] != null && mapNmae[y, x].Length > 0)
                    {
                        Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(mapNmae[y, x], typeof(Texture2D));
                        GUI.DrawTexture(gridRect[y, x], tex);
                    }
                }
            }

            //出力ボタン
            Rect rect = new Rect(0, WINDOW_H - 50, 300, 50);
            GUILayout.BeginArea(rect);
            if (GUILayout.Button("ファイルを出力", GUILayout.MinWidth(300), GUILayout.MinHeight(50)))
            {
                if (parent.outputFileName == "*")
                {
                    EditorUtility.DisplayDialog("MapEditor", "ファイル名入力してください。", "OK");
                    return;
                }
                OutputFile();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }

        /// <summary>
        /// グリッドデータを生成
        /// </summary>
        /// <param name="div">マップサイズ</param>
        /// <returns></returns>
        private Rect[,] CreateGrid(int div)
        {
            int sizeW = div;
            int sizeH = div;

            float x = 0.0f;
            float y = 0.0f;
            float w = gridSize;
            float h = gridSize;

            Rect[,] resultRects = new Rect[sizeH, sizeW];

            for (int yy = 0; yy < sizeH; yy++)
            {
                x = 0.0f;
                for (int xx = 0; xx < sizeW; xx++)
                {
                    Rect r = new Rect(new Vector2(x, y), new Vector2(w, h));
                    resultRects[yy, xx] = r;
                    x += w;
                }
                y += h;
            }
            return resultRects;
        }

        /// <summary>
        /// グリッド線を描画
        /// </summary>
        /// <param name="r">矩形</param>
        private void DrawGridLine(Rect r)
        {
            //グリッド
            Handles.color = new Color(1f, 1f, 1f, 0.5f);

            //上の線
            Handles.DrawLine(
                new Vector2(r.position.x, r.position.y),
                new Vector2(r.position.x + r.size.x, r.position.y));

            //下の線
            Handles.DrawLine(
                new Vector2(r.position.x, r.position.y + r.size.y),
                new Vector2(r.position.x + r.size.x, r.position.y + r.size.y));

            //左の線
            Handles.DrawLine(
                new Vector2(r.position.x, r.position.y),
                new Vector2(r.position.x, r.position.y + r.size.y));

            //右の線
            Handles.DrawLine(
                new Vector2(r.position.x + r.size.x, r.position.y),
                new Vector2(r.position.x + r.size.x, r.position.y + r.size.y));
        }

        /// <summary>
        /// ファイルで出力
        /// </summary>
        private void OutputFile()
        {
            string path = parent.OutputFilePath();

            FileInfo fileInfo = new FileInfo(path);
            StreamWriter sw = fileInfo.AppendText();
            sw.WriteLine(GetMapStrFormat());
            sw.Flush();
            sw.Close();

            //完了ポップアップ
            EditorUtility.DisplayDialog("MapEditor", "ファイルを作成しました。\n" + path, "OK");
        }

        /// <summary>
        /// 出力するマップデータ整形
        /// </summary>
        /// <returns></returns>
        private string GetMapStrFormat()
        {
            string result = "";
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    result += OutputDataFormat(map[i, j]);
                    if (j < mapSize - 1)
                    {
                        result += ",";
                    }
                }
                result += "\n";
            }
            return result;
        }

        //不要な文字を切り出す（パスと拡張し切り出し）
        private string OutputDataFormat(string data)
        {
            if (data != null && data.Length > 0)
            {
                string[] tmps = data.Split('\\');
                string fileName = tmps[tmps.Length - 1];
                return fileName.Split('.')[0];
            }
            else
            {
                return "";
            }
        }
    }
}
