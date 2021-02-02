// Author: Daniel Bäcker

using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float PanBorderThickness;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MaxCameraHeight = 10f;
    [SerializeField] private float MinCameraHeight = 1.0f;

    [SerializeField] private float MinXPos;
    [SerializeField] private float MaxXPos;

    [SerializeField] private float MinZPos;
    [SerializeField] private float MaxZPos;
    

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
        
        if(currentCameraPosition.x < MinXPos) currentCameraPosition = new Vector3(MinXPos,currentCameraPosition.y,currentCameraPosition.z);
        if(currentCameraPosition.x > MaxXPos) currentCameraPosition = new Vector3(MaxXPos,currentCameraPosition.y,currentCameraPosition.z);
        
        if(currentCameraPosition.z + currentCameraPosition.y <= MinZPos) currentCameraPosition = new Vector3(currentCameraPosition.x,currentCameraPosition.y,MinZPos - currentCameraPosition.y);
        if(currentCameraPosition.z + currentCameraPosition.y >= MaxZPos) currentCameraPosition = new Vector3(currentCameraPosition.x,currentCameraPosition.y,MaxZPos - currentCameraPosition.y);
        
        if (currentCameraPosition.y != 2)
        {
            // Scrolling up and down dependence of the scroll speed
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                currentCameraPosition +=
                    new Vector3(0, -1, 1) * (ScrollSpeed * SCROLL_VALUE * Time.deltaTime * 100); // Scroll diagonal into the map.
        }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                currentCameraPosition +=
                    new Vector3(0, 1, -1) * (ScrollSpeed * SCROLL_VALUE * Time.deltaTime * 100); // Scroll diagonal out of the map.

            if (currentCameraPosition.y <= MinCameraHeight)
            {
                currentCameraPosition += new Vector3(0, -1, 1) * currentCameraPosition.y +
                                         new Vector3(0, 1, -1) * MinCameraHeight;
            }
            
            if (currentCameraPosition.y >= MaxCameraHeight)
            {
                currentCameraPosition += new Vector3(0, -1, 1) * currentCameraPosition.y +
                                         new Vector3(0, 1, -1) * MaxCameraHeight;
            }

            transform.position = currentCameraPosition;
    }
    
}
