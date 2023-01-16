using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Canon : MonoBehaviour
{
    public static Canon instance;
    private float shootSpeed = 1;
    private float shootRate = 1;
    private float nextShootTime;
    public GameObject bulletPref;
    public GameObject shootPos;
    public Transform shootDirection;
    public Sequence seq;
    public List<Transform> bullets;
    public bool leverStop = false;
    [SerializeField]
    private ParticleSystem canonShootParticle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        seq = GameManager.instance.seq;
        seq.Join(transform.DORotate(transform.eulerAngles + new Vector3(0, 120, 0), 1.5f))
        .Insert(1.5f, transform.DORotate(transform.eulerAngles + new Vector3(0, 240, 0), 1.5f))
        .Insert(3f, transform.DORotate(transform.eulerAngles + new Vector3(0, 360, 0), 1.5f))
        .SetLoops(-1, LoopType.Restart);
    }


    void Update()
    {
        if (Time.time > nextShootTime && GameManager.instance.timeState == 1 && !leverStop)
        {
            transform.GetChild(1).DOScaleX(0.8f, 0.12f).SetLoops(2, LoopType.Yoyo);
            transform.GetChild(1).DOScaleY(1.2f, 0.12f).SetLoops(2, LoopType.Yoyo);
            transform.GetChild(1).DOScaleZ(1.2f, 0.12f).SetLoops(2, LoopType.Yoyo);
            canonShootParticle.Play();
            transform.GetComponent<AudioSource>().Play();
            //transform.GetChild(1).DOShakeScale(0.1f, 0.3f);
            GameObject bullet = Instantiate(bulletPref, shootPos.transform.position, Quaternion.identity);
            bullets.Add(bullet.transform);
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.AddForce((shootDirection.position - shootPos.transform.position) * 6, ForceMode.Impulse);
            nextShootTime = Time.time + 0.8f;
        }

        if (GameManager.instance.timeState == 0)
        {
            foreach (Transform bullet in bullets)
            {
                bullet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                bullet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else if (GameManager.instance.timeState == 1)
        {
            foreach (Transform bullet in bullets)
            {
                bullet.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }

    }


}
