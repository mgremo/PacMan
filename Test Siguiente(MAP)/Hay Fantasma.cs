﻿using System;
using Pac_Man;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests_MAP
{
    [TestClass]
    public class Hay_Fantasma
    {
        [TestMethod]
        public void HayFantasma()
        {
            //Arrange
            Tablero tab = new Tablero(3, 3);
            int posX = 1, posY = 1;
            tab.setPersonaje(2, posX, posY, -1, 0);
            //Act
            bool hay =tab.hayFantasma(posX, posY);
            //Assert
            Assert.IsTrue(hay, "Error, hay fantasma en esa posicion");
        }
    }
}
