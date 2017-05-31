using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pac_Man
{
    public class Program
    {
        enum MenuOutput {Juega,Salir,Continuar,Guardar,Login,Crear,Cargar }
        static int puntuacion;
        static Usuarios Usuario = null;
        static List<Usuarios> users = new List<Usuarios>();
        
        static void Main(string[] args)
        {
            int level = 1;
            //LevelCreator();
            //Arrancamos la lista de usuarios
            users = Usuarios.cargaUsuarios("usuarios");
            MenuOutput option=MenuOutput.Login;
            while(option == MenuOutput.Login)
            {
                Console.Clear();
                MenuIni(out option);
                if (option == MenuOutput.Login)
                {
                    Console.Clear();
                    Login();
                }
                else if (option == MenuOutput.Crear)
                {
                    Console.Clear();
                    Console.ResetColor();
                    LevelCreator();
                }
                    
                else if (option == MenuOutput.Cargar)
                {
                    option = MenuOutput.Juega;
                    level = 10;
                }
                   
            }
            //Inicializamos las variables
            bool exit = false;
            if (option == MenuOutput.Juega)
            {
                bool next = false;
                bool gameOver = false;
                puntuacion = 0;
                //Empezamos el juego
                while (!exit && !gameOver && level < 11)
                {
                    Console.Clear();
                    //Bucle ppal del juego
                    Juega(level, out next,out exit,out gameOver);
                    //Si no sale, esperamos un poco para pasar de nivel
                    if (!exit)
                        System.Threading.Thread.Sleep(3000);    
                    //Y si se pasa el nivel, ponemos el siguiente
                    if (next)
                        level++;
                }
                if (Usuario != null)
                {
                    Usuario.Save(level, puntuacion);
                    SaveUser();
                    Usuarios.vuelcaLista(users, "users");
                }
                
            }
            Console.Clear();
            ScoreBoard();
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
                    UserDisplay(fils, level,puntLocal+puntuacion);
                    Console.SetCursorPosition(0, fils + 2);
                    System.Threading.Thread.Sleep(lap);
                }
            }
            puntuacion += puntLocal + vidas * 50;
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
                
                next = !captura;
            }
            
        }
        static void GameOver(int fils, int cols)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 - 1);
            Console.Write("             ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2);
            Console.Write(" GAME OVER   ");
            Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 1);
            Console.Write("             ");
            if (Usuario != null)
            {
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 2);
                Console.Write("             ");
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 2);
                Console.Write(" RECORD:" + Math.Max(puntuacion, Usuario.Score()));
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 3);
                Console.Write("             ");
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 4);
                Console.Write("             ");
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 4);
                Console.Write(" SCORE:"+puntuacion);
                Console.SetCursorPosition(cols / 2 - 5, fils / 2 + 5);
                Console.Write("             ");
            }
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
            Console.SetCursorPosition(30, 7);
            Console.WriteLine(" JUGAR");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(30, 9);
            Console.WriteLine(" LOGIN");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(30, 11);
            Console.WriteLine(" CEAR");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(30, 13);
            Console.WriteLine(" CARGAR");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(30, 15);
            Console.WriteLine(" SALIR");
            //Y dibujamos el cursor
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(29, 2 * indice + 7);
            Console.Write(">");
            Console.SetCursorPosition(38, 2 * indice + 7);
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
                    Console.SetCursorPosition(29, 2 * indice + 7);
                    Console.Write(" ");
                    Console.SetCursorPosition(38, 2 * indice + 7);
                    Console.Write(" ");
                    //Luego, cambiamos el indice (Con forma toroidal!)
                    if (input == "UpArrow")
                    {
                        indice--;
                        if (indice < 0)
                            indice = 4;
                    }
                    else
                    {
                        indice++;
                        if (indice >4)
                            indice = 0;
                    }
                    //Y dibujamos el cursor
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(29, 2 * indice + 7);
                    Console.Write(">");
                    Console.SetCursorPosition(38, 2 * indice + 7);
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
                    option = MenuOutput.Crear;
                    break;
                case 3:
                    option = MenuOutput.Cargar;
                    break;
                case 4:
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
            //Pintamos el cartel con el temporizador
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
            //Devolvemos las casillas originales
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
                Console.Write("Lo sentimos, ese usuario no esta registrado, Desea añadir ese usuario?");
                string c = "";
                while (c != "s" && c != "n")
                {
                    Console.Write("\n[s] -Si\n[n] - No\nTu opcion: ");
                    c= Console.ReadKey().KeyChar.ToString();
                }
                if (c == "s")
                {
                    Console.Write("\nCreando . . .");
                    users.Add(new Usuarios(name, 0, 0));
                    Usuario = users.Last<Usuarios>();
                    System.Threading.Thread.Sleep(2000);
                    Console.Write("\nUsuario creado correctamente, regresando al menu . . .");
                    System.Threading.Thread.Sleep(2000);
                    Usuarios.vuelcaLista(users,"users");
                }
                else
                {
                    Console.Write("\nSaliendo. . .");
                    System.Threading.Thread.Sleep(2000);
                }
                    
            }
        }
        static void UserDisplay(int fils,int level,int score)
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
                Console.Write("Level:" +Math.Max(Usuario.Level(), level));
                Console.SetCursorPosition(margen, 4);
                Console.Write("Record:" + Math.Max(score, Usuario.Score()));

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
        static void ScoreBoard()
        {
            //Dibujamos el header del scoreboard
            Tablero tab = new Tablero("Menus/Score.dat");
            tab.DibujaMenu();
            Usuarios.Sort(ref users);
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i <users.Count; i++)
            {
                Console.SetCursorPosition(0, 7+2*i);
                Console.Write("-{0}: {1} ", i+1,users[i].nombre);
                Console.SetCursorPosition(12, 7 + 2 * i);
                Console.Write("| Level: {0} | Score: {1} ", users[i].Level(), users[i].Score());
            }
            Console.SetCursorPosition(0, users.Count * 2 + 8);
            Usuarios.vuelcaLista(users, "users");
        }
        static void LevelCreator()
        {
            Console.WriteLine("Introduzca el tamaño del tablero [fils] x [cols]: Max (50 x 50)");
            bool valido = false;
            int fils=0, cols=0;
            while (!valido)
            {
                Console.Write("Fils: ");
                valido = int.TryParse(Console.ReadLine(), out fils)&&fils!=0&&fils<51;
                if (valido)
                {
                    Console.Write("Cols: ");
                    valido = int.TryParse(Console.ReadLine(), out cols) && cols != 0&&cols<51;
                }
                if (!valido)
                    Console.WriteLine("Datos no validos, introduzcalos de nuevo");
            }

            Tablero tab = new Tablero(fils, cols);
            MenuOutput menu = MenuOutput.Juega;

            //Setup
            int cY=0, cX=0;
            Console.Clear();
            tab.Dibuja();
            tab.DibujaBordes();
            for (int i = 0; i < tab.pers.Length; i++)
            {
                tab.setPersonaje(i,0, cols + i + 1,0,0);
            }
            tab.DibujaPers();
            Console.SetCursorPosition(0, fils+2);
            Console.Write(@"1 - Muro 
2 - Blanco
3 - Muro Fant
4 - Comida
5 - Vitamina
6, 7, 8, 9 - Fantasmas
0 - Pacman
Esc - Salir sin guardar
Enter - Salir y guardar
Return - Borrar");
            Console.SetCursorPosition(0, 0);
            //Menu Principal
            while (menu!=MenuOutput.Salir && menu != MenuOutput.Guardar)
            {
                Console.SetCursorPosition(cY, cX);
                char c = LeeInput();
                /*
                 * 1-Muro 
                 * 2-Blanco
                 * 3-Muro Fant
                 * 4-Comida
                 * 5-Vitamina
                 * 6,7,8,9 - Fantasmas
                 * 0 -Pacman
                */
                if(c!=' ')
                    CambiaCas(ref tab,ref cX,ref cY, c,out menu);
                if (c != ' ' && c != 'u' && c != 'd' && c != 'l' && c != 'r')
                {
                    
                }
                tab.BorraPers();
                tab.DibujaCasilla(cX,cY);
                tab.DibujaPers();

                Console.SetCursorPosition(cY, cX);
                
                System.Threading.Thread.Sleep(100);
            }
            if(menu == MenuOutput.Guardar)
            {
                Console.Write("Introduzca el numero del nivel (Mayor que 9): ");
                //Si el jugador se equivoca o introduce mal el dato lo guardaremos en el archivo lvl 00 (Reservado para copia de seguridad)
                int level = 0;
                int.TryParse(Console.ReadLine(), out level);
                tab.guarda(level);
            }  
        }
        static char LeeInput()
        {
            char c = ' ';
            if (Console.KeyAvailable)
            {
                string key = Console.ReadKey(true).Key.ToString();
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
                switch (key)
                {
                    /*
                     * 1-Muro
                     * 2-Blanco
                     * 3-Muro Fant
                     * 4-Comida
                     * 5-Vitamina
                     * 6,7,8,9 - Fantasmas
                     * 0 -Pacman
                     * */
                    case "UpArrow":
                        c = 'u';
                        break;
                    case "DownArrow":
                        c = 'd';
                        break;
                    case "LeftArrow":
                        c = 'l';
                        break;
                    case "RightArrow":
                        c = 'r';
                        break;
                    case "D1":
                        c = '1';
                        break;
                    case "D2":
                        c = '2';
                        break;
                    case "D3":
                        c = '3';
                        break;
                    case "D4":
                        c = '4';
                        break;
                    case "D5":
                        c = '5';
                        break;
                    case "D6":
                        c = '6';
                        break;
                    case "D7":
                        c = '7';
                        break;
                    case "D8":
                        c = '8';
                        break;
                    case "D9":
                        c = '9';
                        break;
                    case "D0":
                        c = '0';
                        break;
                    case "Escape":
                        c = 'e';
                        break;
                    case "Enter":
                        c = 'q';
                        break;
                    case "Backspace":
                        c = 'b';
                        break;
                    default:
                        c = ' ';
                        break;
                }
            }
            
            return c;
        }
        static void CambiaCas(ref Tablero tab,ref int cX,ref int cY,char c, out MenuOutput menu)
        {
            int fils, cols;
            tab.getDims(out fils, out cols);
            menu = MenuOutput.Juega;
            switch (c)
            {
                case 'u':
                    cX--;
                    if (cX < 0)
                        cX = fils - 1;
                    break;
                case 'd':
                    cX++;
                    if (cX >=fils)
                        cX = 0;
                    break;
                case 'l':
                    cY--;
                    if (cY < 0)
                        cY = fils - 1;
                    break;
                case 'r':
                    cY++;
                    if (cY >= fils)
                        cY = 0;
                    break;
                case '1':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.Muro);
                    break;
                case '2':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.Blanco);
                    break;
                case '3':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.MuroCelda);
                    break;
                case '4':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.Comida);
                    break;
                case '5':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.Vitamina);
                    break;
                case '6': case '7': case '8': case '9':
                    tab.cambiaCasilla(cX,cY,Tablero.Casilla.Blanco);
                    int n = int.Parse(c.ToString())-5;
                    tab.BorraPers();
                    tab.setPersonaje(n, cX, cY, 0, 0);
                    break;
                case '0':
                    tab.cambiaCasilla(cX, cY, Tablero.Casilla.Blanco);
                    tab.BorraPers();
                    tab.setPersonaje(0, cX, cY, 0, 0);
                    break;
                case 'e':
                    menu = MenuOutput.Salir;
                    break;
                case 'q':
                    menu = MenuOutput.Guardar;
                    break;
                case 'b':
                    tab = new Tablero(fils, cols);
                    Console.SetCursorPosition(0, 0);
                    tab.Dibuja();
                    break;
            }
        }
    }
}
