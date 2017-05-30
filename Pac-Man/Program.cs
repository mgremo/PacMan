using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pac_Man
{
    public class Program
    {
        enum MenuOutput {Juega,Salir,Continuar,Guardar,Login }
        static int puntuacion;
        static Usuarios Usuario = null;
        static List<Usuarios> users = new List<Usuarios>();
        
        static void Main(string[] args)
        {
            //Arrancamos la lista de usuarios
            users = Usuarios.cargaUsuarios("users");
            MenuOutput option=MenuOutput.Login;
            while(option == MenuOutput.Login)
            {
                Console.Clear();
                MenuIni(out option);
                if(option == MenuOutput.Login)
                {
                    Console.Clear();
                    Login();
                }
            }
            //Inicializamos las variables
            bool exit = false;
            if (option == MenuOutput.Juega)
            {
                bool next = false;
                bool gameOver = false;
                int i = 1;
                puntuacion = 0;
                //Empezamos el juego
                while (!exit && !gameOver && i < 10)
                {
                    Console.Clear();
                    //Bucle ppal del juego
                    Juega(i, out next,out exit,out gameOver);
                    //Si no sale, esperamos un poco para pasar de nivel
                    if (!exit)
                        System.Threading.Thread.Sleep(3000);    
                    //Y si se pasa el nivel, ponemos el siguiente
                    if (next)
                        i++;
                }
                if (Usuario != null)
                {
                    Usuario.Save(i, puntuacion);
                    SaveUser();
                    Usuarios.vuelcaLista(users, "users");
                }
            }
            Console.ForegroundColor= ConsoleColor.White;
        }
        static void Juega(int level,out bool next,out bool exit,out bool gameOver)
        {

            string nivel = "level0" + level + ".dat"; //Cargamos el nivel correspondiente
            Tablero tab = new Tablero("Niveles/"+nivel);

            //Arrancamos todas las variables
            next = false;
            gameOver = false;
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
            int maxComida = tab.GetComida();
            int puntLocal=0;

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
                    //Actualizamos la puntuacion
                    puntLocal = maxComida - tab.GetComida();

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
                        Vidas(fils, vidas,puntLocal);
                        tab.BorraPers();
                        for (int i = 0; i < tab.pers.Length; i++)
                        {
                            tab.pers[i].posX = tab.pers[i].defX;
                            tab.pers[i].posY = tab.pers[i].defY;
                        }
                        if(vidas>0)
                            Captura(fils, cols, tab);
                        tab.DibujaPers();
                    }
                    //Dibujamos
                    tab.DibujaPers();
                    Vidas(fils, vidas,puntLocal);
                    UserDisplay(fils, level);
                    Console.SetCursorPosition(0, fils + 2);
                    System.Threading.Thread.Sleep(lap);
                }
            }
            if (!exit)
            {
                if (vidas <= 0)
                {
                    GameOver(fils, cols);
                    gameOver = true;
                }
                else
                {
                    Win(fils, cols);
                    
                }
                puntuacion += puntLocal;
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
        static void Vidas(int fils, int vidas,int score)
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
            Console.SetCursorPosition(18, fils + 1);
            Console.Write("Score: " + (puntuacion + score));
            Console.SetCursorPosition(0, fils + 2);
            Console.ForegroundColor = ConsoleColor.Black;
        }
        static void MenuIni(out MenuOutput option)
        {
            //Dibujamos el header del titulo pac-man
            Tablero tab = new Tablero("Menus/Header.dat");
            tab.DibujaMenu();
            option = MenuOutput.Juega;
            int indice = 0;
            //Dibujamos las opciones
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(30, 6);
            Console.WriteLine(" JUGAR");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(30, 8);
            Console.WriteLine(" LOGIN");
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
                    option = MenuOutput.Login;
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
            Tablero tab = new Tablero("Menus/Pausa.dat");
            tab.DibujaMenu();
            option = MenuOutput.Juega;
            int indice = 0;
            int margen = 15;
            //Dibujamos las opciones
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(margen, 7);
            Console.WriteLine(" CONTINUAR");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(margen, 9);
            Console.WriteLine(" SALIR");
            //Y dibujamos el cursor
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(margen-1, 2 * indice + 7);
            Console.Write(">");
            Console.SetCursorPosition(margen +11, 2 * indice + 7);
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
                    Console.SetCursorPosition(margen-1, 2 * indice + 7);
                    Console.Write(" ");
                    Console.SetCursorPosition(margen+11, 2 * indice + 7);
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
                    Console.SetCursorPosition(margen-1, 2 * indice + 7);
                    Console.Write(">");
                    Console.SetCursorPosition(margen+11, 2 * indice + 7);
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
        static void Captura(int fils, int cols,Tablero tab)
        {

            Console.SetCursorPosition(0, fils + 2);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(cols / 2-3, fils / 2-1);
            Console.Write("      ");
            Console.SetCursorPosition(cols / 2-3, fils / 2);
            Console.Write("      ");
            Console.SetCursorPosition(cols / 2-3, fils / 2+1);
            Console.Write("      ");
            for (int i = 3; i > 0; i--)
            {
                Console.SetCursorPosition(cols / 2, fils / 2);
                Console.Write(i);
                Console.SetCursorPosition(0, fils + 2);
                System.Threading.Thread.Sleep(1000);
            }
            for (int i = fils/2-1;i < fils/2+2; i++)
            {
                for (int j = cols/2-3; j < cols/2+3; j++)
                {
                    tab.DibujaCasilla(i,j);
                }
            }
            
            Console.SetCursorPosition(0, fils + 2);
        }
        static void Login()
        {
            //Dibujamos el header del titulo pac-man
            Tablero tab = new Tablero("Menus/Header.dat");
            tab.DibujaMenu();
            //Ahora le pedimos al informacion al jugador
            Console.SetCursorPosition(1, 7);
            Console.Write("Nombre de usuario? : ");
            string name = Console.ReadLine();
            int i = 0;
            //Buscamos si existe
            while (i < users.Count && users[i].nombre != name)
                i++;
            if (i < users.Count)
            {
                //En cuyo caso lo cargamos
                Usuario = users[i];
                Console.Write(" Bienvenido, " + name);
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                //Y si no, se lo informamos al usuario
                Console.Write("Lo sentimos, ese usuario no esta registrado, regresando al menu principal...");
                System.Threading.Thread.Sleep(2000);
            }
        }
        static void UserDisplay(int fils,int level)
        {
            int margen = fils + 3;
            if (Usuario != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(margen-2, 1);
                Console.Write("--USUARIO--");
                Console.SetCursorPosition(margen, 2);
                Console.Write(Usuario.nombre.ToUpper());
                Console.SetCursorPosition(margen, 3);
                Console.Write("Level:" +level);
                Console.SetCursorPosition(margen, 4);
                Console.Write("Record:" + Usuario.Score());

            }
        }
        static void SaveUser()
        {
            int i = 0;
            while (i < users.Count && users[i].nombre != Usuario.nombre)
                i++;
            if (i < users.Count)
            {
                users[i] = Usuario;
            }
        }
    }
}
