using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private CheckpointData checkpointData;

    // Call this method when a checkpoint is reached
    public void SaveCheckpoint(int newPlayerScore, int distance, int coin)
    {
        checkpointData.PlayerScore = newPlayerScore;
        checkpointData.Distance = distance;
        checkpointData.PlayerCoin = coin;
        checkpointData.HasCheckpoint = true;
    }

    // Call this method to restore the player state from the checkpoint
    public void LoadCheckpoint(GameObject player)
    {
        Player playerController = player.GetComponent<Player>();
        if (playerController != null)
        {
            if (checkpointData.HasCheckpoint)
            {
                // Restore state from checkpoint
                playerController.RestoreState(checkpointData.PlayerScore, checkpointData.Distance, checkpointData.PlayerCoin);
            }
            else
            {
                playerController.RestoreState(0, 0, 0);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController component not found on the player GameObject.");
        }
    }
}