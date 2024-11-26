using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Color OddColor;
    public Color EvenColor;

    public void SetColor(bool isOddColor)
    {
        Renderer.color = isOddColor ? EvenColor : OddColor;
    }
}
