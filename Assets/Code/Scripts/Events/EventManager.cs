// File     : EventManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.IO;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

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

        /// <summary>
        /// Starts the next Event Window
        /// </summary>
        /// <param name="_nextFileName"></param>
        /// <param name="_nextEventIndex"></param>
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
        
        /// <summary>
        /// Starts an event.
        /// </summary>
        /// <param name="_fileName">filename of the event</param>
        /// <param name="_startEventIndex">index of the event</param>
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
            
            index = text.IndexOf("EF:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string effect = text.Substring(indexStart + 1, indexEnd - indexStart -1);
            
            index = text.IndexOf("A:",index);
            indexStart = text.IndexOf('"',index);
            indexEnd = text.IndexOf('"',indexStart + 1);
            string amount = text.Substring(indexStart + 1, indexEnd - indexStart -1);


            if (effect != "")
            {
                
                StartEffect(effect,int.Parse(amount));
            }
            
            GameObject eventWindow = Instantiate(EventWindowPrefab,Parent.transform);

            EventWindow ev = eventWindow.GetComponent<EventWindow>();
            ev.SetNextFileName(nextEventFileName);
            ev.SetHeaderText(headerText);
            ev.SetDescriptionText( descriptionText);
            ev.SetEffectText(effectText);
            
            if(nextEventIndex != "") ev.SetNextID(int.Parse(nextEventIndex));
        }

        public void StartRandomEvent()
        {

            int random =Random.Range(1, 5);
            StartEvent("RandomEvent",random);
        }

        private void StartEffect(string _effect, int _amount)
        {
            switch (_effect[0])
            {
                case 'W':
                    PlayerData.GetInstance.IncreaseWood(_amount);
                    break;
                case 'S':
                    PlayerData.GetInstance.IncreaseStone(_amount);
                    break;

                case 'T':
                    PlayerData.GetInstance.IncreaseSteel(_amount);
                    break;

                case 'P':
                    PlayerData.GetInstance.IncreaseToiletPaper(_amount);
                    break;

                case 'F':
                    PlayerData.GetInstance.IncreaseFood(_amount);
                    break;

            }
        }
    }
}