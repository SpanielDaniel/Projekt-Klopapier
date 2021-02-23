// File     : MeshCameraHandler.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace Code.Scripts
{
    public class MeshCameraHandler : MonoBehaviour
    {
        [SerializeField] private float PanBorderThickness;
        [SerializeField] private float CameraSize;
        [SerializeField] private float MaxCameraSize = 60;
        [SerializeField] private float MinCameraSize = 10;
        [SerializeField] private float ScrollSpeed;
        [SerializeField] private float SCROLL_VALUE;
        [SerializeField] private float MovementSpeed;

        [SerializeField] private float XMaxPos;
        [SerializeField] private float ZMaxPos;
        private float CameraSizeHandler
        {
            get => CameraSize;
            set
            {
                CameraSize = value;
                if (CameraSize > MaxCameraSize) CameraSize = MaxCameraSize;
                if (CameraSize < MinCameraSize) CameraSize = MinCameraSize;
                SetCameraSize();
            }
        }
        private void Start()
        {
            SetCameraSize();
        }


        private void Update()
        {
            Vector3 currentCameraPosition = Camera.main.transform.position;
            if (Input.GetKey(KeyCode.W) ||  Input.mousePosition.y >= Screen.height - PanBorderThickness && Input.mousePosition.x <= Screen.height + PanBorderThickness) 
                currentCameraPosition += Vector3.forward * (Time.deltaTime * MovementSpeed);
        
            if (Input.GetKey(KeyCode.S) ||  Input.mousePosition.y <= PanBorderThickness && Input.mousePosition.y >= -PanBorderThickness) 
                currentCameraPosition += Vector3.back * (Time.deltaTime * MovementSpeed);
        
            if (Input.GetKey(KeyCode.D) ||  Input.mousePosition.x >= Screen.width - PanBorderThickness && Input.mousePosition.x <= Screen.width + PanBorderThickness) 
                currentCameraPosition += Vector3.right * (Time.deltaTime * MovementSpeed);
        
            if (Input.GetKey(KeyCode.A) ||  Input.mousePosition.x <= PanBorderThickness && Input.mousePosition.x >= -PanBorderThickness) 
                currentCameraPosition += Vector3.left * (Time.deltaTime * MovementSpeed);
            
            // Scrolling up and down dependence of the scroll speed
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                CameraSizeHandler -=  ScrollSpeed * SCROLL_VALUE * Time.deltaTime * 100; // Scroll diagonal into the map.

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                CameraSizeHandler +=  ScrollSpeed * SCROLL_VALUE * Time.deltaTime * 100; // Scroll diagonal out of the map.

            
            if(currentCameraPosition.x > XMaxPos) currentCameraPosition = new Vector3(XMaxPos, currentCameraPosition.y,currentCameraPosition.z);
            if(currentCameraPosition.z > ZMaxPos) currentCameraPosition = new Vector3(currentCameraPosition.x, currentCameraPosition.y,ZMaxPos);
            
            if(currentCameraPosition.x < 0) currentCameraPosition = new Vector3(0, currentCameraPosition.y,currentCameraPosition.z);
            if(currentCameraPosition.z < 0) currentCameraPosition = new Vector3(currentCameraPosition.x, currentCameraPosition.y,0);

            Camera.main.transform.position = currentCameraPosition;
        }

        private void SetCameraSize()
        {
            Camera.main.orthographicSize = CameraSize;
            foreach (GameObject res in ResourceGenerator.Resources)
            {
                res.transform.localScale = Vector3.one * CameraSize / 50;
            }
        }
        
        private void OnValidate()
        {
            if(Application.isPlaying) CameraSizeHandler = CameraSizeHandler;
        }
    }
}