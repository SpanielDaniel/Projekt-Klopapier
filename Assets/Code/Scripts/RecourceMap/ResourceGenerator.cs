// File     : ResourceGenerator.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class ResourceGenerator : MonoBehaviour
    {
        public static List<GameObject> Resources = new List<GameObject>();
        public static List<GameObject> Lines = new List<GameObject>();
        
        [SerializeField] private int WoodAmount;
        [SerializeField] private int StoneAmount;
        [SerializeField] private int SteelAmount;
        [SerializeField] private int CityAmount;
        [SerializeField] private GameObject WoodImage;
        [SerializeField] private GameObject StoneImage;
        [SerializeField] private GameObject SteelImage;
        [SerializeField] private GameObject HomeImage;
        [SerializeField] private GameObject CityImage;
        [SerializeField] private GameObject Parent;
        [SerializeField] private float TimeToRegenerateResourceInSeconds;
        
        private MeshGenerator MeshGenerator;
        private List<Vector3> Vector3s = new List<Vector3>();


        [SerializeField] private GameObject LinePrefab;
        
        
        
        

        private int WoodPointsToAdd;
        
        
        private float WoodTimer = 0;
        
        
        private void Update()
        {
            if (WoodPointsToAdd > 0)
            {
                WoodTimer += Time.deltaTime;
                if (WoodTimer > TimeToRegenerateResourceInSeconds)
                {
                    WoodTimer = 0;
                    GenerateResource(WoodImage, 1);
                }
            }
        }

        public void Generate(MeshGenerator _meshGenerator)
        {
            MeshGenerator = _meshGenerator;


            GeneratePlaceableVectors();

            GenerateResource(HomeImage, 1);
            
            GenerateResource(WoodImage, WoodAmount);
            
            GenerateResource(StoneImage, StoneAmount);
            GenerateResource(SteelImage, SteelAmount);
            GenerateResource(CityImage, CityAmount);
            
            GenerateLine(EMapResource.Wood);
            GenerateLine(EMapResource.Stone);
            GenerateLine(EMapResource.Steel);
            GenerateLine(EMapResource.City);

            foreach (GameObject line in Lines)
            {
                line.SetActive(false);
            }
        }

        

        private void GeneratePlaceableVectors()
        {
            Vector3[] vecs = MeshGenerator.GetVertices;

            foreach (Vector3 vec in vecs)
            {
                if (vec.y > 1 && vec.y < 1.8f && vec.z < 90)
                {
                    Vector3s.Add(vec);
                }
            }
        }
        private void GenerateResource(GameObject _resourceObj, int _amount)
        {
            for (int j = 0; j < _amount; j++)
            {
                int i = Random.Range(0, Vector3s.Count);
                
                GameObject obj = Instantiate(_resourceObj,Parent.transform);
                obj.transform.position = new Vector3(Vector3s[i].x,4,Vector3s[i].z);
                Resources.Add(obj);
                Vector3s.Remove(Vector3s[i]);
            }
        }


        private void GenerateLine(EMapResource _res)
        {
            foreach (GameObject obj in Resources)
            {
                
                if (obj.GetComponent<MapResource>() != null)
                {
                    EMapResource res = obj.GetComponent<MapResource>().GetRes;
                    if (res == _res)
                    {
                        GameObject line = Instantiate(LinePrefab, Parent.transform);
                        Vector3 homePosition = GetHomeMarker().transform.localPosition;
                        Vector3 markerPos = obj.transform.localPosition;

                        line.transform.localPosition = homePosition;
                        line.transform.localPosition += (markerPos - homePosition) / 2;

                        
                        
                         float distanceBetweenHomeAndMarker = Vector3.Distance(homePosition, markerPos);
                         float radius = Vector3.SignedAngle(Vector3.right, markerPos - homePosition,Vector3.forward);
                         
                        
                        
                         line.transform.localRotation = Quaternion.Euler(0,0,radius );
                         line.GetComponent<RectTransform>().sizeDelta = new Vector2( distanceBetweenHomeAndMarker,line.GetComponent<RectTransform>().rect.height);


                         Lines.Add(line);
                    }
                }
            }
        }

        private GameObject GetHomeMarker() => Resources[0];

        private void RemoveResourceObject(GameObject _resourceObj,EResource _res)
        {
            if (Resources.Contains(_resourceObj))
            {
                Resources.Remove(_resourceObj);
                
                if (_res == EResource.Wood) WoodPointsToAdd++;
                
                Destroy(_resourceObj);
            }
            else
            {
                Debug.Log("Resource konnte in der Liste nicht gefunden werden");
            }
        }
    }
}