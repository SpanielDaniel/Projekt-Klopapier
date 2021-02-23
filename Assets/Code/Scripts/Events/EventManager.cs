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

        public void Awake()
        {
            EventWindow.OnEventWindowClosed += StartNextEvent;
            
        }
        
        private void Start()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory("Events");
            FolderPath = directoryInfo.FullName;
            StartEvent("Events",1);
        }

        private void StartNextEvent(string _nextFileName,int _nextEventIndex)
        {
            if (_nextEventIndex == 0 || _nextEventIndex == null)
            {
                Time.timeScale = 1.0f;
                return;
            }
            if(_nextFileName == "") return;
            
            StartEvent(_nextFileName,_nextEventIndex);
        }

        public void StartEvent(string _fileName, int _startEventIndex)
        {
            Time.timeScale = 0.0f;
            string text = File.ReadAllText(FolderPath + "\\" +  _fileName + FileEnding);

            int index;
            int indexStart;
            int indexEnd;
            
            
            index = text.IndexOf("\""+ _startEventIndex + "\"");
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

            index = text.IndexOf("NID:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string nextEventIndex = text.Substring(indexStart + 1, indexEnd - indexStart -1);
            
            
            index = text.IndexOf("NFN:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string nextEventFileName = text.Substring(indexStart + 1, indexEnd - indexStart -1);
            
            
            
            GameObject eventWindow = Instantiate(EventWindowPrefab,Parent.transform);

            EventWindow ev = eventWindow.GetComponent<EventWindow>();
            ev.SetNextFileName(nextEventFileName);
            ev.SetHeaderText(headerText);
            ev.SetDescriptionText( descriptionText);
            ev.SetEffectText(effectText);
            
            if(nextEventIndex != "") ev.SetNextID(int.Parse(nextEventIndex));
        }
    }
}