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
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 0, 1);

            //Act
            bool HaCambiado = tab.cambiaDir('r');

            //Assert
            Assert.IsTrue(HaCambiado, "Error, debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == 0 && tab.pers[0].dirY == 1, "Error, tendría que haberse cambiado la dir hacia la derecha");
        }
        [TestMethod]
        public void CambiaAIzquierda()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 0, 1);

            //Act
            bool HaCambiado = tab.cambiaDir('l');

            //Assert
            Assert.IsTrue(HaCambiado, "Error, debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == 0 && tab.pers[0].dirY == -1, "Error, tendría que haberse cambiado la dir hacia la izquierda");
        }
        [TestMethod]
        public void CambiaAArriba()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 1, 0);

            //Act
            bool HaCambiado = tab.cambiaDir('u');

            //Assert
            Assert.IsTrue(HaCambiado, "Error, debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == -1 && tab.pers[0].dirY == 0, "Error, tendría que haberse cambiado la dir hacia arriba");
        }
        [TestMethod]
        public void CambiaAAbajo()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 1, 0);

            //Act
            bool HaCambiado = tab.cambiaDir('d');

            //Assert
            Assert.IsTrue(HaCambiado, "Error, debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == 1 && tab.pers[0].dirY == 0, "Error, tendría que haberse cambiado la dir hacia abajo");
        }
        [TestMethod]
        public void NoCambiaADerecha()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, -1,0);//dir original hacia arriba
            tab.cambiaCasilla(0, 2,Tablero.Casilla.Muro);
            tab.cambiaCasilla(1, 2, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 2, Tablero.Casilla.Muro);

            //Act
            bool HaCambiado = tab.cambiaDir('r');

            //Arrange
            Assert.IsFalse(HaCambiado, "Error, no debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == -1 && tab.pers[0].dirY == 0, "Error, debería seguir moviendose hacia arriba");
        }
        [TestMethod]
        public void NoCambiaAIzquierda()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, -1, 0);//dir original hacia arriba
            tab.cambiaCasilla(0, 0, Tablero.Casilla.Muro);
            tab.cambiaCasilla(1, 0, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 0, Tablero.Casilla.Muro);

            //Act
            bool HaCambiado = tab.cambiaDir('l');

            //Arrange
            Assert.IsFalse(HaCambiado, "Error, no debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == -1 && tab.pers[0].dirY == 0, "Error, debería seguir moviendose hacia arriba");
        }
        [TestMethod]
        public void NoCambiaAArriba()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 0, 1);//dir original hacia arriba
            tab.cambiaCasilla(0, 0, Tablero.Casilla.Muro);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            tab.cambiaCasilla(0, 2, Tablero.Casilla.Muro);

            //Act
            bool HaCambiado = tab.cambiaDir('u');

            //Arrange
            Assert.IsFalse(HaCambiado, "Error, no debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == 0 && tab.pers[0].dirY == 1, "Error, debería seguir moviendose hacia la derecha");
        }
        [TestMethod]
        public void NoCambiaAAbajo()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(0, 1, 1, 0, 1);//dir original hacia arriba
            tab.cambiaCasilla(2, 0, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 1, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 2, Tablero.Casilla.Muro);

            //Act
            bool HaCambiado = tab.cambiaDir('d');

            //Arrange
            Assert.IsFalse(HaCambiado, "Error, no debería haberse cambiado la dir");
            Assert.IsTrue(tab.pers[0].dirX == 0 && tab.pers[0].dirY == 1, "Error, debería seguir moviendose hacia la derecha");
        }
    }
}
