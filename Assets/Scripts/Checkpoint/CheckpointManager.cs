using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    #region serializefields
    [SerializeField] private CheckpointData _checkpointData;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    #endregion

    // Call this method when a checkpoint is reached
    public void SaveCheckpoint(int newPlayerScore, int distance, int coin, int numberOfCheckpoint)
    {
        _checkpointData.PlayerScore = newPlayerScore;
        _checkpointData.Distance = distance;
        _checkpointData.PlayerCoin = coin;
        _checkpointData.HasCheckpoint = true;
        _checkpointData.CheckpointCount = numberOfCheckpoint;
    }

    // Call this method to restore the player state from the checkpoint
    public void LoadCheckpoint(GameObject player)
    {
        Player playerController = player.GetComponent<Player>();
        if (playerController != null)
        {
            if (_checkpointData.HasCheckpoint)
            {
                // Restore state from checkpoint
                playerController.RestoreState(_checkpointData.PlayerScore, _checkpointData.Distance, _checkpointData.PlayerCoin);
                _obstacleSpawner.NumberOfCheckpoints = _checkpointData.CheckpointCount;

            }
            else
            {
                playerController.RestoreState(0, 0, 0);
                _obstacleSpawner.NumberOfCheckpoints = 0;
            }
        }
        else
        {
            Debug.LogWarning("PlayerController component not found on the player GameObject.");
        }
    }
}