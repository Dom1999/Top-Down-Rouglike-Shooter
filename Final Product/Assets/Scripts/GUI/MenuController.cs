using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static bool menuOn;
    // Start is called before the first frame update
    void Start()
    {
        menuOn = false;
    }

    // Update is called once per frame
    void Update()
    {
            gameObject.transform.GetChild(0).gameObject.SetActive(menuOn);
    }
}
