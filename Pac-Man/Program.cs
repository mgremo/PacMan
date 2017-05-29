using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
    public class Program
    {
        enum MenuOutput {Juega,Salir,Continuar,Guardar,Cargar }
        static void Main(string[] args)
        {
            MenuOutput option;
            MenuIni(out option);
            //Inicializamos las variables
            bool exit = false;
            if (option == MenuOutput.Juega)
            {
                bool next = false;
                int i = 1;
                //Empezamos el juego
                while (!exit&& i < 10)
                {
                    Console.Clear();
                    //Bucle ppal del juego
                    Juega(i, out next,out exit);
                    //Si no sale, esperamos un poco para pasar de nivel
                    if (!exit)
                        System.Threading.Thread.Sleep(3000);
                    //Y si se pasa el nivel, ponemos el siguiente
                    if (next)
                        i++;
                }
            }
            Console.ForegroundColor= ConsoleColor.White;
        }
        static void Juega(int level,out bool next,out bool exit)
        {

            string nivel = "level0" + level + ".dat"; //Cargamos el nivel correspondiente
            Tablero tab = new Tablero(nivel);

            //Arrancamos todas las variables
            next = false;
            MenuOutput menu;
            char c = ' ';
            int vidas = 5;
            bool captura = false;
            int lap = 100;
            tab.SetLap(3000);
            int lapFantAct = tab.GetLap();
            bool FlagFant = false;
            exit=false;
            int fils, cols;
            tab.getDims(out fils, out cols);
            //Dibujamos el tablero
            tab.Dibuja();
            //E iniciamos el juego
            while (!exit&&!tab.finNivel()&&vidas>0)
            {

                //Primero quitamos a los personajes
                tab.BorraPers();
                //Ahora leemos input
                tab.leeInput(ref c);
                if (c == 'p')
                {
                    MenuPausa(out menu);
                    //Menu de pausa
                    if (menu == MenuOutput.Continuar)
                    {
                        Console.Clear();
                        tab.Dibuja();
                        tab.DibujaPers();
                    }
                    else
                        exit = true;
                }
                if (!exit)
                {
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
                        Console.SetCursorPosition(0, fils + 2);
                        Console.Write("                          ");

                    }
                    //Dibujamos
                    tab.DibujaPers();
                    Vidas(fils, vidas);


                    System.Threading.Thread.Sleep(lap);
                }
            }
            if (!exit)
            {
                if (vidas <= 0)
                {
                    GameOver(fils, cols);
                }
                else
                {
                    Win(fils, cols);
                }
                next = !captura;
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
        static void MenuIni(out MenuOutput option)
        {
            //Dibujamos el header del titulo pac-man
            Tablero tab = new Tablero("Header.dat");
            tab.DibujaMenu();
            option = MenuOutput.Cargar;
            int indice = 0;
            //Dibujamos las opciones
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(30, 6);
            Console.WriteLine(" JUGAR");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(30, 8);
            Console.WriteLine(" CARGAR");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(30, 10);
            Console.WriteLine(" SALIR");
            //Y dibujamos el cursor
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(29, 2 * indice + 6);
            Console.Write(">");
            Console.SetCursorPosition(38, 2 * indice + 6);
            Console.Write("<");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 12);
            string input = "";
            while (input != "Enter")
            {
                input = Console.ReadKey().Key.ToString();
                while (Console.KeyAvailable)
                    Console.ReadKey();
                if (input == "UpArrow" || input == "DownArrow")
                {
                    //Si el usuario cambia la opcion, primero borramos los marcadores actuales
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(29, 2 * indice + 6);
                    Console.Write(" ");
                    Console.SetCursorPosition(38, 2 * indice + 6);
                    Console.Write(" ");
                    //Luego, cambiamos el indice (Con forma toroidal!)
                    if (input == "UpArrow")
                    {
                        indice--;
                        if (indice < 0)
                            indice = 2;
                    }
                    else
                    {
                        indice++;
                        if (indice >2)
                            indice = 0;
                    }
                    //Y dibujamos el cursor
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(29, 2 * indice + 6);
                    Console.Write(">");
                    Console.SetCursorPosition(38, 2 * indice + 6);
                    Console.Write("<");
                    Console.SetCursorPosition(0, 12);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            switch (indice)
            {
                case 0:
                    option = MenuOutput.Juega;
                    break;
                case 1:
                    option = MenuOutput.Juega;
                    break;
                case 2:
                    option = MenuOutput.Salir;
                    break;
            }
        }
        static void MenuPausa(out MenuOutput option)
        {
            Console.Clear();
            //Dibujamos el header del menu
            Tablero tab = new Tablero("Pausa.dat");
            tab.DibujaMenu();
            option = MenuOutput.Cargar;
            int indice = 0;
            int margen = 15;
            //Dibujamos las opciones
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(margen, 6);
            Console.WriteLine(" CONTINUAR");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(margen, 8);
            Console.WriteLine(" SALIR");
            //Y dibujamos el cursor
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(margen-1, 2 * indice + 6);
            Console.Write(">");
            Console.SetCursorPosition(margen +11, 2 * indice + 6);
            Console.Write("<");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 12);
            string input = "";
            while (input != "Enter")
            {
                input = Console.ReadKey().Key.ToString();
                while (Console.KeyAvailable)
                    Console.ReadKey();
                if (input == "UpArrow" || input == "DownArrow")
                {
                    //Si el usuario cambia la opcion, primero borramos los marcadores actuales
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(margen-1, 2 * indice + 6);
                    Console.Write(" ");
                    Console.SetCursorPosition(margen+11, 2 * indice + 6);
                    Console.Write(" ");
                    //Luego, cambiamos el indice (Con forma toroidal!)
                    if (input == "UpArrow")
                    {
                        indice--;
                        if (indice < 0)
                            indice = 1;
                    }
                    else
                    {
                        indice++;
                        if (indice > 1)
                            indice = 0;
                    }
                    //Y dibujamos el cursor
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(margen-1, 2 * indice + 6);
                    Console.Write(">");
                    Console.SetCursorPosition(margen+11, 2 * indice + 6);
                    Console.Write("<");
                    Console.SetCursorPosition(0, 12);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            switch (indice)
            {
                case 0:
                    option = MenuOutput.Continuar;
                    break;
                case 1:
                    option = MenuOutput.Salir;
                    break;
            }
        }
    }
}
