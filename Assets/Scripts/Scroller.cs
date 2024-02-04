using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RawImage bgImg;
    [SerializeField] private float imgX, imgY;

    // Update is called once per frame
    void Update()
    {
        bgImg.uvRect = new Rect(bgImg.uvRect.position + new Vector2(imgX, imgY) * Time.deltaTime, bgImg.uvRect.size);
    }
}
