
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    public Camera camera;
    public Vector3 LocalVelocity;


    float rotX, rotY = 0;

    private Rigidbody rb;
    public float Speed;
    
    private Vector3 _oldPosition;
    private Vector3 oldRotate;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LocalVelocity = transform.InverseTransformDirection(rb.velocity);
        LocalVelocity.y = 0;
        Screen.lockCursor = true;
    }

    void Awake()
    {
    }
    

    void FixedUpdate()
    {

        transform.eulerAngles = new Vector3(0, rotX, 0);
        //camera.transform.eulerAngles = new Vector3(-rotY, rotX, 0);

        if (Input.GetAxis("Horizontal") > 0)
            LocalVelocity.x = -Speed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") < 0)
            LocalVelocity.x = Speed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") == 0)
            LocalVelocity.x = 0;
        if (Input.GetAxis("Vertical") > 0)
            LocalVelocity.z = -Speed * Time.deltaTime;
        if (Input.GetAxis("Vertical") < 0)
            LocalVelocity.z = Speed * Time.deltaTime;
        if (Input.GetAxis("Vertical") == 0)
            LocalVelocity.z = 0;

        transform.Translate(LocalVelocity);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 1.5f))
                if (hit.transform.tag != "Player")
                {
                    transform.GetComponent<Rigidbody>().AddForce(new Vector3(0f,2f,0f)*2.5f, ForceMode.Impulse);
                }
        }
    }
    
    private void Update()
    {
        rotX += Input.GetAxis("Mouse X");
        rotY += Input.GetAxis("Mouse Y");
        rotY = Mathf.Clamp(rotY, -50, 30);
    }
}
