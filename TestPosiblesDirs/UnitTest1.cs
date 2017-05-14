using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pac_Man;

namespace TestPosiblesDirs
{
    [TestClass]
    public class UnitTest1
    {
        Tablero tab = new Tablero("test.dat");

        [TestMethod]
        public void PosiblesDirs0()
        {
            int cont = 0;
            ListaPares l = new ListaPares();
            tab.posiblesDirs(1, out l, out cont);
            Assert.AreEqual(0, cont);
        }
        [TestMethod]
        public void PosiblesDirs1()
        {
            int cont = 0;
            ListaPares l = new ListaPares();
            tab.posiblesDirs(2, out l, out cont);
            Assert.AreEqual(1, cont);
        }
        [TestMethod]
        public void PosiblesDirs2()
        {
            int cont = 0;
            ListaPares l = new ListaPares();
            tab.posiblesDirs(3, out l, out cont);
            Assert.AreEqual(2, cont);
        }
        [TestMethod]
        public void PosiblesDirs3()
        {
            int cont = 0;
            ListaPares l = new ListaPares();
            tab.posiblesDirs(4, out l, out cont);
            Assert.AreEqual(3, cont);
        }
        [TestMethod]
        public void PosiblesDirs4()
        {
            int cont = 0;
            ListaPares l = new ListaPares();
            tab.posiblesDirs(0, out l, out cont);
            Assert.AreEqual(4, cont);
        }
    }
}
