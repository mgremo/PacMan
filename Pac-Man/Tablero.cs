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
        public Tablero(string file)
        {
            //Vamos a inicializar el random (En debug tendra una seed)
            if (Debug)
                rnd = new Random(100);
            else
                rnd = new Random();

            StreamReader level = new StreamReader(file);
            getDims(file); //Esto pone FILS y COLS bien
            cas = new Casilla[FILS, COLS];
            string line;
            for (int i = 0; i < FILS; i++)
            {
                line = level.ReadLine();
                for (int j = 0; j < COLS; j++)
                {
                    switch (line[j * 2])
                    {
                        case '0':
                            cas[i, j] = Casilla.Blanco;
                            break;
                        case '1':
                            cas[i, j] = Casilla.Muro;
                            break;
                        case '2':
                            cas[i, j] = Casilla.Comida;
                            break;
                        case '3':
                            cas[i, j] = Casilla.Vitamina;
                            break;
                        case '4':
                            cas[i, j] = Casilla.MuroCelda;
                            break;
                        case '5': case '6': case '7': case '8':
                            cas[i, j] = Casilla.Blanco;
                            int n = int.Parse(line[j * 2].ToString()) - 4;
                            pers[n].posX = pers[n].defX = i;
                            pers[n].posY = pers[n].defY = j;
                            pers[n].dirX = pers[n].dirY = 0; //Por defecto la direccion sera 0,0 , no válida, pero en el siguiente update se corrige
                            break;
                        case '9':
                            cas[i, j] = Casilla.Blanco;
                            pers[0].posX = pers[0].defX = i;
                            pers[0].posY = pers[0].defY = j;
                            pers[0].dirX = pers[0].dirY = 0; //Por defecto la direccion sera 0,0 , no válida, pero en el siguiente update se corrige
                            break;
                    }
                }

            }

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
            COLS = (COLS + 1) / 2;
            level.Close();
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
