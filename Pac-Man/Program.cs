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
            int i = 1;
            while (i < 10)
            {
                Console.Clear();
                Juega(i);
                Console.ReadLine();
                i++;
            }
        }
        static void Juega(int level)
        {
            string nivel = "level0" + level + ".dat";
            Tablero tab = new Tablero(nivel);
            tab.Dibuja();
            char c = ' ';
            bool captura = false;
            int lap = 100;
            tab.SetLap(3000);
            int lapFantAct = 3000;
            bool FlagFant = false;
            while (!tab.finNivel() && !captura)
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

                captura = tab.captura();

                tab.mueveFantasma(0);
                if (!captura)
                    captura = tab.captura();
                //tab.Dibuja();
                tab.DibujaPers();

                System.Threading.Thread.Sleep(lap);
            }
            if (captura)
            {
                int fils, cols;
                tab.getDims(out fils, out cols);
                GameOver(fils, cols);
            }
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
    }
}
