using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutShrinkTube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            TutGameManagerCrimp.instance.isOnTube = true;
            collision.gameObject.GetComponentInParent<ObjectMovement>().DeactiveMove();
            TutorialManagerCrimp.instance.tut1.SetTrigger("crimp");
            TutorialManagerCrimp.instance.actionButton[0].interactable = true;

        }
    }

}
