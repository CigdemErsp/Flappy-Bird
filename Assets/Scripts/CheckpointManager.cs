using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private CheckpointData checkpointData;
    // Reference to the player (or other systems) can be set in the Inspector

    // Call this method when a checkpoint is reached
    public void SaveCheckpoint(int newPlayerScore, int distance, int coin)
    {
        checkpointData.playerScore = newPlayerScore;
        checkpointData.distance = distance;
        checkpointData.playerCoin = coin;
        Debug.Log("Checkpoint saved!");
    }

    // Call this method to restore the player state from the checkpoint
    public void LoadCheckpoint(GameObject player)
    {
        // You can also restore other data (e.g., score, health)
        Debug.Log("Checkpoint loaded!");
    }
}