using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    public Material[] mat;

    private GameObject player;
    private GameObject child;
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Chara");
        child = transform.GetChild(0).gameObject;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (child.transform.position.y < player.transform.position.y)
        {
            sprite.sortingLayerName = "Middle";
            sprite.material = mat[0];
        }
        else
        {
            sprite.sortingLayerName = "Default";
            sprite.material = mat[1];
        }
    }
}
