using System;
using Pac_Man;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_Siguiente__MAP_
{
    [TestClass]
    public class UnitTest1
    {
        Tablero tab = new Tablero("test.dat");
        [TestMethod]
        public void SiguienteVacioUp()
        {
            int nx, ny;
            Assert.IsTrue(tab.siguiente(1, 4, -1,0, out nx, out ny),"Error, tendria que poder moverse");
            Assert.IsTrue(nx == 0, "Error: La nueva posX esta mal definida");
            Assert.IsTrue(ny == 4, "Error: La nueva posY esta mal definida");
        }
        [TestMethod]
        public void SiguienteVacioDown()
        {
            int nx, ny;
            Assert.IsTrue(tab.siguiente(1, 4, 1, 0, out nx, out ny), "Error, tendria que poder moverse");
            Assert.IsTrue(nx == 2, "Error: La nueva posX esta mal definida");
            Assert.IsTrue(ny == 4, "Error: La nueva posY esta mal definida");
        }
        [TestMethod]
        public void SiguienteVacioLeft()
        {
            int nx, ny;
            Assert.IsTrue(tab.siguiente(1, 4, 0, -1, out nx, out ny), "Error, tendria que poder moverse");
            Assert.IsTrue(nx == 1, "Error: La nueva posX esta mal definida");
            Assert.IsTrue(ny == 3, "Error: La nueva posY esta mal definida");
        }
        [TestMethod]
        public void SiguienteVacioRight()
        {
            int nx, ny;
            Assert.IsTrue(tab.siguiente(1, 4, 0, 1, out nx, out ny), "Error, tendria que poder moverse");
            Assert.IsTrue(nx == 1, "Error: La nueva posX esta mal definida");
            Assert.IsTrue(ny == 5, "Error: La nueva posY esta mal definida");
        }
    }
}
