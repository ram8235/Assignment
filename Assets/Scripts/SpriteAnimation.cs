using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] frameArray;
    private int currentFrame;
    private float timer;

    public void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 0.2f)
        {
            timer -= 0.2f;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            GetComponent<Image>().sprite = frameArray[currentFrame];
        }
    }
}
