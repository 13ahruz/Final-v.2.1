using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class Mirror : MonoBehaviour
{

    [SerializeField]
    private TMP_Text nextLevelName;
    Sequence seq;

    void Start()
    {
        seq = DOTween.Sequence();
    }


    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Torch"))
        {

            seq.Append(DOTween.To(() => nextLevelName.color, x => nextLevelName.color = x, new Color32(255, 255, 255, 255), 2f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        seq.Kill();
    }


}
