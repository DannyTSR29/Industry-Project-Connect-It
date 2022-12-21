using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDragComponent : MonoBehaviour
{
    public static NewDragComponent instance { get; private set; }
    enum InputType { touchInput, mouseInput };
    [SerializeField] private InputType currentInput;

    delegate void RayDelegate();
    RayDelegate rayDelegate;

    private ObjectMovement currentMoveObj;
    private Camera cam;
    [SerializeField] private LayerMask selectedLayer;

    private Vector3 dis;
    private float posX;
    private float posY;
    private float posZ;

    private bool touched = false;
    private bool dragging = false;
    public bool allowToShotRay = true;

    private Vector3 dragTargetPos;
    private Vector3 previousPosition;
    private Vector3 moveVector;

    void Awake()
    {
        //Create Var ,GetComponent
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        //if (onClickUnityEvent == null)
        //    onClickUnityEvent = new ClickMagnetEventClass();
        //if (onReleaseUnityEvent == null)
        //    onReleaseUnityEvent = new ClickMagnetEventClass();
    }

    private void Start()
    {
        //Get Value, Initiallze Value
        cam = Camera.main;
        CheckRayDetection();

    }
    void Update()
    {
        rayDelegate();
    }

    void CheckRayDetection()
    {
        if (currentInput == InputType.touchInput && allowToShotRay)
            rayDelegate = TouchControl;
        else if (currentInput == InputType.mouseInput && allowToShotRay)
            rayDelegate = MouseControl;
        else
        {
            dragTargetPos = Vector3.zero;
            releaseInput();
            rayDelegate = StopControl;
        }
    }
    void TouchControl()
    {
        if (Input.touchCount != 1)
        {
            dragging = false;
            touched = false;
            return;
        }

        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit,selectedLayer))
            {
                InitializeMove(hit.collider.gameObject, Input.GetTouch(0).position);
            }
        }

        if (touched && touch.phase == TouchPhase.Moved)
        {
            DragMove(currentMoveObj, Input.GetTouch(0).position);
        }

        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            releaseInput();
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(pos);
            if (Physics.Raycast(ray, out hit, selectedLayer))
            {
                InitializeMove(hit.collider.gameObject, pos);
            }
        }

        if (touched)
        {
            DragMove(currentMoveObj, Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && touched)
        {
            releaseInput();
        }
    }

    void StopControl()
    {

    }

    void InitializeMove(GameObject hitObj, Vector3 inputPos)
    {
        if(hitObj.transform.parent == null)
        {
            //Debug.Log("Use Self Object Movement");
            currentMoveObj = hitObj.GetComponent<ObjectMovement>();
        }
        else
        {
            currentMoveObj = hitObj.transform.root.GetComponent<ObjectMovement>();
            //Debug.Log("Use Parent Object Movement");
        }

        if (currentMoveObj != null)
        {
            //Debug.Log("Current Move Object is not null");
            dragTargetPos = hitObj.transform.position;
            previousPosition = hitObj.transform.position;

            dis = cam.WorldToScreenPoint(previousPosition);
            posX = inputPos.x - dis.x;
            posY = inputPos.y - dis.y;

            touched = true;
            currentMoveObj.IsMouseDrag = true;

            //If Draging the whole Group, dont do the Index Check
            //if(currentMoveObj.currentObjType == ObjectMovement.ObjectMovmentType.Individual && currentMoveObj.IsMagneticFunction)
            //{
            //    int sourceIndex = RetriveMagnetSourceIndex(currentMoveObj.gameObject);
            //    //UnityEventManager.instance.onClickUnityEvent?.Invoke(sourceIndex, currentMoveObj.gameObject.GetInstanceID());
            //}
        }
    }

    void DragMove(ObjectMovement moveObj, Vector3 inputPos)
    {
        dragging = true;

        //New Mosue Position
        float posXNow = inputPos.x - posX;
        float posYNow = inputPos.y - posY;
        //float posZNow = inputPos.z - posz
        Vector3 curPos = new Vector3(posXNow, posYNow, dis.z);
        Vector3 worldPos = cam.ScreenToWorldPoint(curPos);
        worldPos.z = dragTargetPos.z;

        //Old Pos - new pos;
        moveVector = worldPos - previousPosition;
        Debug.DrawLine(Camera.main.transform.position, worldPos, Color.red, 1.0f);
        dragTargetPos = worldPos;
        moveObj.setObjectMovement(moveVector);

        previousPosition = dragTargetPos;
    }

    void releaseInput()
    {
        dragging = false;
        touched = false;
        previousPosition = Vector3.zero;
        currentMoveObj.IsMouseDrag = false;

        //if (currentMoveObj.currentObjType == ObjectMovement.ObjectMovmentType.Individual && currentMoveObj.IsMagneticFunction)
        //{
        //    //int sourceIndex = RetriveMagnetSourceIndex(currentMoveObj.gameObject);
        //    //UnityEventManager.instance.onReleaseUnityEvent?.Invoke(sourceIndex,currentMoveObj.gameObject.GetInstanceID());
        //}
    }

    //int RetriveMagnetSourceIndex(GameObject targetObj)
    //{
    //    var itemRecorder = targetObj.GetComponent<ItemRecorder>();
    //    if(itemRecorder == null)
    //    {
    //        Debug.Log("Unable to Get Item Recorder while Get Magnet Index");
    //        return 999;
    //    }
    //    else
    //        return itemRecorder.magnetIndex;
    //}
}
