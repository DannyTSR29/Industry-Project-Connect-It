using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class TutManager : MonoBehaviour
{
    public static TutManager instance;
    public GameObject[] panel;
    public Animator[] tut1;
    public Animator[] tut2;
    public Button[] actionButton;
    public bool action = false;

    // Start is called before the first frame update
    void Start()
    {
        tut1[1].enabled = false;
        StartCoroutine(Wait());

        for (int i = 1; i < actionButton.Length; i++)
        {
            actionButton[i].interactable = false;
        }

    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (TutGameCutManager.instance.missionNo == 2)
        {
            if (action && TutGameCutManager.instance.gameDistance == 300f)
            {
                tut1[0].SetInteger("tut1", 2);
            }

            else if (action && TutGameCutManager.instance.gameDistance != 300f)
            {
                tut1[0].SetInteger("tut1", 1);
            }


        }

        if (TutGameCutManager.instance.missionNo == 3)
        {
            panel[0].SetActive(false);
            panel[1].SetActive(true);
            actionButton[1].interactable = true;
        }
    }

    public void Tut1()
    {
        tut1[1].enabled = false;
        tut1[0].SetInteger("tut1", 1);
        //tut1[1].gameObject.transform.position = TutGameCutManager.instance.tssStartPos;
        action = true;
    }

    public void Tut2()
    {
        tut2[0].SetInteger("tut2", tut2[0].GetInteger("tut2") + 1);
        for (int i = 0; i < actionButton.Length; i++)
        {
            actionButton[i].interactable = false;
        }

        if (tut2[0].GetInteger("tut2") <= 11)
        {
            actionButton[tut2[0].GetInteger("tut2") + 1].interactable = true;

        }

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.3f);
        tut1[1].enabled = true;
    }

}
