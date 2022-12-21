using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerCrimp : MonoBehaviour
{
    public static TutorialManagerCrimp instance;
    public GameObject panel;
    public Animator tut1;
    public Button[] actionButton;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < actionButton.Length; i++)
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

    }

    public void Tut1()
    {
        tut1.SetInteger("tut1", tut1.GetInteger("tut1") + 1);
        if (tut1.GetInteger("tut1") == 1 || tut1.GetInteger("tut1") == 3)
        {
            actionButton[1].interactable = true;
        }

        else
        {
            actionButton[1].interactable = false;
        }

        if (tut1.GetInteger("tut1") == 5 || tut1.GetInteger("tut1") == 6 || tut1.GetInteger("tut1") == 7 || tut1.GetInteger("tut1") == 9)
        {
            actionButton[2].interactable = true;
        }

        else
        {
            actionButton[2].interactable = false;
        }
    }



}
