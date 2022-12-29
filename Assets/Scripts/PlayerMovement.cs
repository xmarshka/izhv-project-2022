using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private GameObject gameCamera;
    private CharacterController characterController;

    public void SetPosition(Vector3 position)
    {
        characterController.enabled = false;
        transform.position = position;
        characterController.enabled = true;
    }

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        playerStatus = gameObject.GetComponent<PlayerStatus>();
        gameCamera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        Vector3 playerForward, playerSideways;
        playerSideways = Input.GetAxis("Horizontal") * gameCamera.transform.right;
        playerForward = Input.GetAxis("Vertical") * Vector3.Normalize(Vector3.ProjectOnPlane(gameCamera.transform.forward, Vector3.up));

        // Vector3 can be implicitly converted to Vector2 (z is discarted)
        Vector3 playerInput = Vector3.ClampMagnitude(playerForward + playerSideways, 1.0f);

        Vector3 displacement = playerInput * Time.deltaTime * playerStatus.speed;
        characterController.Move(displacement);
    }
}
