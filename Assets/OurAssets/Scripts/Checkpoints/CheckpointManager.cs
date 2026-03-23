using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    static CheckpointManager _instance;
    public static CheckpointManager Instance
    {
        get
        {
            // Lazy instantiation
            if (!_instance)
            {
                GameObject go = new GameObject("CheckpointManager");
                _instance = go.AddComponent<CheckpointManager>();
            }
            return _instance;
        }
    }

    readonly StackADT<Checkpoint> checkpointStack = new StackADT<Checkpoint>();

    PlayerRespawner _player;
    PlayerRespawner Player
    {
        get
        {
            // This ensures that if a player exists they will be used. We don't want
            // to assign the player in awake since checkpoint manager awake might
            // happen before player respawner gets created. We also don't want to
            // assign the player in start since checkpoint start might happen before
            // checkpoint manager start and then player won't be assigned when trying
            // to spawn the player at the starting checkpoint
            if (!_player) _player = FindFirstObjectByType<PlayerRespawner>();
            return _player;
        }
    }

    void Awake()
    {
        if (_instance && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public void SetStartingCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpointStack.IsEmpty && checkpointStack.Peek() != checkpoint)
        {
            checkpoint.IsStart = false;
            checkpoint.StartingLives = 0;
        }
        else
        {
            CaptureCheckpoint(checkpoint);
            Transform startingPoint = checkpoint.RespawnPoint ?? checkpoint.transform;
            Player.Respawn(startingPoint);
        }
    }

    public void CaptureCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint.HasBeenCaptured) return;
        checkpoint.HasBeenCaptured = true;
        if (!checkpointStack.IsEmpty)
        {
            Checkpoint last = checkpointStack.Pop();
            checkpoint.Lives = last.Lives;
            checkpoint.Score = last.Score;
        }
        checkpointStack.Push(checkpoint);
    }

    public void LoseLife()
    {
        if (checkpointStack.IsEmpty) return;
        --checkpointStack.Peek().Lives;
        if (checkpointStack.Peek().Lives > 0)
        {
            Transform respawnPoint = checkpointStack.Peek().RespawnPoint ?? checkpointStack.Peek().transform;
            Player.Respawn(respawnPoint);
        }
        else
        {
            // Death Screen
        }
    }
}
