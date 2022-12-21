using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutStripManager : MonoBehaviour
{
    public static TutStripManager instance;
    public GameObject[] panel;
    public Animator tut1;
    public Animator tut2;
    public Button[] actionButton;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < actionButton.Length; i++)
        {
            actionButton[i].interactable = false;
        }
        tut2.enabled = false;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (TutGameManagerStrip.instance.missionComplete == 1)
        {
            panel[1].SetActive(true);
            tut2.enabled = true;
        }

        else if (TutGameManagerStrip.instance.missionComplete == 2)
        {
            panel[1].SetActive(false);
        }
    }

    public void Tut1()
    {
        tut1.SetInteger("tut1", tut1.GetInteger("tut1") + 1);
    }

    public void Tut2()
    {
        actionButton[1].interactable = false;
        tut2.SetInteger("tut2", tut2.GetInteger("tut2") + 1);

    }
}
