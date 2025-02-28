using UnityEngine;

[CreateAssetMenu(menuName = "Checkpoint/CheckpointData", fileName = "NewCheckpointData")]
public class CheckpointData : ScriptableObject
{
    #region private fields
    private int _playerScore;
    private int _distance;
    private int _playerCoin;
    private int _checkpointCount;

    private bool _hasCheckpoint;
    #endregion

    public int PlayerScore {  get { return _playerScore; } set { _playerScore = value; } }
    
    public int Distance { get { return _distance; } set { _distance = value; } }

    public int PlayerCoin { get { return _playerCoin; } set { _playerCoin = value; } }

    public int CheckpointCount { get { return _checkpointCount; } set { _checkpointCount = value; } }

    public bool HasCheckpoint { get { return _hasCheckpoint; } set { _hasCheckpoint = value; } }

    public void OnDisable()
    {
        _hasCheckpoint = false;
        _distance = 0;
        _playerScore = 0;
        _playerCoin = 0;
    }
}