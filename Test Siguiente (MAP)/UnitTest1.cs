using System;
using Pac_Man;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_Siguiente__MAP_
{
    [TestClass]
    public class UnitTest1
    { 
        //Test siguiente normal
        [TestMethod]
        public void SiguienteVacioUp()
        {
            //Arrange 
            //Nuevo tablero de 3x3
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, -1, 0, out nx, out ny);

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover arriba");
            Assert.AreEqual(nx, 0, "Error, tendria que moverse a la pos 0");
            Assert.AreEqual(ny, 1, "Error, tendria que moverse a la pos Y 1");


        }
        [TestMethod]
        public void SiguienteVacioDown()
        {
            //Arrange 
            //Nuevo tablero de 3x3
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, 1, 0, out nx, out ny);

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover abajo");
            Assert.AreEqual(nx, 2, "Error, tendria que moverse a la pos X 2");
            Assert.AreEqual(ny, 1, "Error, tendria que moverse a la pos Y 1");
        }
        [TestMethod]
        public void SiguienteVacioLeft()
        {
            //Arrange 
            //Nuevo tablero de 3x3
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, 0, -1, out nx, out ny);

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover izquierda");
            Assert.AreEqual(nx, 1, "Error, tendria que moverse a la pos X 1");
            Assert.AreEqual(ny, 0, "Error, tendria que moverse a la pos Y 0");
        }
        [TestMethod]
        public void SiguienteVacioRight()
        {
            //Arrange 
            //Nuevo tablero de 3x3
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 1;
            int nx, ny;

            //Act
            bool puedeMover = tab.siguiente(posX, posY, 0, 1, out nx, out ny);

            //Assert
            Assert.IsTrue(puedeMover, "Error: Tendria que poder mover derecha");
            Assert.AreEqual(nx, 1, "Error, tendria que moverse a la pos X 1");
            Assert.AreEqual(ny, 2, "Error, tendria que moverse a la pos Y 2");
        }

        //Test siguiente toroidal
        [TestMethod]
        public void SiguienteToroUp()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 0;
            int posY = 1;
            int nx, ny;

            //Act
            bool PuedeMover = tab.siguiente(posX, posY, -1, 0, out nx, out ny);

            //Arrange
            Assert.IsTrue(PuedeMover, "Error, debería poderse mover");
            Assert.AreEqual(nx, 2, "Error, debería estar abajo tras salir por arriba");
            Assert.AreEqual(ny, 1, "Error, no se debería haber cambiado de columna");
        }
        [TestMethod]
        public void SiguienteToroDown()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 2;
            int posY = 1;
            int nx, ny;

            //Act
            bool PuedeMover = tab.siguiente(posX, posY, 1, 0, out nx, out ny);

            //Arrange
            Assert.IsTrue(PuedeMover, "Error, debería poderse mover");
            Assert.AreEqual(nx, 0, "Error, debería estar arriba tras salir por abajo");
            Assert.AreEqual(ny, 1, "Error, no se debería haber cambiado de columna");
        }
        [TestMethod]
        public void SiguienteToroLeft()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 0;
            int nx, ny;

            //Act
            bool PuedeMover = tab.siguiente(posX, posY, 0, -1, out nx, out ny);

            //Arrange
            Assert.IsTrue(PuedeMover, "Error, debería poderse mover");
            Assert.AreEqual(nx, 1, "Error, no se debería haber cambiado de fila");
            Assert.AreEqual(ny, 2, "Error, tendria que estar en el lado derecho");
        }
        [TestMethod]
        public void SiguienteToroRight()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1;
            int posY = 2;
            int nx, ny;

            //Act
            bool PuedeMover = tab.siguiente(posX, posY, 0, 1, out nx, out ny);

            //Arrange
            Assert.IsTrue(PuedeMover, "Error, debería poderse mover");
            Assert.AreEqual(nx, 1, "Error, no se debería haber cambiado de fila");
            Assert.AreEqual(ny, 0, "Error, tendria que estar en el lado izquierdo");
        }

    }
}
