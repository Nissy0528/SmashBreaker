using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;



/// <summary>
/// Rateの設定関連
/// </summary>
public class SpRateSettings
{
	private static int[] reserve;

	public static string file = "Assets/Data/Asset/SpRate.asset";
	public static SpRate GetRates()
	{
		return (SpRate)AssetDatabase.LoadAssetAtPath(file, typeof(SpRate));
	}

	public static void ChangeMaxHp(int maxHp)
	{
		var rate = GetRates();
		reserve = rate.spRates;
		rate.spRates = new int[maxHp];

		var length = Mathf.Min(maxHp, reserve.Length);
		for (int i = 0; i < length; i++)
		{
			rate.spRates[i] = reserve[i];
		}
	}
}