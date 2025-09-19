using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Collectible : MonoBehaviour
{
    private int score = 0;
    public TMP_Text scoreText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            score++;
            UpdateScoreUI();
            Debug.Log("Coin collected! Score: " + score);
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