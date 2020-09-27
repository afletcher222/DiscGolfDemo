using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    public bool inWindZone = false;
    public GameObject windZone;
    public Vector3 originalForce = new Vector3(0, 0.4f, 1);
    public float speed = 3000;
    public bool madeContact;
    public float curve = 0;
    public float turn = 2;
    public float fade = 2;
    public bool turnFinished;
    public bool glideFinished;
    public bool glideOnce;
    public float turnSpeed;
    public float turningTime = 0.1f;
    public Vector3 turning = new Vector3(2, 0, 0);
    public Quaternion locTurning;
    public float localTurningOfDisc = 70;
    public float glide = 6;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        curve = 0;
        rb = GetComponent<Rigidbody>();
        madeContact = true;
        turnFinished = false;
        glideOnce = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inWindZone)
        {
            rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "windArea")
        {
            windZone = coll.gameObject;
            inWindZone = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.tag == "windArea")
        {
            inWindZone = false;
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "ground")
        {
            madeContact = true;
            print("hit ground");
        }
    }

    public void OnButtonPress()
    {
        madeContact = false;
        rb.useGravity = true;
        rb.AddForce(originalForce * speed);
        StartCoroutine(Turning());
        print("threw");
    }

    public IEnumerator Turning()
    {
        while (!madeContact)
        {
            if (curve < (turn * 4f) && !turnFinished)
            {
                curve += (0.1f * turn);
                turnSpeed = curve + (speed * .005f);
                locTurning.z -= Time.deltaTime * localTurningOfDisc;
                transform.rotation = Quaternion.Euler(locTurning.x, locTurning.y, locTurning.z);
                if (curve >= turn)
                {
                    turnFinished = true;
                    rb.useGravity = false;
                    StartCoroutine(GlideTime());
                }
            }
            else if (curve > (fade * -10f) && turnFinished && glideFinished)
            {
                curve -= (0.1f * fade);
                turnSpeed = -curve - (speed * .005f);
                locTurning.z += Time.deltaTime * localTurningOfDisc;
                transform.rotation = Quaternion.Euler(locTurning.x, locTurning.y, locTurning.z);
            }

            rb.AddForce(turning * turnSpeed);
            yield return new WaitForSeconds(turningTime);
        }
    }

    IEnumerator GlideTime()
    {
        yield return new WaitForSeconds(glide * 0.2f);
        rb.useGravity = true;
        glideFinished = true;
    }
}
