// File     : EventManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.IO;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class EventManager : Singleton<EventManager>
    {
        [SerializeField] private GameObject EventWindowPrefab;
        [SerializeField] private GameObject Parent;

        private string FolderPath;
        private string FileEnding = ".event";


        private void Start()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory("Test2000");
            FolderPath = directoryInfo.FullName;
            
            StartEvent("Events");
            
        }

        public void StartEvent(string _fileName)
        {
            string text = File.ReadAllText(FolderPath + "\\" +  _fileName + FileEnding);

            int index;
            int indexStart;
            int indexEnd;

            string[] splitString = text.Split(';');
            int StartEventIndex = 1;

            index = text.IndexOf("\""+ StartEventIndex + "\"");
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string id = text.Substring(indexStart + 1, indexEnd - indexStart - 1);


            index = text.IndexOf("H:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string headerText = text.Substring(indexStart + 1, indexEnd - indexStart -1);

            
            index = text.IndexOf("D:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            
            string descriptionText = text.Substring(indexStart + 1, indexEnd - indexStart -1);

            index = text.IndexOf("E:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            
            string effectText = text.Substring(indexStart + 1, indexEnd - indexStart -1);

            
            
            GameObject eventWindow = Instantiate(EventWindowPrefab,Parent.transform);

            EventWindow ev = eventWindow.GetComponent<EventWindow>();
            ev.SetHeaderText(headerText);
            ev.SetDescriptionText( descriptionText);
            ev.SetEffectText(effectText);


        }



    }
}