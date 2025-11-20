using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject startingPoint;

    public Slider healthBar;

    public int health;

    private CharacterController playerController;

    [HideInInspector] public int playerCurrency;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 currentPosition;
    private Vector3 moveDirection;
    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController = GetComponent<CharacterController>();
        currentPosition = startingPoint.transform.position;
        healthBar.value = health;
    }

    private void Update()
    {
        Movement();

        if (currentPosition.y <= 10)
        {
            currentPosition = startingPoint.transform.position;
        }
    }

    void Movement()
    {
        isGrounded = playerController.isGrounded;
        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput;

        playerController.Move(movementDirection * moveSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        moveDirection.y += gravity * Time.deltaTime;
        playerController.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Level");
    }

    private void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
    TakeDamage(1);
}
    }
}