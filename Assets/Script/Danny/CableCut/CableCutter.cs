using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCutter : MonoBehaviour
{
    Rigidbody rigidbody;

    [SerializeField] private Animator cutterAnimator;
    [SerializeField] private float force = 0;
    [SerializeField] private float maximumY = 0;
    [SerializeField] private bool isMoveUp = false;




    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoveUp)
        {
            cutterAnimator.SetTrigger("rotate90");
            rigidbody.AddForce(Vector3.up * force);
            if (transform.position.y >= maximumY)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.Sleep();
                isMoveUp = false;
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        ICable cable = collision.gameObject.GetComponent<ICable>();
        if (cable != null)
        {
            cutterAnimator.SetTrigger("cut");
            GameManagerGameCut.instance.CableCutterCollide(true);
            cable.CableWork();
            isMoveUp = true;
        }
    }
   

    public void OnCutClick()
    {
        if (!isMoveUp)
        {
            cutterAnimator.SetTrigger("rotate0");
            GameManagerGameCut.instance.OnClickCableCutter(true);
            rigidbody.AddForce(Vector3.down * force);
        }

    }

 
}
