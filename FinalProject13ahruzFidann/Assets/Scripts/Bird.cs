using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CASP.CameraManager;

public class Bird : MonoBehaviour
{
    public static Bird instance;
    [SerializeField]
    Transform portal2;
    [SerializeField]
    Transform portal1;
    [SerializeField]
    Transform birdMove;

    [SerializeField]
    Transform startColorUp;
    [SerializeField]
    Transform startColorDown;
    public Sequence seq;
    public Animator anim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        seq = DOTween.Sequence();
        anim = GetComponent<Animator>();
        seq.Append(transform.DOMove(portal1.position, 10f));
        StartCoroutine(birdCamera());


    }


    void Update()
    {
        if (transform.position == portal1.position)
        {
            SoundManager.instance.Play("Portal");
            transform.position = portal2.position;
        }

        if (transform.position == portal2.position)
        {
            seq.Append(transform.DOMove(birdMove.position, 50f));
        }
    }

    IEnumerator birdCamera()
    {
        yield return new WaitForSeconds(15f);
        CameraManager.instance.OpenCamera("Game", 2f, CameraEaseStates.EaseInOut);
        startColorDown.DOLocalMoveY(-800, 1f);
        startColorUp.DOLocalMoveY(850, 1f);

    }


}
