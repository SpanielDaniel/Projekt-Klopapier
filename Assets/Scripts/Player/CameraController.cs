// Author: Daniel Bäcker

using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] private Camera CurrentCamera;
    [SerializeField] private float PanBorderThickness;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private float RotationSpeed;
    

    private void Update()
    {
        // --- Input Handler ---
        
        // Save the current position of the camera.
        Vector3 pos = transform.position;
        
        // Movement forward, backward, right and left.
        if (Input.GetKey(KeyCode.W)||  Input.mousePosition.y >= Screen.height - PanBorderThickness) 
            pos += Vector3.forward * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.S)||  Input.mousePosition.y <= PanBorderThickness) 
            pos += Vector3.back * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.D)||  Input.mousePosition.x >= Screen.width - PanBorderThickness) 
            pos += Vector3.right * (Time.deltaTime * MovementSpeed);
        
        if (Input.GetKey(KeyCode.A)||  Input.mousePosition.x <= PanBorderThickness) 
            pos += Vector3.left * (Time.deltaTime * MovementSpeed);
        
        // Scrolling up and down dependence of the scroll speed
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) pos += new Vector3(0,-1,1) * (Time.deltaTime * ScrollSpeed * 100); // Scroll diagonal into the map.
        if (Input.GetAxis("Mouse ScrollWheel") < 0f ) pos += new Vector3(0,1,-1) * (Time.deltaTime * ScrollSpeed * 100); // Scroll diagonal out of the map.

        transform.position = pos;
        
    }
    
}
