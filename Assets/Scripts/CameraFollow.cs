using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Reference to the player's transform (position, rotation, scale)
    private Transform player;

    // Temporary position variable to modify the camera's position
    private Vector3 tempPos;

    // Serialized fields to define camera boundaries
    [SerializeField]
    private float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object using its "Player" tag and get its transform
        player = GameObject.FindWithTag("Player").transform;
    }

    // LateUpdate is called after all Update methods have run
    void LateUpdate()
    {
        // Copy the current camera position into a temporary variable
        tempPos = transform.position;

        // Set the camera's x and y position to match the player's position
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;

        // Optionally, clamp the camera's position to stay within defined boundaries
        // Uncomment this block to enforce bounds
        // tempPos.x = Mathf.Clamp(tempPos.x, minX, maxX);
        // tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

        // Apply the updated position back to the camera
        transform.position = tempPos;
    }
}
