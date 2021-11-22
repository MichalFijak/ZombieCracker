using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public CharacterController controller;
    public float playerHealth = 100;
    public float speed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = -0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    // Update is called once per frame

    private void Start()
    {
        healthText.text = "Hp: " + playerHealth;
    }

    void Update()
    {
       
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded&&velocity.y<0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        
        controller.Move(move*speed*Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump")&&isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight -  gravity);
        }
        
    }
    public void PlayerHealth(float health)
    {
        playerHealth -= health;
        healthText.text = "Hp:" + playerHealth;
        if (playerHealth<=0)
        {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerHealth(25);
        }
        
    }
}
