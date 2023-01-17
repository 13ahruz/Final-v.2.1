using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using DG.Tweening;
using CASP.CameraManager;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float fallMultiplier;
    [SerializeField]
    private float lowjumpMultiplier;
    [SerializeField]
    private float jumpVelocity;
    [SerializeField]
    private float jumpForce = 5;
    private Rigidbody rb;
    [SerializeField]
    public bool isGrounded;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float walkSpeed;
    private float horizontal;
    private float vertical;
    private float currentVelocity;
    private float smoothTurnTime = 0.1f;
    private Vector3 direction;
    private Animator anim;
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform throwPos;
    [SerializeField]
    private float throwPower;
    public bool changeLayer = true;
    [SerializeField]
    private AvatarMask freeAavatarMask;
    [SerializeField]
    private AvatarMask handsAvatarMask;
    [SerializeField]
    private AnimatorController animC;
    [SerializeField]
    Transform canon;
    private bool canonCamActive = false;
    [SerializeField]
    bool isPicked = false;

    [SerializeField]
    private float canonThrowPower = 30f;
    [SerializeField]
    Transform riverStone;
    [SerializeField]
    Transform stonePullPos;
    private bool torchOnHand;
    [SerializeField]
    private Transform torch;
    [SerializeField]
    private Transform torchPos;
    private bool doorClose = true;
    [SerializeField]
    List<ParticleSystem> finalKotukParticles;
    [SerializeField]
    List<Transform> finalKotukler;
    bool portalBurned;
    [SerializeField]
    private Transform finalBuz;
    private bool cavePuzzleCamActive = false;
    [SerializeField]
    private int health = 100;
    public bool isAlive = true;
    [SerializeField]
    Transform magicPos;
    GameObject magicSphere;
    [SerializeField]
    Transform magicMovePos;
    [SerializeField]
    Transform portal2;
    [SerializeField]
    private ParticleSystem canonShootParticle;
    [SerializeField]
    private List<ParticleSystem> sarmasiqPart;
    [SerializeField]
    private Transform sarmasiq;
    [SerializeField]
    private Transform finalBuzzz;
    [SerializeField]
    Transform sarmasiqPos;








    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine(startScreen());

    }


    private void Update()
    {
        if (torchOnHand && Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Throw");
            StartCoroutine(takeTorch());
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CameraManager.instance.OpenCamera("Game", 1f, CameraEaseStates.EaseInOut);
        }

        UIManager.instance.healtBar.GetComponent<Image>().fillAmount = health * 0.01f;

        if (!canonCamActive)
        {
            PlayerJump();
            PlayerMove();
            PlayerCrouch();
            ThrowBulletAnim();
            Dive();
        }

        else if (canonCamActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canonShootParticle.Play();
                Canon.instance.GetComponent<AudioSource>().Play();
                GameObject canonBullet = Instantiate(Canon.instance.bulletPref, Canon.instance.shootPos.transform.position, Quaternion.identity);
                Rigidbody rbC = canonBullet.AddComponent<Rigidbody>();
                rbC.AddForce((Canon.instance.shootDirection.position - Canon.instance.shootPos.transform.position) * canonThrowPower, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(canonCamWaiter());
                CameraManager.instance.OpenCamera("Game", 1.5f, CameraEaseStates.EaseInOut);

            }

            float angle = Mathf.SmoothDampAngle(canon.transform.eulerAngles.y, cam.eulerAngles.y - 90, ref currentVelocity, 0.1f);
            //canon.transform.rotation = Quaternion.Euler(canon.eulerAngles.x, cam.eulerAngles.y - 90, canon.eulerAngles.z);
            canon.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        if (isPicked)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(riverStoneWaiter());
                anim.SetBool("Pushing", false);
                anim.SetBool("CrouchDown", false);
                anim.SetTrigger("CrouchUp");
            }
        }

        if (health <= 0 && isAlive)
        {
            SoundManager.instance.Play("Death");
            anim.SetTrigger("Death");
            isAlive = false;
            UIManager.instance.CloseGeneralPanel();
            StartCoroutine(OpenLosePanel());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DOTween.To(() => UIManager.instance.timeBar.GetComponent<Image>().fillAmount, x => UIManager.instance.timeBar.GetComponent<Image>().fillAmount = x, 0, 3f);
            SoundManager.instance.Play("Time");
            anim.SetTrigger("Time");
            magicSphere = Instantiate(bullet, magicPos.position, Quaternion.identity);
            magicSphere.transform.SetParent(magicPos);
            magicSphere.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void StopTime()
    {

        magicSphere.transform.SetParent(magicMovePos);
        magicSphere.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f).OnComplete(() =>
        {
            magicSphere.transform.SetParent(null);
        });
        GameManager.instance.StopTime();
        StartCoroutine(ResumeTime());
    }

    private void FixedUpdate()
    {

    }





    private void ThrowBulletAnim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            changeLayer = false;
            anim.SetTrigger("Throw");
            transform.DORotate(new Vector3(transform.rotation.z, cam.eulerAngles.y, transform.rotation.z), 0.2f);
        }
    }

    private void ThrowBullet()
    {
        GameObject spawnedBullet = Instantiate(bullet, throwPos.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce((cam.transform.rotation * Vector3.forward + new Vector3(0, 0.15f, 0)) * throwPower, ForceMode.Impulse);

    }


    private void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && !anim.GetBool("CrouchDown"))
        {

            anim.SetBool("CrouchDown", true);
            anim.SetTrigger("CrouchDownTrigger");
        }
        else if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Jump"))
        {
            if (anim.GetBool("CrouchDown"))
            {
                anim.SetTrigger("CrouchUp");
                anim.SetBool("CrouchDown", false);
            }

        }
    }

    private void Dive()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Dive");
        }
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * lowjumpMultiplier * Time.deltaTime;
            }

        }
    }

    public void throwTorch()
    {
        if (torch != null)
        {
            torch.DOJump(sarmasiqPos.position, 0.005f, 1, 0.4f).OnComplete(() =>
            {
                torch.SetParent(sarmasiqPos);
                foreach (ParticleSystem part in sarmasiqPart)
                {
                    part.Play();
                }
                sarmasiq.DOScale(0, 3f);
            });
            anim.SetBool("Torch", false);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * lowjumpMultiplier * Time.deltaTime;
        }

    }

    public void FootSound()
    {
        SoundManager.instance.Play("Foot");
    }

    void PlayerMove()
    {
        if (!isPicked && isAlive)
        {
            anim.SetFloat("Walk", direction.magnitude);
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            direction = new Vector3(horizontal, 0, vertical).normalized;
            if ((direction.magnitude > 0.01))
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTurnTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rb.MovePosition(transform.position + (moveDir.normalized * movementSpeed * Time.fixedDeltaTime));

            }


            if (Input.GetButton("Debug Multiplier"))
            {
                movementSpeed = runSpeed;
                anim.SetBool("Run", true);
            }

            if (Input.GetButtonUp("Debug Multiplier"))
            {
                movementSpeed = walkSpeed;
                anim.SetBool("Run", false);
            }
        }

        if (isPicked)
        {
            anim.SetFloat("Walk", direction.magnitude);
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            direction = new Vector3(horizontal, 0, vertical).normalized;
            if ((direction.magnitude > 0.01))
            {
                float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y) - 180;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTurnTime);
                //transform.rotation = Quaternion.Euler(0, angle, 0);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rb.MovePosition(transform.position - (moveDir.normalized * movementSpeed * Time.fixedDeltaTime));

            }

            riverStone.transform.position = new Vector3(stonePullPos.transform.position.x, stonePullPos.transform.position.y + 0.1f, stonePullPos.transform.position.z);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (other.transform.CompareTag("CanonBullet"))
        {
            health -= 10;
            anim.SetTrigger("Fall");
            SoundManager.instance.Play("Damage");
        }

        if (other.transform.CompareTag("Water"))
        {
            health = 0;
        }


    }

    private void LayerWeightChange()
    {
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WoodStarter"))
        {
            Woods.instance.moveWoods();
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Lever") && !canonCamActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                UIManager.instance.pressF.SetActive(false);
                StartCoroutine(canonCamWaiter());
                other.transform.GetChild(0).DOLocalRotate(new Vector3(12f, -372f, -13f), 0.3f);
                Canon.instance.seq.Kill();
                Canon.instance.leverStop = true;
                CameraManager.instance.OpenCamera("Canon", 1.5f, CameraEaseStates.EaseInOut);

            }
        }

        if (other.CompareTag("Stone"))
        {
            if (Input.GetKeyDown(KeyCode.F) && !isPicked)
            {
                UIManager.instance.pressF.SetActive(false);
                StartCoroutine(riverStoneWaiter());
                anim.SetTrigger("Push");
                anim.SetBool("Pushing", true);
                anim.SetBool("CrouchDown", true);
                anim.SetTrigger("CrouchDownTrigger");
            }
        }


        if (other.CompareTag("Torch") && !torchOnHand)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                torch = other.transform;
                UIManager.instance.pressF.SetActive(false);
                other.transform.SetParent(torchPos);
                other.transform.localRotation = Quaternion.Euler(0, 0, 0);
                other.transform.localPosition = new Vector3(0, 0, 0);
                anim.SetBool("Torch", true);
                StartCoroutine(takeTorch());
            }
        }

        if (other.CompareTag("Portal") && !portalBurned)
        {
            StartCoroutine(burnPortal());
        }




    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FirePart"))
        {
            health = 0;
            Debug.Log(health);
        }

        if (other.CompareTag("PortalBir"))
        {
            SoundManager.instance.Play("Portal");
            transform.position = portal2.position;
        }

        if (other.CompareTag("PortalFinish"))
        {
            UIManager.instance.pressF.SetActive(false);
            SoundManager.instance.Play("Portal");
            UIManager.instance.OpenWinPanel();
            UIManager.instance.CloseGeneralPanel();
        }

        if (other.CompareTag("Lever"))
        {
            UIManager.instance.pressF.SetActive(true);
        }
        if (other.CompareTag("Stone"))
        {
            UIManager.instance.pressF.SetActive(true);
        }
        if (other.CompareTag("Torch"))
        {
            UIManager.instance.pressF.SetActive(true);
        }
        if (other.CompareTag("Portal"))
        {
            UIManager.instance.pressF.SetActive(true);
        }

        if (other.CompareTag("Magara"))
        {
            CameraManager.instance.OpenCamera("Cave", 1f, CameraEaseStates.EaseInOut);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lever"))
        {
            UIManager.instance.pressF.SetActive(false);
        }
        if (other.CompareTag("Stone"))
        {
            UIManager.instance.pressF.SetActive(false);
        }
        if (other.CompareTag("Torch"))
        {
            UIManager.instance.pressF.SetActive(false);
        }
        if (other.CompareTag("Portal"))
        {
            UIManager.instance.pressF.SetActive(false);
        }

    }







    IEnumerator canonCamWaiter()
    {
        yield return new WaitForSeconds(1f);
        canonCamActive = !canonCamActive;
    }

    IEnumerator riverStoneWaiter()
    {
        yield return new WaitForSeconds(0.4f);
        isPicked = !isPicked;
    }

    IEnumerator takeTorch()
    {
        yield return new WaitForSeconds(1f);
        torchOnHand = !torchOnHand;
    }

    IEnumerator burnPortal()
    {
        var seq = DOTween.Sequence();
        yield return new WaitForSeconds(1f);
        foreach (ParticleSystem part in finalKotukParticles)
        {
            part.Play();
        }
        portalBurned = !portalBurned;
        finalBuz.DOScale(0, 3f).OnComplete
        (() =>
        {
            Portal.instance.iceDirector.Play();

            foreach (Transform finalKot in finalKotukler)
            {
                finalKot.DOScale(0, 2f);
            }

        });
        yield return new WaitForSeconds(7f);
        if (finalBuzzz.gameObject.activeInHierarchy)
        {
            finalBuzzz.gameObject.SetActive(false);
        }
    }

    IEnumerator cavePuzzleWaiter()
    {
        yield return new WaitForSeconds(0.5f);
        cavePuzzleCamActive = !cavePuzzleCamActive;

    }

    IEnumerator ResumeTime()
    {
        yield return new WaitForSeconds(3f);
        SoundManager.instance.Stop("Time");
        Bird.instance.anim.speed = 1;
        magicSphere.transform.DOScale(new Vector3(0, 0, 0), 1f);
        GameManager.instance.ResumeTime();
        yield return new WaitForSeconds(1f);
        Destroy(magicSphere);
    }

    IEnumerator OpenLosePanel()
    {
        yield return new WaitForSeconds(2f);
        UIManager.instance.OpenLosePanel();
    }

    IEnumerator startScreen()
    {
        UIManager.instance.IntroBGPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIManager.instance.IntroBGPanel.SetActive(false);
        UIManager.instance.IntroPanel.SetActive(true);
        Time.timeScale = 0;
        yield return new WaitForSeconds(3f);
        SoundManager.instance.Play("Time");
        Bird.instance.anim.speed = 0;
        anim.SetTrigger("Time");
        magicSphere = Instantiate(bullet, magicPos.position, Quaternion.identity);
        magicSphere.transform.SetParent(magicPos);
        magicSphere.transform.localPosition = new Vector3(0, 0, 0);
    }



}


