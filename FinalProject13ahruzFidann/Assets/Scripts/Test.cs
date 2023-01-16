using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class Test : MonoBehaviour
{
    Sequence seq;
    bool timeStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        seq = GameManager.instance.seq;
        seq.Append(transform.DOMoveY(8f, 0.5f)).SetLoops(-1, LoopType.Yoyo);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !timeStopped)
        {
            timeStopped = true;
            GameManager.instance.seq.DOTimeScale(0, 0.4f);
            GameManager.instance.timeState = 0;
        }
        if (Input.GetKeyDown(KeyCode.R) && timeStopped)
        {
            timeStopped = false;
            GameManager.instance.seq.DOTimeScale(1, 0.4f);
            GameManager.instance.timeState = 1;
        }
    }

}
