using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToView : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            float height = Camera.main.orthographicSize * 2;
            float width = height * Screen.width/ Screen.height;

            Sprite sprite = spriteRenderer.sprite;
            float unitWidth = sprite.textureRect.width / sprite.pixelsPerUnit;
            float unitHeight = sprite.textureRect.height / sprite.pixelsPerUnit;

            transform.localScale = new Vector3(width / unitWidth, height / unitHeight, 1);
        }
    }
}
