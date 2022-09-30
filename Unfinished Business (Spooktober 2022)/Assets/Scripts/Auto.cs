using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Auto : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI t;

    static bool auto;

    private void Start()
    {
        switch (auto)
        {
            case true:
                t.color = Color.green;
                break;
            case false:
                t.color = Color.white;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            auto = !auto;           
        }

        switch (auto)
        {
            case true:
                t.color = Color.green;
                break;
            case false:
                t.color = Color.white;
                break;
        }
    }
}
