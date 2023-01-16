using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Woods : MonoBehaviour
{
    public Sequence seq;
    public static Woods instance;
    public bool startMove;
    public float startLoc;
    public bool timeStopped = false;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        startLoc = transform.localPosition.z;
        seq = DOTween.Sequence();
        //seq.AppendInterval(GameManager.instance.movingWoods.IndexOf(transform) * 0.8f);
        seq.Append(transform.DOLocalMoveZ(13f, 2 * ((13f - startLoc) / 5.5f)))
        .Insert(2 * ((13f - startLoc) / 5.5f), transform.DOLocalMoveZ(7.5f, 2f))
        .Insert((2 * (((13f - startLoc) / 5.5f) + 1)), transform.DOLocalMoveZ(startLoc, 2 * (startLoc - 7.5f) / 5.5f))
        .SetLoops(-1, LoopType.Restart);


    }

    void Update()
    {
        if ((GameManager.instance.timeState == 0) && !timeStopped)
        {
            timeStopped = true;
            seq.DOTimeScale(0, 0.1f);
        }
        if ((GameManager.instance.timeState == 1) && timeStopped)
        {
            timeStopped = false;
            seq.DOTimeScale(1, 0.1f);
        }
    }

    public void moveWoods()
    {
        if (!startMove)
        {


            startMove = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            // GameManager.instance.movingWoods.IndexOf(transform);
            seq.Pause();
            if (GameManager.instance.movingWoods.IndexOf(transform) != (GameManager.instance.movingWoods.Count - 1))
            {
                GameManager.instance.movingWoods[GameManager.instance.movingWoods.IndexOf(transform) + 1].GetComponent<Woods>().seq.Pause();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (GameManager.instance.movingWoods.IndexOf(transform) != 0)
            {
                GameManager.instance.movingWoods[GameManager.instance.movingWoods.IndexOf(transform) - 1].GetComponent<Woods>().seq.Play();
            }
        }
    }
    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.transform.CompareTag("Player"))
    //     {

    //         // GameManager.instance.movingWoods.IndexOf(transform);
    //         seq.Play();
    //         if (GameManager.instance.movingWoods.IndexOf(transform) != (GameManager.instance.movingWoods.Count - 1))
    //         {
    //             GameManager.instance.movingWoods[GameManager.instance.movingWoods.IndexOf(transform) + 1].GetComponent<Woods>().seq.Play();
    //         }
    //     }
    // }









}
