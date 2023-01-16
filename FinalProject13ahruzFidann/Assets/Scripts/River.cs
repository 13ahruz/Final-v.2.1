using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class River : MonoBehaviour
{
    [SerializeField]
    private float positionY;
    [SerializeField]
    ParticleSystem woodParticle1;
    [SerializeField]
    ParticleSystem woodParticle2;
    void Start()
    {
        positionY = transform.position.y;
    }


    void Update()
    {
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            DOTween.To(() => positionY, x => positionY = x, 0.07351704f, 2f).OnComplete(() =>
            {
                DOTween.To(() => woodParticle1.transform.localScale, x => woodParticle1.transform.localScale = x, new Vector3(0, 0, 0), 2f);
                DOTween.To(() => woodParticle2.transform.localScale, x => woodParticle2.transform.localScale = x, new Vector3(0, 0, 0), 2f);

            }).OnComplete(() =>
            {
                Destroy(woodParticle1.gameObject);
                Destroy(woodParticle2.gameObject);
            });
        }

    }
}
