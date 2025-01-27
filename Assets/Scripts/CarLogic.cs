using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLogic : MonoBehaviour
{
    public float speed;
    public float speedCar;
    public float inputMove;
    public Rigidbody2D tireFront, tireRear, carRb;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInut();
    }

    private void FixedUpdate() {
        MoveLogic();
    }

    void GetInut()
    {
        inputMove = Input.GetAxis("Horizontal");
    }

    void MoveLogic()
    {
        tireFront.AddTorque(-inputMove * speed * Time.fixedDeltaTime);
        tireRear.AddTorque(-inputMove * speed * Time.fixedDeltaTime);
        carRb.AddTorque(-inputMove * speedCar * Time.fixedDeltaTime);
    }

}
