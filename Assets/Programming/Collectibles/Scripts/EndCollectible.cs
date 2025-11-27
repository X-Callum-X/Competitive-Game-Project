using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndCollectible : MonoBehaviour
{
    public TMP_Text youWinText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        youWinText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MainMenu");
    }
}
