using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GameObject startingPoint;

    public GameObject playerProjectile;

    public GameObject notEnoughCurrencyText;

    public TMP_Text healthAmountText;

    public Slider healthBar;

    public int health;

    private CharacterController playerController;

    private BarrierPaymentSystem barrier;

    public ParticleSystem deathEffect;

    private PauseManager pauseManager;

    [Header("Variables")]
    [HideInInspector] public int playerCurrency;

    [HideInInspector] public int secretsCollected = 0;

    [SerializeField] private float currentMoveSpeed = 5f;
    [SerializeField] private float originalMoveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 moveDirection;
    private bool isGrounded;
    private bool isRunning = false;

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
    }

    private void Update()
    {
        Movement();

        if (transform.position.y <= -10)
        {
            Die();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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
        healthBar.value = health;
        healthAmountText.text = health.ToString();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //StopAllCoroutines();

        //StartCoroutine(DelayedReset());

        SceneManager.LoadScene("Level");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Projectile"))
        {
            Debug.Log("Damaged");
            Destroy(other.gameObject);
            TakeDamage(1);
        }

        if (other.gameObject.CompareTag("Hazard"))
        {
            SceneManager.LoadScene("Level");
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


    //private IEnumerator DelayedReset()
    //{
    //    yield return new WaitForSeconds(2f);

    //    SceneManager.LoadScene("Level");
    //}
}