using System;
using Pac_Man;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests_MAP
{
    [TestClass]
    public class No_Mueve
    {
        [TestMethod]
        public void NoMueveArriba()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            int posX = 1;
            int posY = 1;
            int nx, ny;
            tab.setPersonaje(0, posX, posY, -1, 0);

            //Act
            bool puedeMover = tab.siguiente(posX, posY, -1, 0, out nx, out ny);
            tab.muevePacman();

            //Assert
            Assert.IsFalse(puedeMover, "Error: No tendria que poder mover");
            Assert.AreEqual(posX, tab.getPersonaje(0).posX, "Error, tendria que mantener la posX");
            Assert.AreEqual(posY, tab.getPersonaje(0).posY, "Error, tendria que mantener la posY");

        }
    }
}
