using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pac_Man;

namespace Test_Mueve_PacMan
{
    [TestClass]
    public class Mueve_Pacman
    {
        //Test normales
        [TestMethod]
        public void MueveArriba()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            tab.setPersonaje(0, posX,posY, -1, 0);
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, -1, 0, out nx, out ny);
            tab.muevePacman();

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover");
            Assert.AreEqual(0, tab.getPersonaje(0).posX, "Error, tendria que haber movido a Pac-Man");
            Assert.AreEqual(1, tab.getPersonaje(0).posY, "Error,tendria que haber movido a Pac-Man");

        }
        [TestMethod]
        public void MueveIzq()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            tab.setPersonaje(0, posX, posY, 0, -1);
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, 0, -1, out nx, out ny);
            tab.muevePacman();

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover");
            Assert.AreEqual(1, tab.getPersonaje(0).posX, "Error, tendria que haber movido a Pac-Man");
            Assert.AreEqual(0, tab.getPersonaje(0).posY, "Error,tendria que haber movido a Pac-Man");

        }

        //Test toroidales
        [TestMethod]
        public void MueveArribaToro()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 0;
            int posY = 1;
            tab.setPersonaje(0, posX, posY, -1, 0);
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, -1, 0, out nx, out ny);
            tab.muevePacman();

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover");
            Assert.AreEqual(2, tab.getPersonaje(0).posX, "Error, tendria que haber movido a Pac-Man");
            Assert.AreEqual(1, tab.getPersonaje(0).posY, "Error,tendria que haber movido a Pac-Man");

        }
        [TestMethod]
        public void MueveIzqToro()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 0;
            tab.setPersonaje(0, posX, posY, 0, -1);
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, 0, -1, out nx, out ny);
            tab.muevePacman();

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover");
            Assert.AreEqual(1, tab.getPersonaje(0).posX, "Error, tendria que haber movido a Pac-Man");
            Assert.AreEqual(2, tab.getPersonaje(0).posY, "Error,tendria que haber movido a Pac-Man");

        }
    }
}
