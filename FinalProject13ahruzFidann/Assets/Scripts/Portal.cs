using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class Portal : MonoBehaviour
{
    public static Portal instance;
    [SerializeField]
    private Transform ice;
    public PlayableDirector iceDirector;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        iceDirector = ice.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {


    }

}
