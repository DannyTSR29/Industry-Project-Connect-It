using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public static ObjectMovement instance;

    public enum ObjectMovmentType
    {
        Individual,
        Parenting,
        Limited
    };

    public ObjectMovmentType currentObjType;
    //Movement
    private Vector3 targetMoveVector;
    private bool isMouseDrag = false;
    private bool isActive = true;

    [SerializeField] private Vector3 limitedVector = Vector3.zero;

    public bool IsMouseDrag { get => isMouseDrag; set => isMouseDrag = value; }

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        targetMoveVector = transform.position;
    }

    public void setObjectMovement(Vector3 vec)
    {
        if (isActive && isMouseDrag)
        {
            if(currentObjType == ObjectMovmentType.Individual)
            {
                transform.position += vec;
            }
            else if(currentObjType == ObjectMovmentType.Limited)
            {
                //Set limit Vec
                transform.position += (Vector3.Scale(vec,limitedVector));
            }
        }
    }

    public void SetObjMoveToParent()
    {
        //collider.enabled = false;
        DeactiveMove();
        currentObjType = ObjectMovmentType.Parenting;
    }

    public void DeactiveMove()
    {
        isActive = false;
        IsMouseDrag = false;
    }

    public void ActiveMove()
    {
        isActive = true;
        IsMouseDrag = true;
    }



}
