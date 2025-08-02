using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleSprite : MonoBehaviour
{
    [SerializeField] bool staticSprite = true;
    [SerializeField] Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0,sprites.Length)];
    }
}
