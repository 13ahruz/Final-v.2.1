using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public float timeState = 1;
    public Sequence seq;
    public List<Transform> movingWoods;
    public bool woodMoveStarted;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        seq = DOTween.Sequence();
    }
    void Start()
    {

    }


    void Update()
    {

    }

    public void StopTime()
    {
        timeState = 0;
        seq.Pause();
        Bird.instance.seq.Pause();
    }

    public void ResumeTime()
    {
        timeState = 1;
        seq.Play();
        Bird.instance.seq.Play();
    }
}
