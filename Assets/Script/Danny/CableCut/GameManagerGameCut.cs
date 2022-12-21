using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManagerGameCut : MonoBehaviour
{
    public static GameManagerGameCut instance;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Animator lengthAnimator;
    [SerializeField] private GameObject distanceChecker;
    [SerializeField] private GameObject TSSCable;
    [SerializeField] private GameObject frontCableTSS;
    [SerializeField] private GameObject backCableTSS;
    [SerializeField] private GameObject GRCable;
    [SerializeField] private GameObject machinePanel;

    [SerializeField] private Text missionText;
    [SerializeField] private Text lengthText;
    [SerializeField] private Text errorText;
    [SerializeField] private Text errorCountText;
    [SerializeField] private Text timeText;
    [SerializeField] private float oriDistance;
    [SerializeField] private float gameDistance;
    [SerializeField] private float deviderScaleTSS;
    [SerializeField] private GameObject[] arrowDown;
    [SerializeField] private Text[] attributes;
    [SerializeField] private Button[] settingButtons;
    [SerializeField] private Button[] numpadButtons;
    private float currentPoint = 0;
    private float touchPointScale = 0;
    private float move = 0;
    private int raycastLength = 1;
    private float timer = 0.0f;
    private bool insertCable = false;
    private string[] mission = new string[] { "Cut a TSS-802-D02 1840mm cable " , "Cut a TSS-802-D02 2840mm cable ", "Cut 4 101518GR 75mm cable ", "Congrate!\nYou Unclock Next Level!" };
    private string[] cableQtyRequire = new string[] { "/1", "/1", "/4", "" };
    private string[] textError = new string[] { "Length is wrong! Should be 1840mm!", "Length is wrong! Should be 2840mm!", "Haven't insert cable!", "Your attribute is wrong! Please Try Again!", "Cable cutted more than requirement!" };
    private int [] attributeCompleteStartIndex = new int[] { 4, 3, 75, 6, 20, 60, 10, 1, 4};
    private int [] attributeCompleteEndIndex = new int[] { 4, 3, 75, 10, 20, 60, 10, 4, 4};

    private int missionNo = 1;
    private int missionComplete = 0;
    private int noCompleteCount = 0;
    private int errorCount = 0;
    private bool isCollide = false;
    private int attributeIndex = 0;
    private bool setButton = false;
    private int playTime = 0;


    RaycastHit hit;
    Vector3 tssStartPos;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        missionText.text = mission[0] + noCompleteCount + cableQtyRequire[0];
        tssStartPos = TSSCable.transform.position;
        distanceChecker.transform.position = TSSCable.transform.position;


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
        if (missionComplete <2)
        {
            oriDistance = (TSSCable.transform.position.x - distanceChecker.transform.position.x);
            gameDistance = oriDistance * 100;

            if (gameDistance >= 1839f && gameDistance < 1843f)
            {
                gameDistance = 1840f;
                lengthAnimator.SetInteger("correct", 1);
            }

            else if (gameDistance >= 2839f && gameDistance < 2844f)
            {
                gameDistance = 2840f;
                lengthAnimator.SetInteger("correct", 1);
            }

            else
            {
                lengthAnimator.SetInteger("correct", 0);
            }

            lengthText.text = "Length: " + gameDistance.ToString("F0") + "mm";
        }

        else
        {
            lengthText.text = "";
        }
    }

    public void OnClickCableCutter(bool isClick)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastLength))
        {
            Debug.DrawRay(transform.position, Vector3.down * raycastLength, Color.yellow);
            if (hit.collider.gameObject.GetComponent<CableCutScene>())
            {
                currentPoint = hit.transform.position.x;

            }
        }

        touchPointScale = -tssStartPos.x + currentPoint;
        touchPointScale /= 2;
        touchPointScale /= deviderScaleTSS;

        isClick = false;
    }

    public void CableCutterCollide(bool isCollide)
    {
        if (isCollide)
        {
            this.isCollide = isCollide;
            ScaleAndMoveCable(frontCableTSS, backCableTSS);

            switch (missionNo)
            {
                case 1:
                    FirstMission1840();
                    break;

                case 2:
                    SecondMission2840();
                    break;

                default:
                    break;

            }
            this.isCollide = false;
        }
    }

    private void ScaleAndMoveCable(GameObject front, GameObject back)
    {
        move = front.transform.localScale.y - touchPointScale;
        float opositeScale = move + 0.5f;

        front.transform.localScale = new Vector3(front.transform.localScale.x, touchPointScale, front.transform.localScale.z);
        front.transform.localPosition = new Vector3(front.transform.localPosition.x, front.transform.localPosition.y + move, front.transform.localPosition.z);
        back.transform.localScale = new Vector3(1, opositeScale, 1);
        back.transform.localPosition = new Vector3(back.transform.localPosition.x, back.transform.localPosition.y + move, back.transform.localPosition.z);

    }

    private void FirstMission1840()
    {
        if (gameDistance == 1840f && isCollide)
        {
            noCompleteCount++;
            SoundManager.PlayCorrectClip();
            StartCoroutine(MissionDone());
        }

        else
        {
            errorText.text = textError[missionComplete];
            Error();
        }

    }

    private void SecondMission2840()
    {

        if (gameDistance == 2840f && isCollide)
        {
            noCompleteCount++;
            SoundManager.PlayCorrectClip();
            StartCoroutine(MissionDone());
        }

        else
        {
            errorText.text = textError[missionComplete];
            Error();
        }

    }

    public void ThirdMission75()
    {
        int count = 0;
        int[] intAttributes = new int [attributes.Length];

        for (int i = 0; i < attributes.Length; i++)
        {
            int.TryParse(attributes[i].text, out intAttributes[i]);
        }

        if (insertCable)
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                if (intAttributes[i] >= attributeCompleteStartIndex[i] && intAttributes[i] <= attributeCompleteEndIndex[i])                {
                    count++;
                    SoundManager.PlayCorrectClip();
                }

                else
                {
                    errorText.text = textError[missionComplete + 1];
                    Error();
                    break;
                }
            }
        }

        else
        {
            errorText.text = textError[missionComplete];
            Error();
        }

        if (count == attributes.Length)
        {
            int.TryParse(attributes[7].text, out int temp);
            UIManager.CloseBoardPanel();
            playTime = 0;
            StartCoroutine(MachineCut(temp));

        }

    }

    IEnumerator MachineCut(int n)
    {
        while (playTime < n)
        {
            GRCable.GetComponent<Animator>().SetTrigger("cut");
            yield return new WaitForSeconds(1.1f);
            noCompleteCount++;
            missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
            ++playTime;
        }

        if (noCompleteCount == attributeCompleteStartIndex[8])
        {
            StartCoroutine(MissionDone());
        }

        else if (noCompleteCount >= attributeCompleteStartIndex[8])
        {
            noCompleteCount = 0;
            errorText.text = textError[missionComplete + 2];
            Error();
        }

    }

    IEnumerator MissionDone()
    {
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        yield return new WaitForSeconds(1f);
        noCompleteCount = 0;
        missionComplete++;
        missionNo++;
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        if (missionComplete == 3)
        {
            UIManager.SetError(errorCountText.text);
            UIManager.SetTime(timeText.text);
            UIManager.StartCompletePanel();
        }
    }

    public void Error()
    {
        SoundManager.PlayErroClip();
        errorCount++;
        errorCountText.text = errorCount.ToString();
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        StartCoroutine(UIManager.Error());

    }

    public void ChangeCutterMachine()
    {
        SoundManager.PlayClickClip();
        machinePanel.SetActive(true);
        cameraAnimator.SetBool("machine", true);
        UIManager.ChangeMachine();
    }

    public void ChangeManual()
    {
        SoundManager.PlayClickClip();
        machinePanel.SetActive(false);
        cameraAnimator.SetBool("machine", false);
        UIManager.ChangeManual();
    }

    public void InsertGRCable()
    {
        insertCable = true;
        SoundManager.PlayClickClip();
        GRCable.GetComponent<Animator>().SetTrigger("insert");
    }



    

    int temp = 0;

    public void SelectAttribute()
    {
        SoundManager.PlayClickClip();
        for (int i = 0; i < settingButtons.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == settingButtons[i].name)
            {
                attributeIndex = i;
                arrowDown[i].SetActive(true);
                attributes[attributeIndex].text = "-";

                if (!setButton)
                {
                    attributes[temp].text = "0";
                    arrowDown[temp].SetActive(false);
                    temp = i;
                }

                else
                {
                    temp = i;
                    setButton = false;
                }

                break;
            }
        }
    }

    public void KeyInAttribute()
    {
        SoundManager.PlayClickClip();
        for (int i = 0; i < numpadButtons.Length; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name == numpadButtons[i].name)
            {
                if (i >= 0 && i <= 9)
                {
                    if (attributes[attributeIndex].text.Length <= 2)
                    {
                        if (attributes[attributeIndex].text == "-" || attributes[attributeIndex].text == "0")
                        {
                            attributes[attributeIndex].text = "";
                        }
                        attributes[attributeIndex].text += i.ToString();
                    }
                }

                else if (i == 10)
                {
                    attributes[attributeIndex].text = "0";
                }

                else if (i == 11)
                {
                    arrowDown[temp].SetActive(false);
                    if (attributes[attributeIndex].text == "-")
                    {
                        attributes[attributeIndex].text = "0";
                    }
                    setButton = true;

                }

                break;
            }

        }
    }


}
