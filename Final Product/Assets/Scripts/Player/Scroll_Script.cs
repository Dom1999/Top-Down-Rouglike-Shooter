using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Script : MonoBehaviour
{
    public float scrollSpeed = 100f;
    // Update is called once per frame
    void Update()
    {
        
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;
        offset = transform.position / new Vector2(scrollSpeed, scrollSpeed);

        mat.mainTextureOffset = offset;
    }
}
