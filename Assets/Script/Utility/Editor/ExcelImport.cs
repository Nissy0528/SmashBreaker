﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class ExcelImport
{
    public static void Import(string file, int num)
    {
        if (file.EndsWith(".xls"))
        {
            string asset_path = "Assets/Data/Asset/" + Path.GetFileNameWithoutExtension(file) + ".asset";

            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                CharaDataSet charaDataSet = AssetDatabase.LoadAssetAtPath(asset_path, typeof(CharaDataSet)) as CharaDataSet;
                if (charaDataSet == null)
                {
                    charaDataSet = ScriptableObject.CreateInstance<CharaDataSet>();
                    AssetDatabase.CreateAsset(charaDataSet, asset_path);
                }
                charaDataSet.Clear();

                IWorkbook book = new HSSFWorkbook(fs);
                int sheetNum = book.NumberOfSheets;
                for (int i = 0; i < sheetNum; ++i)
                {
                    ISheet sheet = book.GetSheetAt(i);

                    int firstRow = sheet.FirstRowNum;
                    int lastRow = sheet.LastRowNum;

                    for (int rowIdx = firstRow + 1; rowIdx <= lastRow; ++rowIdx)
                    {
                        IRow row = sheet.GetRow(rowIdx);
                        if (row == null) { continue; }

                        int cellIdx = 2;

                        ICell valueCell = row.GetCell(cellIdx);

                        SetData(num, charaDataSet, rowIdx, valueCell);
                    }
                }

                //データ更新
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath(asset_path, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty(obj);
            }
        }
    }

    /// <summary>
    /// 指定したデータを設定
    /// </summary>
    /// <param name="i"></param>
    /// <param name="rowIdx"></param>
    /// <param name="valueCell"></param>
    /// <returns></returns>
    private static void SetData(int i,CharaDataSet charaDataSet, int rowIdx, ICell valueCell)
    {
        switch (i)
        {
            case 0:
                 PlayerData(charaDataSet,rowIdx, valueCell);
                break;
        }
    }

    /// <summary>
    /// プレイヤーパラメータデータ
    /// </summary>
    /// <param name="rowIdx"></param>
    /// <param name="valueCell"></param>
    /// <returns></returns>
    private static void PlayerData(CharaDataSet charaDataSet, int rowIdx, ICell valueCell)
    {
        PlayerData charaData = new PlayerData();
        charaData.param = (float)valueCell.NumericCellValue;

        charaDataSet.PlayerAdd(charaData);
    }
}
