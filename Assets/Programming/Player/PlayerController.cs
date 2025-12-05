using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Camera followCamera;

    public GameObject startingPoint;
    public GameObject playerProjectile;
    public GameObject notEnoughCurrencyText;

    public Slider healthBar;

    public TMP_Text healthAmountText;
    public TMP_Text moveFasterText;
    public TMP_Text jumpHigherText;

    private CharacterController playerController;

    private BarrierPaymentSystem barrier;

    private PauseManager pauseManager;

    [Header("Variables")]

    public int secretsCollected = 0;

    public int playerCurrency;

    public int health;

    private float healTimer = 0;

    public float currentMoveSpeed = 5f;
    public float originalMoveSpeed = 5f;
    public float jumpHeight = 1.0f;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 moveDirection;
    private bool isGrounded;
    private bool isRunning = false;

    [Header("Respawning")]
    public Vector3 placeToRespawn;

    public Transform respawnPoint;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController = GetComponent<CharacterController>();
        barrier = FindAnyObjectByType<BarrierPaymentSystem>();
        pauseManager = FindFirstObjectByType<PauseManager>();

        healthBar.value = health;
        healthAmountText.text = health.ToString();
        notEnoughCurrencyText.SetActive(false);

        placeToRespawn = respawnPoint.position;
    }

    private void Update()
    {
        Movement();

        if (transform.position.y <= -10 || health <= 0)
        {
            Die();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (health < 5)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= 5)
            {
                health += 1;
                healTimer = 0;

                healthBar.value = health;
                healthAmountText.text = health.ToString();
            }
        }
    }

    void Movement()
    {
        isGrounded = playerController.isGrounded;

        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0f;
        }

        bool pressingShift = Input.GetKey(KeyCode.LeftShift);
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput;

        playerController.Move(movementDirection.normalized * currentMoveSpeed * Time.deltaTime);

        if (pressingShift && !isRunning)
        {
            currentMoveSpeed *= 1.5f;
            isRunning = true;
        }
        else if (!pressingShift)
        {
            currentMoveSpeed = originalMoveSpeed;
            isRunning = false;
        }

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        if (!isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        playerController.Move(moveDirection * Time.deltaTime);
    }

    private void Attack()
    {
        if (!pauseManager.isPaused)
        {
            Rigidbody rb = Instantiate(playerProjectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

            Destroy(rb.gameObject, 3);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healTimer = 0;
        healthBar.value = health;
        healthAmountText.text = health.ToString();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerController.enabled = false;
        transform.position = placeToRespawn;
        playerController.enabled = true;

        health = 5;

        healthBar.value = health;

        healthAmountText.text = health.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Projectile"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
        }

        if (other.gameObject.CompareTag("Hazard"))
        {
            SceneManager.LoadScene("Level");
        }

        if (other.gameObject.CompareTag("Explosion"))
        {
            TakeDamage(1);

            if (health <= 0)
            {
                Die();
            }
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            placeToRespawn = other.gameObject.transform.position;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Power-Up"))
        {
            StopAllCoroutines();

            if (other.GetComponent<PowerUpController>().isSpeedBoost)
            {
                StartCoroutine(DisplayMoveFasterText());
            }
            else if (other.GetComponent<PowerUpController>().isJumpBoost)
            {
                StartCoroutine(DisplayJumpHigherText());
            }
        }

        if (other.gameObject.name == "Barrier Trigger" && playerCurrency >= barrier.amountToPay)
        {
            Debug.Log("Collision");
            playerCurrency -= barrier.amountToPay;
            Destroy(barrier.gameObject);
        }
        else if (other.gameObject.name == "Barrier Trigger" && playerCurrency < barrier.amountToPay)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayNotEnoughText());
        }
    }

    private IEnumerator DisplayNotEnoughText()
    {
        notEnoughCurrencyText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        notEnoughCurrencyText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayMoveFasterText()
    {
        jumpHigherText.gameObject.SetActive(false);

        moveFasterText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        moveFasterText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayJumpHigherText()
    {
        moveFasterText.gameObject.SetActive(false);

        jumpHigherText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        jumpHigherText.gameObject.SetActive(false);
    }

    //private IEnumerator DelayedReset()
    //{
    //    yield return new WaitForSeconds(2f);

    //    SceneManager.LoadScene("Level");
    //}
}