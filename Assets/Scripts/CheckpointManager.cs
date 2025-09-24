using UnityEngine;
using TMPro;

public class CheckpointManager : MonoBehaviour
{
    [Header("How many checkpoints in the race")]
    [SerializeField] private int totalCheckpoints = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI checkpointText;

    private int nextCheckpointIndex = 0;
    private bool raceFinished = false;

    private void Start()
    {
        UpdateCheckpointUI();
    }

    public void HitCheckpoint(int checkpointIndex)
    {
        if (raceFinished) return; // stop updating after finish

        if (checkpointIndex == nextCheckpointIndex)
        {
            Debug.Log($"✅ Checkpoint {checkpointIndex + 1} cleared!");
            nextCheckpointIndex++;
            UpdateCheckpointUI();
        }
        else
        {
            Debug.Log($"❌ Wrong checkpoint! Next required: {nextCheckpointIndex + 1}");
        }
    }

    public bool AllCheckpointsCleared()
    {
        return nextCheckpointIndex >= totalCheckpoints;
    }

    public void FinishRace()
    {
        raceFinished = true;
        checkpointText.text = "🏁 Race Finished!";
        Debug.Log("🏁 Race finished!");
    }

    public void ResetCheckpoints()
    {
        nextCheckpointIndex = 0;
        raceFinished = false;
        UpdateCheckpointUI();
        Debug.Log("🔄 Checkpoints reset!");
    }

    private void UpdateCheckpointUI()
    {
        int shownCheckpoint = Mathf.Min(nextCheckpointIndex, totalCheckpoints);
        checkpointText.text = $"Checkpoint: {shownCheckpoint}/{totalCheckpoints}";
    }
}
