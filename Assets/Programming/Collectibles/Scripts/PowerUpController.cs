using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    private PlayerController player;

    public ParticleSystem collectEffect;

    private GameObject speedIcon;

    private GameObject jumpIcon;

    public int rotationSpeed;

    [Header("What Power-Up Is It?")]

    public bool isSpeedBoost;
    public bool isJumpBoost;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        speedIcon = transform.GetChild(0).gameObject;
        jumpIcon = transform.GetChild(1).gameObject;

        if (isSpeedBoost)
        {
            speedIcon.SetActive(true);
        }
        else if (isJumpBoost)
        {
            jumpIcon.SetActive(true);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);

            if (isSpeedBoost)
            {
                player.currentMoveSpeed += 3;
                player.originalMoveSpeed = player.currentMoveSpeed;
            }
            else if (isJumpBoost)
            {
                player.jumpHeight += 1f;
            }

            Destroy(this.gameObject);
        }
    }
}

//    private IEnumerator DisplayMoveFasterText()
//    {
//        moveFasterText.gameObject.SetActive(true);

//        yield return new WaitForSeconds(1.5f);

//        moveFasterText.gameObject.SetActive(false);
//    }

//    private IEnumerator DisplayJumpHigherText()
//    {
//        jumpHigherText.gameObject.SetActive(true);

//        yield return new WaitForSeconds(1.5f);

//        jumpHigherText.gameObject.SetActive(false);
//    }
//}
