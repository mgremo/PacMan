using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pac_Man;
namespace Test_Siguiente__MAP_
{
    [TestClass]
    public class TestCaptura
    {
        [TestMethod]
        public void EstaCapturado()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 0, 0, 1);
            tab.setPersonaje(1, 1, 2, 0, -1);
            tab.setPersonaje(2, 0, 0, 0, 1);
            tab.setPersonaje(3, 1, 1, 0, -1);
            tab.setPersonaje(4, 2, 2, 0, -1);

            //Act
            tab.muevePacman();
            bool Capturado = tab.captura();
            if (!Capturado)
            {
                tab.mueveFantasma(1);
                Capturado = tab.captura();
            }

            //Assert
            Assert.IsTrue(Capturado, "Error, PacMan debería estar capturado por el fantasma");
        }
    }
}
