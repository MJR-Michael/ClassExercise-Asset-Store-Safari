using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex; // 0 = first, 1 = second, etc.
    private CheckpointManager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<CheckpointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.HitCheckpoint(checkpointIndex);
            Destroy(gameObject);

        }
    }
}
