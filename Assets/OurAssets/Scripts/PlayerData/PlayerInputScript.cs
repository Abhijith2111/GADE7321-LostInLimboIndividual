using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerLook))]
public class PlayerInputScript : MonoBehaviour
{
    PlayerMovement pM;
    PlayerLook pL;

    void Awake()
    {
        pM = GetComponent<PlayerMovement>();
        pL = GetComponent<PlayerLook>();
    }

    public void EnableCharacterInput()
    {
        pM.EnableMovement();
        pL.EnableLook();
        Time.timeScale = 1f;
    }

    public void DisableCharacterInput()
    {
        pM.DisableMovement();
        pL.DisableLook();
        Time.timeScale = 0f;
    }
}
