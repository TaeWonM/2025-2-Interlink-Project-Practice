using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardController : MonoBehaviour
{

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput is NULL");
        }
        gameObject.SetActive(false);
    }

    public void OnEscPushed()
    {
        playerInput.camera = Cameras.CameraDict.Curcameras;
        if (this.gameObject.activeSelf)
        {

            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}