using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    private AudioSource audio;//サウンド
    private AudioClip clip;//効果音

    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if(!audio.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 再生する効果音設定
    /// </summary>
    /// <param name="clip"></param>
    public void SetClip(AudioClip clip)
    {
        this.clip = clip;
    }
}
