using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdText : MonoBehaviour
{
    [SerializeField]
    Transform tmp;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            tmp.localPosition = new Vector3(-0.007f, 1.67f, 0);
            Debug.Log("Hello");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {
            tmp.localPosition = new Vector3(0.046f, 1.67f, 0f);
        }
    }
}
