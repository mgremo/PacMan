//Miguel Angel Gremo    
//Hector Marcos Rabadán
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        Personaje[] pers;       //pacaman y los fantasmicos

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
        Tablero(string file)
        {
            getDims(file);


        }
        private void getDims(string file)
        {
            COLS = FILS = 0;
            string line;
            StreamReader level = new StreamReader(file);
            line = level.ReadLine(); //Leemos la primera linea y vemos en su longitud el numero de columnas
            COLS = line.Length;
            if (COLS > 1)
            {
                do
                {
                    FILS++;
                } while (level.ReadLine() !="" && level.ReadLine().Length == COLS);
            }
        }
        public void Dibuja()
        {
            for (int i = 0; i < FILS; i++)
            {
                for(int j = 0; j < COLS; j++)
                {
                    switch (cas[i, j])
                    {
                        case Casilla.Blanco:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(" ");
                            break;
                        case Casilla.Comida:
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("·");
                            break;
                        case Casilla.Muro:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" ");
                            break;
                        case Casilla.MuroCelda:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            break;
                        case Casilla.Vitamina:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.Write("*");
                            break;
                        default:break;
                    }
                    Console.BackgroundColor = ConsoleColor.Black;       //devolvemos los colores originales por si acaso
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            if (Debug)
            {
                for (int a = 0; a < pers.Length; a++)
                {
                    if (a == 0)
                    {
                        Console.Write("Posicion de Pacman: {0}(X), {1}(Y) ", pers[a].posX, pers[a].posY);
                        Console.WriteLine("Direccion de Pacman: ({0},{1}).", pers[a].dirX, pers[a].dirY);
                    }
                    else
                    {
                        Console.Write("Posicion del fantasma{2}: {0}(X), {1}(Y) ", pers[a].posX, pers[a].posY, a);
                        Console.WriteLine("Direccion del fantasma{2}: ({0},{1}).", pers[a].dirX, pers[a].dirY, a);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
