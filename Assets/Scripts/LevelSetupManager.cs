using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject UIPrefab;
    public GameObject InputHandlerPrefab;
    public GameObject CameraPrefab;
    public GameObject EventSystemPrefab;

    private UIController UI;

    public void Awake()
    {
        GameObject player = Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        UI = Instantiate(UIPrefab, transform.position, Quaternion.identity).GetComponent<UIController>();
        Instantiate(EventSystemPrefab, transform.position, Quaternion.identity);
        Instantiate(InputHandlerPrefab, transform.position, Quaternion.identity).GetComponent<InputHandler>();
        GameObject camera = Instantiate(CameraPrefab, transform.position, Quaternion.identity);

        camera.GetComponent<CameraFollow>().playerGameObject = player;
        TopDownMovement tdm = player.GetComponent<TopDownMovement>();
        tdm.camPos = camera.transform;
        tdm.SetCamera(camera);
        InteractionController interactionController = player.GetComponent<InteractionController>();
        interactionController.UIController = UI;

        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        UI.CreateRegularInterfaces();
        UI.ToggleInventory();
        UI.Player = player;
        UI.interactionController = interactionController;
        UI.playerShooting = playerShooting;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}