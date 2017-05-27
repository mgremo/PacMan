using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Inicializamos las variables
            bool next = false;
            int i = 1;
            //Empezamos el juego
            while (i < 10)
            {
                Console.Clear();
                //Bucle ppal del juego
                Juega(i,out next);
                Console.ReadLine();
                //Y si se pasa el nivel, ponemos el siguiente
                if(next)
                    i++;
            }
        }
        static void Juega(int level,out bool next)
        {

            string nivel = "level0" + level + ".dat"; //Cargamos el nivel correspondiente
            Tablero tab = new Tablero(nivel);

            //Arrancamos todas las variables
            char c = ' ';
            int vidas = 5;
            bool captura = false;
            int lap = 100;
            tab.SetLap(3000);
            int lapFantAct = tab.GetLap();
            bool FlagFant = false;
            int fils, cols;
            tab.getDims(out fils, out cols);
            //Dibujamos el tablero
            tab.Dibuja();
            //E iniciamos el juego
            while (!tab.finNivel()&&vidas>0)
            {

                //Primero quitamos a los personajes
                tab.BorraPers();
                //Ahora leemos input
                tab.leeInput(ref c);
                //Ahora miramos a ver si podemos cambiar de direccion, en cuyo caso limpiamos el buffer
                if (c != ' ' && tab.cambiaDir(c)) c = ' ';

                if (lapFantAct > 0)
                    lapFantAct -= lap;
                else if (lapFantAct <= 0 && !FlagFant)
                {
                    tab.eliminaMuroFant();
                    FlagFant = true;
                }

                //Y movemos a pacman
                tab.muevePacman();
                //Vemos si se ha chocado con un fantasma
                captura = tab.captura();
                //Movemos al fantasma
                tab.mueveFantasma(0);
                //Y si pacman no habia chocado con el, vemos si ha chocado el con pacman
                if (!captura)
                    captura = tab.captura();
                //Finalmente, si hemos colisionado, quitamos vida a pacman
                if (captura)
                {
                    vidas--;
                    Vidas(fils, vidas);
                    tab.BorraPers();
                    for (int i = 0; i < tab.pers.Length; i++)
                    {
                        tab.pers[i].posX = tab.pers[i].defX;
                        tab.pers[i].posY = tab.pers[i].defY;
                    }
                    tab.DibujaPers();
                    Console.SetCursorPosition(0, fils + 2);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Pulsa enter para empezar");
                    Console.SetCursorPosition(0, fils + 3);
                    Console.ReadLine();
                    Console.SetCursorPosition(0, fils+2);
                    Console.Write("                          ");

                }
                //Dibujamos
                tab.DibujaPers();
                Vidas(fils,vidas);
                    

                System.Threading.Thread.Sleep(lap);
            }
           
            if (vidas<=0)
            {
                GameOver(fils, cols);
            }
            else
            {
                Win(fils, cols);
            }
            next = !captura;
        }
        static void GameOver(int fils,int cols)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 - 1);
            Console.Write("           ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2);
            Console.Write(" GAME OVER ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 1);
            Console.Write("           ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(cols, fils);
        }
        static void Win(int fils, int cols)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 - 1);
            Console.Write("           ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2);
            Console.Write("    WIN!   ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 1);
            Console.Write("           ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(cols, fils);
        }
        static void Vidas(int fils, int vidas)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, fils+1);
            Console.Write("                                         ");
            Console.SetCursorPosition(0, fils+1);
            Console.Write("Vidas: ");
            for (int i = 0; i < vidas; i++)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
            }
            Console.SetCursorPosition(0, fils + 2);
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
