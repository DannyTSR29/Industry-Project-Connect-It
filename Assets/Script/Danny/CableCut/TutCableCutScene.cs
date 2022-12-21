using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutCableCutScene : MonoBehaviour, ICable
{
    [SerializeField] private GameObject frontCable;
    [SerializeField] private GameObject backCable;
    //[SerializeField] private Material blackT;
    Vector3 tempPos;
    Vector3 cableStartPosition;
    Vector3 frontCableStartPosition;
    Vector3 backCableStartPosition;
    Vector3 frontCableStartScale;
    Vector3 backCableStartScale;

    Color oldColor;
    Color currentColor;
    MeshRenderer meshFrontCable;
    MeshRenderer meshBackCable;

    // Start is called before the first frame update
    void Start()
    {
        cableStartPosition = transform.position;
        tempPos = transform.position;
        frontCableStartPosition = frontCable.transform.localPosition;
        backCableStartPosition = backCable.transform.localPosition;
        frontCableStartScale = frontCable.transform.localScale;
        backCableStartScale = backCable.transform.localScale;

        meshFrontCable = frontCable.gameObject.GetComponent<MeshRenderer>();
        meshBackCable = backCable.gameObject.GetComponent<MeshRenderer>();

        oldColor = meshFrontCable.material.color;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        CableMoveCheck();
    }

    public void CableMoveCheck()
    {
        if (tempPos != transform.position)
        {
            TutGameCutManager.instance.LengthShow();
            tempPos = transform.position;
        }
    }


    public void CableWork()
    {
        StartCoroutine(Fade());
        currentColor = oldColor;
    }

    IEnumerator Fade()
    {
        //currentColor = blackT.color;
        //meshFrontCable.material = blackT;
        //meshBackCable.material = blackT;
        frontCable.SetActive(true);
        backCable.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        for (float i = 1.0f; i >= -0.05; i -= 0.05f)
        {
            currentColor.a = i;

            meshFrontCable.material.color = currentColor;
            meshBackCable.material.color = currentColor;
            yield return new WaitForSeconds(0.05f);
        }

        frontCable.transform.localScale = frontCableStartScale;
        backCable.transform.localScale = backCableStartScale;
        frontCable.transform.localPosition = frontCableStartPosition;
        backCable.transform.localPosition = backCableStartPosition;

        meshFrontCable.material.color = oldColor;
        meshBackCable.material.color = oldColor;

        frontCable.SetActive(false);
        backCable.SetActive(false);

        transform.position = cableStartPosition;

        gameObject.GetComponent<MeshRenderer>().enabled = true;

    }



}
