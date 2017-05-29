using System;
using Pac_Man;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCambiaDIR
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CambiaADerecha()
        {
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 0, 1);
            
        }
    }
}
