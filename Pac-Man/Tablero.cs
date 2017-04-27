using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
    class Tablero
    {
        // dimensiones del tablero
        int FILS, COLS;
        // contenido de las casillas
        enum Casilla { Blanco, Muro, Comida, Vitamina, MuroCelda };
        // matriz de casillas (tablero)
        Casilla[,] cas;
        // representacion de los personajes (Pacman y fantasmas)
        struct Personaje
        {
            public int posX, posY; // posicion del personaje
            public int dirX, dirY; // direccion de movimiento
            public int defX, defY; // posiciones de partida por defecto
        }
        // vector de personajes, 0 es Pacman y el resto fantasmas
        Personaje[] pers;
        // colores para los personajes
        ConsoleColor[] colors = { ConsoleColor.DarkYellow, ConsoleColor.Red,
            ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.DarkBlue };
        int lapFantasmas; // tiempo de retardo de salida del los fantasmas
        int numComida; // numero de casillas retantes con comida o vitamina
        int numNivel; // nivel actual de juego
                      // generador de numeros aleatorios para el movimiento de los fantasmas
        Random rnd;
        // flag para mensajes de depuracion en consola
        private bool Debug = true;

    }
}
