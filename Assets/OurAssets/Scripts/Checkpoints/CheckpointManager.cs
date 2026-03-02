using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    readonly StackADT<Checkpoint> checkpointStack = new StackADT<Checkpoint>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void SetStartingCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpointStack.IsEmpty && checkpointStack.Peek() != checkpoint)
        {
            checkpoint.IsStart = false;
            checkpoint.StartingLives = 0;
        }
        else CaptureCheckpoint(checkpoint);
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
            // Respawn
        }
        else
        {
            // Death Screen
        }
    }
}
