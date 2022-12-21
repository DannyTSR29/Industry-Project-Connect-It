using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutGameManagerCrimp : MonoBehaviour
{
    public static TutGameManagerCrimp instance;

    [SerializeField] private GameObject wholeCable;
    [SerializeField] private Animator crimpMachine;
    [SerializeField] private Button buttonUp;
    [SerializeField] private Button buttonDown;
    [SerializeField] private Text missionText;
    [SerializeField] private Text errorText;
    [SerializeField] private Text errorCountText;
    [SerializeField] private Text timeText;
    public GameObject[] shrinkTube = new GameObject[5];
    public bool[] isCrimp = new bool[5];
    public bool isOnTube = false;
    private bool rotateCable = false;

    private string[] mission = new string[] { "Crimp 5 terminal for TSS-802-D02 cable ", "Congrate!\nYou Unclock Next Level!" };
    private string[] cableQtyRequire = new string[] { "/5", "" };
    private string[] textError = new string[] { "Please touch the terminal!", "This cable is terminal! Please change other cable!" };

    private int errorCount = 0;
    public int missionComplete = 0;
    private int noCompleteCount = 0;
    public int currentCableIndex = 2;
    private float timer = 0;
    [Range(0, 3)] public float speed = 0.01f;

    Vector3 oriPosition;
    public Quaternion[] rotateAngle = new Quaternion[5];

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        missionText.text = mission[0] + noCompleteCount + cableQtyRequire[0];
        currentCableIndex = 2;
        oriPosition = wholeCable.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateCable)
        {
            wholeCable.transform.rotation = Quaternion.Lerp(wholeCable.transform.rotation, rotateAngle[currentCableIndex], Time.deltaTime * speed);
            if (wholeCable.transform.rotation == rotateAngle[currentCableIndex])
            {
                rotateCable = false;
            }
        }
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        timeText.text = timer.ToString("F0");
    }

    public void MissionCrimp()
    {
        TutorialManagerCrimp.instance.actionButton[0].interactable = false;
        crimpMachine.SetTrigger("crimp");
        if (isOnTube && !isCrimp[currentCableIndex])
        {
            noCompleteCount++;
            isCrimp[currentCableIndex] = true;
            shrinkTube[currentCableIndex].SetActive(true);
            SoundManager.PlayCorrectClip();
            StartCoroutine(MissionDone());

        }

        else if (isOnTube && isCrimp[currentCableIndex])
        {
            errorText.text = textError[1];
            Error();
        }

        else if (!isOnTube)
        {
            errorText.text = textError[0];
            Error();
        }

    }

    IEnumerator MissionDone()
    {
        missionText.text = mission[missionComplete] + noCompleteCount + cableQtyRequire[missionComplete];
        yield return new WaitForSeconds(1f);
        isOnTube = false;
        ObjectMovement.instance.ActiveMove();
        wholeCable.transform.position = oriPosition;
        if (noCompleteCount == 5)
        {
            TutorialManagerCrimp.instance.panel.SetActive(false);
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

    public void RotateCableUp()
    {
        currentCableIndex++;
        rotateCable = true;

        if (currentCableIndex == 4)
        {
            buttonUp.interactable = false;
        }

        else if (currentCableIndex != 0)
        {
            buttonDown.interactable = true;
        }
    }

    public void RotateCableDown()
    {
        currentCableIndex--;
        rotateCable = true;

        if (currentCableIndex == 0)
        {
            buttonDown.interactable = false;
        }

        else if (currentCableIndex != 4)
        {
            buttonUp.interactable = true;
        }
    }


}


