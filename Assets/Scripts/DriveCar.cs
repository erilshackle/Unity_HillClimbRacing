using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveCar : MonoBehaviour
{
    public Rigidbody2D tireFront, tireRear, carRb;
    public float speed = 150f;
    public float rotationSpeed = 300f;
    public float inputMove;

    public float fuel = 1f;
    public float fuelSpeed = 0.1f;
    public Image uiFuel;
    public Gradient gradientFuel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(fuel <= 0f){
            Debug.Log("Fuel is empty");
            tireFront.velocity = Vector2.zero;
            tireRear.velocity = Vector2.zero;
            if(carRb.velocity.sqrMagnitude < 2f){
                GameManager.instance.GameOver();    
            }
            return;        
        }

        fuelConsuming();
        updateUI();
        GetInuts();
    }

    private void FixedUpdate()
    {
        if (fuel > 0)
        {
            MoveLogic();

        }
        
    }

    void GetInuts()
    {
        inputMove = Input.GetAxis("Horizontal");
    }

    void updateUI()
    {
        uiFuel.fillAmount = fuel;
        uiFuel.color = gradientFuel.Evaluate(uiFuel.fillAmount);
    }

    void MoveLogic()
    {
        tireFront.AddTorque(-inputMove * speed * Time.fixedDeltaTime);
        tireRear.AddTorque(-inputMove * speed * Time.fixedDeltaTime);
        carRb.AddTorque(rotationSpeed * inputMove * Time.fixedDeltaTime);
    }

    void fuelConsuming()
    {
        float move = inputMove < 0f ? 0.5f * inputMove : inputMove;
        fuel -= fuelSpeed * Mathf.Abs(move) * Time.fixedDeltaTime;
    }

    public void FillFuel(){
        fuel = 1f;
    }

    public float getComsumedFuel(){
        return fuel;
    }

}
