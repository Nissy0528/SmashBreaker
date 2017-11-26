using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MapReader
{
    public const char SPRIT_CHAR = ',';//区切り

    private string comment = "//";//コメントを表す文字列
    //読み込んだデータ
    private List<List<string>> data = new List<List<string>>();
    TextAsset textAsset = null;
    private int counter;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="c">コメント</param>
    public MapReader(string c = "//")
    {
        textAsset = null;
        data = new List<List<string>>(10);
        comment = c;
    }

    /// <summary>
    /// テキストファイル読み込み
    /// </summary>
    /// <param name="fileName">読み込むファイルの名前</param>
    /// <returns></returns>
    private TextReader CreateTextReader(string fileName)
    {
        textAsset = Resources.Load<TextAsset>(fileName);
        return new StringReader(textAsset.text);
    }

    /// <summary>
    /// データを保存
    /// </summary>
    /// <param name="name">ファイル名</param>
    /// <returns></returns>
    public bool Load(string name)
    {
        data.Clear();
        TextReader reader = CreateTextReader(name);

        counter = 0;
        string line = "";
        while ((line = reader.ReadLine()) != null)
        {
            //コメントが入ってるときはスキップする
            if (line.Contains(comment))
            {
                continue;
            }

            //今の列をマス毎に区切る
            string[] fields = line.Split(SPRIT_CHAR);
            data.Add(new List<string>());

            foreach (var field in fields)
            {
                if (field.Contains(comment) || field == "")
                {
                    continue;
                }
                data[counter].Add(field);
            }
            counter++;
        }

        //読み込んだリソースを開放する
        Resources.UnloadAsset(textAsset);
        textAsset = null;
        Resources.UnloadUnusedAssets();
        return true;
    }

    //↓読み込んだデータ取得
    //String型で取得
    public string GetString(int row, int col)
    {
        return data[row][col];
    }
    //Bool型で取得
    public bool GetBool(int row, int col)
    {
        string data = GetString(row, col);
        return bool.Parse(data);
    }
    //Int型で取得
    public int GetInt(int row,int col)
    {
        string data = GetString(row, col);
        return int.Parse(data);
    }
    //Float型で取得
    public float GetFloat(int row,int col)
    {
        string data = GetString(row, col);
        return float.Parse(data);
    }

    /// <summary>
    /// マップサイズを取得
    /// </summary>
    public int GetSize
    {
        get { return counter; }
    }
}
