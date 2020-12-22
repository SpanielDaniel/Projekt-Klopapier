// File     : TestBase.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace TestScripts
{
    public abstract  class TestBase : MonoBehaviour
    , ITest
    {
        //public  int TestNubmer = 12;


        public abstract void DoAction();
        
        public void TestFunction()
        {
            Debug.Log("TestBestanden");
        }
    }
}