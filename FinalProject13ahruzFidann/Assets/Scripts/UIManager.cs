using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("General Panel")]
    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject Play;
    public GameObject timeBar;
    public GameObject healtBar;



    [Header("Settings Panel")]
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject SettingsBackground;
    public GameObject musicImage;
    public GameObject soundImage;





    [Header("Win Panel")]
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject WinPanelBackground;
    [SerializeField] List<Transform> winRingBoxes;
    [SerializeField] private GameObject get2xButton;
    [SerializeField] private GameObject getCollectedMoneyButton;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private GameObject levelCompletedText;

    [Header("Intro Panel")]
    public GameObject IntroPanel;
    public GameObject IntroBGPanel;

    public GameObject downColor;
    public GameObject upColor;
    public GameObject pressF;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenSettingsPanel();
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //demo value for start
        // GoldAmount = 1670;
        // PlayerPrefs.SetInt("gold",GoldAmount);
        // GemAmount = 650;
        // PlayerPrefs.SetInt("gem",GemAmount);


        // Getting Values on the Beginning

    }

    private void Start()
    {
        IntroPanel.SetActive(true);
        downColor.SetActive(false);
        upColor.SetActive(false);
    }




    //////// General Panel ///////
    public void OpenGeneralPanel()
    {
        GeneralPanel.SetActive(true);
        //  tapToStart.transform.DOScale(new Vector3(3.25f, 2.243f, 2.549f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void CloseGeneralPanel()
    {
        GeneralPanel.SetActive(false);
    }

    //////// Home Panel ///////
    /*public void OpenIntroPanel()
    {
        IntroPanel.SetActive(true);
        DOTween.To(() => IntroLoading.GetComponent<Image>().fillAmount, x => IntroLoading.GetComponent<Image>().fillAmount = x, 1, 4);
    }
    public void CloseIntroPanel()
    {
        IntroPanel.SetActive(false);
    }*/


    //////// Settings Panel ///////
    public void OpenSettingsPanel()
    {
        GameManager.instance.StopTime();
        SettingsPanel.SetActive(true);
        SettingsPanel.transform.localScale = Vector3.zero;
        Image panelImg = SettingsPanel.GetComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0);
        DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(255, 255, 255, 255), 0.3f);
        SettingsPanel.transform.DOScale(1f, 0.2f);
    }

    public void CloseSettingsPanel()
    {
        GameManager.instance.ResumeTime();
        Image panelImg = SettingsPanel.GetComponent<Image>();
        DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(0, 0, 0, 0), 0.3f);
        SettingsBackground.transform.DOScale(0f, 0.2f);
        SettingsPanel.SetActive(false);
    }

    public GameObject losePanel;
    public void OpenLosePanel()
    {
        losePanel.SetActive(true);
    }


    //////// Win Panel ///////
    public void OpenWinPanel()
    {
        WinPanel.SetActive(true);
        // Image panelImg = WinPanel.GetComponent<Image>();
        // DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(32, 32, 32, 0), 0.2f);
        // WinPanelBackground.transform.DOScale(new Vector3(0.057f, 0.039f, 0.036f), 0.3f);
        // get2xButton.transform.DOScale(new Vector3(0.283f, 0.744f, 1.010f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        // levelCompletedText.transform.DOLocalMoveY(30.7f, 0.3f);
        // levelCompletedText.transform.DORotate(new Vector3(-5.169f, -20.05f, -13.209f), 2f).SetLoops(-1, LoopType.Yoyo);

    }
    /* public void CloseWinPanel()
     {
         DOTween.To(() => int.Parse(winText.text), x => winText.text = x.ToString(), MoneyManager.instance.moneyCount, 2).OnComplete(() =>
         {
             Image panelImg = WinPanel.GetComponent<Image>();
             DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(255, 255, 255, 0), 0.3f);
             WinPanelBackground.transform.DOScale(0, 0.3f);
             WinPanel.SetActive(false);
             GetMoney();
         });



     }*/



    // IEnumerator startGame()
    // {

    //     OpenIntroPanel();
    //     yield return new WaitForSeconds(4f);
    //     PlayerController.instance.SpeedMultiplier = 0;
    //     CloseIntroPanel();
    //     CloseSettingsPanel();
    //     CloseWinPanel();
    //     OpenGeneralPanel();
    // }



    public void SoundUp()
    {
        if (soundImage.GetComponent<Image>().fillAmount < 0.98f)
        {
            soundImage.GetComponent<Image>().fillAmount += 7 / 100f;
        }
    }

    public void SoundDown()
    {
        if (soundImage.GetComponent<Image>().fillAmount > 0.2f)
            soundImage.GetComponent<Image>().fillAmount -= 7 / 100f;
    }

    public void MusicUp()
    {
        if (musicImage.GetComponent<Image>().fillAmount < 0.98f)
        {
            musicImage.GetComponent<Image>().fillAmount += 7 / 100f;
        }
    }

    public void MusicDown()
    {
        if (musicImage.GetComponent<Image>().fillAmount > 0.2f)
            musicImage.GetComponent<Image>().fillAmount -= 7 / 100f;
    }



    /* public void TapToStart()
     {
         tapToStart.SetActive(false);
         PlayerController.instance.SpeedMultiplier = 6;
     }
 */

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        IntroPanel.SetActive(false);
        downColor.SetActive(true);
        upColor.SetActive(true);
    }

    /* IEnumerator IntroScreen()
     {
         IntroPanel.SetActive(true);
         yield return new WaitForSeconds(2f);
         IntroPanel.SetActive(false);

     }*/

}