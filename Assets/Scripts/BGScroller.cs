using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float offsetSpeed = -0.0006f;
    private Renderer myRenderer;

    [HideInInspector]
    public bool canScroll;

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll)
        {
            myRenderer.material.mainTextureOffset -= new Vector2(offsetSpeed, 0);
        }
    }
}
