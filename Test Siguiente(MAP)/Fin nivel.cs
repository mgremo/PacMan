using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pac_Man;

namespace Test_Siguiente__MAP_
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Acaba()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setNumComida(3);
            //Act
            bool noacaba = tab.finNivel();
            tab.setNumComida(0);
            bool acaba = tab.finNivel();
            //Assert
            Assert.IsFalse(noacaba, "Error, todavia sigue habiendo comida");
            Assert.IsTrue(acaba, "Error, ya no hay comida");
        }
    }
}
