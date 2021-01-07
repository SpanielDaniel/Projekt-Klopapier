﻿// Author: Daniel Bäcker

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float PanBorderThickness;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private float RotationSpeed;

    private const float SCROLL_VALUE = 0.1f;
    

    private void Update()
    {
        // --- Input Handler ---
        
        // Save the current position of the camera.
        Vector3 currentCameraPosition = transform.position;
        
        // Movement forward, backward, right and left.
        if (Input.GetKey(KeyCode.W) ||  Input.mousePosition.y >= Screen.height - PanBorderThickness && Input.mousePosition.x <= Screen.height + PanBorderThickness) 
            currentCameraPosition += Vector3.forward * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.S) ||  Input.mousePosition.y <= PanBorderThickness && Input.mousePosition.y >= -PanBorderThickness) 
            currentCameraPosition += Vector3.back * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.D) ||  Input.mousePosition.x >= Screen.width - PanBorderThickness && Input.mousePosition.x <= Screen.width + PanBorderThickness) 
            currentCameraPosition += Vector3.right * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.A) ||  Input.mousePosition.x <= PanBorderThickness && Input.mousePosition.x >= -PanBorderThickness) 
            currentCameraPosition += Vector3.left * (Time.deltaTime * MovementSpeed);

        if (currentCameraPosition.y != 2)
        {
            // Scrolling up and down dependence of the scroll speed
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                currentCameraPosition +=
                    new Vector3(0, -1, 1) * (ScrollSpeed * SCROLL_VALUE); // Scroll diagonal into the map.
        }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                currentCameraPosition +=
                    new Vector3(0, 1, -1) * (ScrollSpeed * SCROLL_VALUE); // Scroll diagonal out of the map.

            if (currentCameraPosition.y <= 2)
                currentCameraPosition = new Vector3(currentCameraPosition.x, 2, currentCameraPosition.z);

        transform.position = currentCameraPosition;
        
    }
    
}
