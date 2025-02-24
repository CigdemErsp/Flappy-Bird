using UnityEngine;

[CreateAssetMenu(menuName = "Checkpoint/CheckpointData", fileName = "NewCheckpointData")]
public class CheckpointData : ScriptableObject
{
    public int playerScore;
    public int distance;
    public int playerCoin;
}