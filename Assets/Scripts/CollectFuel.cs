using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFuel : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("1");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("12");
            DriveCar car = other.gameObject.GetComponentInParent<DriveCar>();
            if (car != null)
            {
                Debug.Log("123");
                car.FillFuel();
                Destroy(gameObject);
            }
        }
    }
}

