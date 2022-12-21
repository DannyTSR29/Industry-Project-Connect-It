using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutCableStrip : MonoBehaviour
{
    Vector3 tempPos;
    [SerializeField] Button rotateButton;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        tempPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CableMoveCheck();
        Vector3 V = Camera.main.WorldToScreenPoint(transform.position);
        rotateButton.transform.position = new Vector3(V.x, V.y + 70f, V.z);
    }

    public void CableMoveCheck()
    {
        if (tempPos != transform.position)
        {
            TutGameManagerStrip.instance.LengthShow();
            tempPos = transform.position;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interactable" && TutGameManagerStrip.instance.isTSS)
        {
            TutGameManagerStrip.instance.isCollide = true;
            ObjectMovement.instance.DeactiveMove();
            rb.isKinematic = true;
            TutStripManager.instance.tut1.SetInteger("tut1", 1);
            TutStripManager.instance.actionButton[2].interactable = true;

        }
    }







}

