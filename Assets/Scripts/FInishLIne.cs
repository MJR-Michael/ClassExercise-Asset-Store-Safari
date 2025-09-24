using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private CheckpointManager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (manager.AllCheckpointsCleared())
            {
                manager.FinishRace();
            }
            else
            {
                Debug.Log("❌ You must clear all checkpoints first!");
            }
        }
    }
}
