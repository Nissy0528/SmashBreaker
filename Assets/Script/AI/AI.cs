using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize() { }

    // Update is called once per frame
    void Update()
    {
        AIUpdate();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual void AIUpdate() { }

    /// <summary>
    /// アクティブフラグ
    /// </summary>
    public virtual bool IsActive { get { return false;} }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        if (!enabled) return;

        Initialize();
        enabled = false;
    }
}
