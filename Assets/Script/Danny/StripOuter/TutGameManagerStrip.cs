using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutGameManagerStrip : MonoBehaviour
{
    public static TutGameManagerStrip instance;

    [SerializeField] private Animator tssAnimation;
    [SerializeField] private Animator grAnimation;
    [SerializeField] private Animator machineAnimation;
    [SerializeField] private Animator lengthAnimator;
    [SerializeField] private GameObject TSSCable;
    [SerializeField] private GameObject GRCable;
    [SerializeField] private GameObject rotateButton;


    [SerializeField] private Text missionText;
    [SerializeField] private Text errorText;
    [SerializeField] private Text errorCountText;
    [SerializeField] private Text lengthText;
    [SerializeField] private Text timeText;

    [SerializeField] private Vector3 distanceChecker;

    [SerializeField] private float oriDistance;
    [SerializeField] private float gameDistance;

    private string[] mission = new string[] { "Strip 1 TSS-802-D02 cable ", "Strip 1 101518GR cable 4mm | 20mm ", "Congrate!\nYou Unclock Next Level!" };
    private string[] cableQtyRequire = new string[] { "/1", "/1", "" };
    private string[] textError = new string[] { "Length is wrong! Touch strip threshold", "Length is wrong! Should be 20mm!", "Length is wrong! Should be 4mm!", "Length is wrong! Should be 20mm or 4mm!" };

    private float timer = 0.0f;

    private int errorCount = 0;
    public int missionComplete = 0;
    private int noCompleteCount = 0;

    public bool isCollide = false;
    public bool isTSS = true;
    private bool leftPosition, rightPosition = false;
    private bool leftStrip, rightStrip, gr20, gr4 = false;

    Vector3 tssCableStartPos;
    Vector3 grCableStartPos;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        missionText.text = mission[0] + noCompleteCount + cableQtyRequire[0];
        rightPosition = true;
        tssCableStartPos = TSSCable.transform.position;
        grCableStartPos = GRCable.transform.position;
        GRCable.GetComponent<Rigidbody>().isKinematic = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        timeText.text = timer.ToString("F0");

    }

    public void LengthShow()
    {
        if (missionComplete == 1)
        {
            if (GRCable.transform.position.x >= 21f)
            {
                oriDistance = (GRCable.transform.position.x - distanceChecker.x);
                gameDistance = oriDistance * 25;

                if ((gameDistance >= 3f && gameDistance < 6f) && !rightStrip)
                {
                    gameDistance = 4f;
                    TutStripManager.instance.tut2.SetInteger("tut2", 2);
                    TutStripManager.instance.actionButton[2].interactable = true;
                    lengthAnimator.SetInteger("correct", 1);
                }


                else if ((gameDistance >= 19f && gameDistance < 22f) && rightStrip)
                {
                    gameDistance = 20f;
                    TutStripManager.instance.tut2.SetInteger("tut2", 5);
                    TutStripManager.instance.actionButton[2].interactable = true;
                    lengthAnimator.SetInteger("correct", 1);
                }

                else
                {
                    lengthAnimator.SetInteger("correct", 0);
                    TutStripManager.instance.actionButton[2].interactable = false;
                    if (TutStripManager.instance.tut2.GetInteger("tut2") == 2)
                    {
                        TutStripManager.instance.tut2.SetInteger("tut2", 0);
                        TutStripManager.instance.tut2.SetTrigger("drag4");
                    }

                    else if (TutStripManager.instance.tut2.GetInteger("tut2") == 5)
                    {
                        TutStripManager.instance.tut2.SetInteger("tut2", 0);
                        TutStripManager.instance.tut2.SetTrigger("drag20");
                    }
                }


                lengthText.text = "Length: " + gameDistance.ToString("F0") + "mm";
            }

        }

        else
        {
            lengthText.gameObject.SetActive(false);
            lengthText.text = "";
        }
    }

    public void Mission()
    {
        SoundManager.PlayClickClip();
        machineAnimation.SetTrigger("run");
        if (missionComplete == 0 && isTSS)
        {
            if (isCollide && (leftPosition && !leftStrip))
            {
                leftStrip = true;
                SoundManager.PlayCorrectClip();
                tssAnimation.SetInteger("tss", 3);
                TSSCable.transform.position = new Vector3(22.06f, TSSCable.transform.position.y, TSSCable.transform.position.z);
                StartCoroutine(BackToPosition());

            }

            else if (isCollide && (rightPosition && !rightStrip))
            {
                rightStrip = true;
                SoundManager.PlayCorrectClip();
                tssAnimation.SetInteger("tss", 1);
                TSSCable.transform.position = new Vector3(21.5f, TSSCable.transform.position.y, TSSCable.transform.position.z);
                StartCoroutine(BackToPosition());

            }

            else if (isCollide)
            {
                TSSCable.transform.position = tssCableStartPos;
                ObjectMovement.instance.ActiveMove();
                TSSCable.GetComponent<Rigidbody>().isKinematic = false;
                Error();
            }

            else
            {
                errorText.text = textError[0];
                Error();

            }

            if (leftStrip && rightStrip)
            {
                noCompleteCount++;
                leftStrip = false; rightStrip = false;
                rightPosition = true; leftPosition = false;
                StartCoroutine(MissionDone());
            }
        }

        else if (missionComplete == 1 && !isTSS)
        {
            if ((!gr20 && gameDistance == 20f) && (leftPosition && !leftStrip))
            {
                gr20 = true;
                leftStrip = true;
                SoundManager.PlayCorrectClip();
                grAnimation.SetInteger("right4", 3);
                GRCable.transform.position = new Vector3(21.4404f, GRCable.transform.position.y, GRCable.transform.position.z);
                StartCoroutine(BackToPosition());
            }

            else if ((!gr20 && gameDistance == 20f) && (rightPosition && !rightStrip))
            {
                gr20 = true;
                rightStrip = true;
                SoundManager.PlayCorrectClip();
                grAnimation.SetInteger("right20", 1);
                GRCable.transform.position = new Vector3(21.39f, GRCable.transform.position.y, GRCable.transform.position.z);
                StartCoroutine(BackToPosition());
            }

            else if ((!gr4 && gameDistance == 4f) && (leftPosition && !leftStrip))
            {
                gr4 = true;
                leftStrip = true;
                SoundManager.PlayCorrectClip();
                grAnimation.SetInteger("right20", 3);
                GRCable.transform.position = new Vector3(21.419f, GRCable.transform.position.y, GRCable.transform.position.z);
                StartCoroutine(BackToPosition());
            }

            else if ((!gr4 && gameDistance == 4f) && (rightPosition && !rightStrip))
            {
                gr4 = true;
                rightStrip = true;
                SoundManager.PlayCorrectClip();
                grAnimation.SetInteger("right4", 1);
                GRCable.transform.position = new Vector3(21.05f, GRCable.transform.position.y, GRCable.transform.position.z);
                StartCoroutine(BackToPosition());
            }

            else
            {
                if (gr4)
                {
                    errorText.text = textError[1];
                }

                else if (gr20)
                {
                    errorText.text = textError[2];
                }

                else
                {
                    errorText.text = textError[3];
                }
                Error();

            }


            if (leftStrip && rightStrip)
            {
                noCompleteCount++;
                leftStrip = false; rightStrip = false;
                leftPosition = false; rightPosition = true;
                gr4 = false; gr20 = false;
                StartCoroutine(MissionDone());
            }
        }


        else
        {
            Error();
        }


    }

    IEnumerator BackToPosition()
    {
        TutStripManager.instance.actionButton[2].interactable = false;
        yield return new WaitForSeconds(1f);
        isCollide = false;
        if (rightStrip)
        {
            rotateButton.SetActive(true);
        }

        ObjectMovement.instance.ActiveMove();
        if (isTSS)
        {
            TSSCable.GetComponent<Rigidbody>().isKinematic = false;

            tssAnimation.SetInteger("tss", 0);
            TSSCable.transform.position = tssCableStartPos;
        }

        else
        {
            grAnimation.SetInteger("right4", 0);
            grAnimation.SetInteger("right20", 0);
            GRCable.transform.position = grCableStartPos;
            lengthText.text = "Length: 0" + "mm";

        }


    }


    IEnumerator MissionDone()
    {
        TutStripManager.instance.panel[0].SetActive(false);
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        UIManager.SetError(errorCountText.text);
        UIManager.SetTime(timeText.text);
        yield return new WaitForSeconds(1f);
        if (missionComplete == 0 && noCompleteCount == 1)
        {
            noCompleteCount = 0;
            missionComplete++;
            TutStripManager.instance.actionButton[1].interactable = true;
            lengthText.gameObject.SetActive(true);
        }

        if (missionComplete == 1 && noCompleteCount == 1)
        {
            noCompleteCount = 0;
            missionComplete++;
        }

        if (missionComplete == 2)
        {
            UIManager.StartCompletePanel();
        }
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];

    }

    public void Error()
    {
        SoundManager.PlayErroClip();
        errorCount++;
        errorCountText.text = errorCount.ToString();
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        StartCoroutine(UIManager.Error());

    }



    public void ChangeCableTSS()
    {
        SoundManager.PlayClickClip();
        isTSS = true;
        rightPosition = true;
        leftPosition = false;
        TSSCable.transform.position = tssCableStartPos;
        TSSCable.SetActive(true);
        GRCable.SetActive(false);
    }

    public void ChangeCableGR()
    {
        SoundManager.PlayClickClip();
        isTSS = false;
        rightPosition = true;
        leftPosition = false;
        GRCable.transform.position = grCableStartPos;
        TSSCable.SetActive(false);
        GRCable.SetActive(true);
    }

    public void CheckCurrentPosition()
    {
        SoundManager.PlayClickClip();
        if (leftPosition)
        {
            rightPosition = true;
            leftPosition = false;

        }

        else if (rightPosition)
        {
            leftPosition = true;
            rightPosition = false;
        }

        if (isTSS)
        {
            tssAnimation.SetInteger("tss", 2);
        }

        else
        {
            grAnimation.SetInteger("right4", 2);
            grAnimation.SetInteger("right20", 2);
        }

        rotateButton.SetActive(false);

    }

    //public void NextScene()
    //{
    //    if (missionComplete == 2)
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    }

    //    else
    //    {
    //        StartCoroutine(Error());
    //    }
    //}

}
