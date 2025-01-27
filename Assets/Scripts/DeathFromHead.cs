using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFromHead : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Ground")){
            Debug.Log("Player died");
            GameManager.instance.GameOver();
        }
    }
}
