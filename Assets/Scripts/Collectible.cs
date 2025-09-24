using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Collectible : MonoBehaviour
{
    private int score = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            score++;
            UpdateScoreUI();
            Debug.Log("Coin collected! Score: " + score);

            // Play sound
            if (audioSource != null && collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            Destroy(other.gameObject);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
