// File     : TestClass.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace TestScripts
{
    public class TestClass : TestBase
    {
        private int SecondNumber;

        public override void DoAction()
        {
            Debug.Log("Action");
        }
    }
}