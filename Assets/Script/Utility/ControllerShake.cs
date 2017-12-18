using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerShake : MonoBehaviour
{
    /// <summary>
    /// コントローラー振動
    /// </summary>
    public static void Shake(float value)
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerIndex pI = (PlayerIndex)i;
            GamePadState state = GamePad.GetState(pI);
            if (state.IsConnected)
            {
                GamePad.SetVibration(pI, value, value);
            }
        }
    }
}
