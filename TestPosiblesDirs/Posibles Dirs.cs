using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pac_Man;

namespace TestPosiblesDirs
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void PosiblesDirs3()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(1, 0);
            newL.insertaFin(0, 1);
            newL.insertaFin(-1, 0);
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);
            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(3, cont,"Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for(int i=0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs2()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(1, 0);
            newL.insertaFin(0, 1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);
            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(2, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs1()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);                //  #
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);  //  0
            tab.cambiaCasilla(2, 1, Tablero.Casilla.Muro);  //  #
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(0, 1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);
            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(1, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs0()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            tab.cambiaCasilla(1, 2, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 1, Tablero.Casilla.Muro);
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(0, -1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);

            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(1, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs2Fant()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(1, 0);
            newL.insertaFin(0, 1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);
            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(2, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs1Fant()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);                //  #
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);  //  0
            tab.cambiaCasilla(2, 1, Tablero.Casilla.Muro);  //  #
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(0, 1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);
            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(1, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
        [TestMethod]
        public void PosiblesDirs0Fant()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            tab.setPersonaje(1, 1, 1, 0, 1);
            tab.cambiaCasilla(0, 1, Tablero.Casilla.Muro);
            tab.cambiaCasilla(1, 2, Tablero.Casilla.Muro);
            tab.cambiaCasilla(2, 1, Tablero.Casilla.Muro);
            ListaPares l = new ListaPares();
            //Creamos la lista con las direcciones que tendrian que aparecer
            ListaPares newL = new ListaPares();
            //Añadimos las direcciones que tendria que llevar
            newL.insertaFin(0, -1);
            //Aqui almacenaremos las direcciones
            int dX, dY, ndX, ndY;
            int cont = 0;
            //Act
            tab.posiblesDirs(1, out l, out cont);

            //Assert 
            //Primero comprobamos que solo haya tres direcciones
            Assert.AreEqual(1, cont, "Error, hay mas/menos direcciones");
            //Hay que comprobar que estan añadidas las direcciones
            for (int i = 0; i < cont; i++)
            {
                l.nEsimo(i, out dX, out dY);
                newL.nEsimo(i, out ndX, out ndY);
                Assert.AreEqual(dX, ndX, "Error,la direccion X " + i + " no coincide");
                Assert.AreEqual(dY, ndY, "Error,la direccion Y " + i + " no coincide");
            }
        }
    }
}
