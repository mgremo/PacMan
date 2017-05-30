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
    public class Tablero
    {
        // dimensiones del tablero
        int FILS, COLS;

        // contenido de las casillas
        public enum Casilla { Blanco, Muro, Comida, Vitamina, MuroCelda };

        //Direcciones
        ListaPares MurosFant = new ListaPares();
        // matriz de casillas (tablero)
        Casilla[,] cas;

        // representacion de los personajes (Pacman y fantasmas)
        public struct Personaje
        {
            public int posX, posY; // posicion del personaje
            public int dirX, dirY; // direccion de movimiento
            public int defX, defY; // posiciones de partida por defecto
        }
        // vector de personajes, 0 es Pacman y el resto fantasmas

        public Personaje[] pers;       //pacaman y los fantasmicos

        // colores para los personajes
        ConsoleColor[] colors = { ConsoleColor.Yellow, ConsoleColor.Red,
            ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.DarkBlue };
        int lapFantasmas=3000; // tiempo de retardo de salida del los fantasmas
        int numComida=0; // numero de casillas retantes con comida o vitamina
        int numNivel; // nivel actual de juego
                      // generador de numeros aleatorios para el movimiento de los fantasmas
        Random rnd;
        // flag para mensajes de depuracion en consola
        private bool Debug =! true; 
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
            pers = new Personaje[5];
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
                            numComida++;
                            break;
                        case '3':
                            cas[i, j] = Casilla.Vitamina;
                            numComida++;
                            break;
                        case '4':
                            cas[i, j] = Casilla.MuroCelda;
                            MurosFant.insertaFin(i, j); //Tambien guardamos la pos de los muros
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
            level.Close();
        } //Constructora a partir de un archivo

        #region Código para tests de unidad

        /// <summary>
        /// Crea un nuevo tablero de casillas vacías de las dimensiones indicadas.
        /// Crea también el array de personajes (vacío)
        /// </summary>
        /// <param name="nFils">Número de filas del tablero</param>
        /// <param name="nCols">Número de columnas del tablero</param>
        public Tablero(int nFils, int nCols)
        {
            cas = new Casilla[nFils, nCols];
            COLS = nCols;
            FILS = nFils;
            pers = new Personaje[5];
            for (int i = 0; i < nFils; i++)
                for (int j = 0; j < nCols; j++)
                {
                    cas[i, j] = Casilla.Blanco;
                }
        }

        /// <summary>
        /// Cambia el tipo de una casilla del tablero
        /// </summary>
        /// <param name="fila">Fila de la casilla</param>
        /// <param name="columna">Columna de la casilla</param>
        /// <param name="tipoCasilla">Tipo que se quiere poner en la casilla</param>
        public void cambiaCasilla(int fila, int columna, Casilla tipoCasilla)
        {
            cas[fila, columna] = tipoCasilla;
        }

        /// <summary>
        /// Establece el número de comidas del tablero
        /// </summary>
        /// <param name="actComida">Número de comidas que queremos que haya en el tablero</param>
        public void setNumComida(int actComida)
        {
            numComida = actComida;
        }

        /// <summary>
        /// Consulta el número de comidas que hay en el tablero
        /// </summary>
        /// <returns>El número de comidas del tablero</returns>
        public int getNumComida()
        {
            return numComida;
        }

        /// <summary>
        /// Establece la posición y dirección de un personaje en el tablero,
        /// representado por la posición que ocupa en el array de personajes.
        /// Recuerda que el 0 es pacman y el resto [1,4] son fantasmas
        /// </summary>
        /// <param name="nPersonaje">Posición que ocupa el personaje a establecer</param>
        /// <param name="posX">Coordenada X</param>
        /// <param name="posY">Coordenada Y</param>
        /// <param name="dirX">Dirección X</param>
        /// <param name="dirY">Dirección Y</param>
        public void setPersonaje(int nPersonaje, int posX, int posY, int dirX, int dirY)
        {
            pers[nPersonaje].posX = posX;
            pers[nPersonaje].posY = posY;
            pers[nPersonaje].dirX = dirX;
            pers[nPersonaje].dirY = dirY;
        }

        /// <summary>
        /// Devuelve uno de los personajes del tablero,
        /// representado por la posición que ocupa en el array de personajes.
        /// Recuerda que el 0 es pacman y el resto [1,4] son fantasmas
        /// </summary>
        /// <param name="nPersonaje">Posición que ocupoa el personaje</param>
        /// <returns>El personaje que hay en la posición indicada</returns>
        public Personaje getPersonaje(int nPersonaje)
        {
            return pers[nPersonaje];
        }

        #endregion

        public void SetLap(int lap) { lapFantasmas = lap; }
        public int GetLap()
        {
            return lapFantasmas;
        }
        public int GetComida() { return numComida; }
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
                    line = level.ReadLine();
                } while ( line!="" && line.Length == COLS);
            }
            if (line != "")
                try
                {
                    numNivel = int.Parse(line);
                }
                catch
                {
                    throw new Exception("Error al guardar el numero del nivel");
                }
            COLS = (COLS + 1) / 2;
            level.Close();
        } //Metodo auxiliar para la constructora
        public void Dibuja()
        {
            for (int i = 0; i < FILS; i++)
            {
                for(int j = 0; j < COLS; j++)
                {
                    switch (cas[i, j])
                    {
                        case Casilla.Blanco:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write(" ");
                            break;
                        case Casilla.Comida:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("·");
                            break;
                        case Casilla.Muro:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write(" ");
                            break;
                        case Casilla.MuroCelda:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            break;
                        case Casilla.Vitamina:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("*");
                            break;
                        default:break;
                    }
                    
                }
                
                Console.WriteLine();
            }
            DibujaPers();
            Console.SetCursorPosition(0,FILS);
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;       //devolvemos los colores originales por si acaso
            Console.ForegroundColor = ConsoleColor.White;
            
        } //Metodo encargado de dibujar
        public void DibujaPers()
        {
            //Para cada personaje, dibujaremos su pos actual con su color correspondiente
            for(int i = 0; i < pers.Length; i++) { 
            
                Console.SetCursorPosition(pers[i].posY, pers[i].posX);
                Console.BackgroundColor = colors[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("c");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(0, FILS + 1);
            //Ademas devolveremos la informacion de degug, si este estuviera activado
            if (Debug)
            {
                for (int a = 0; a < pers.Length; a++)
                {
                    if (a == 0)
                    {
                        Console.Write("Pacman: {0}(X), {1}(Y) ", pers[a].posX, pers[a].posY);
                        Console.WriteLine("Dir Pacman: ({0},{1}).", pers[a].dirX, pers[a].dirY);
                    }
                    else
                    {
                        Console.Write("Pos Fant {2}: {0}(X), {1}(Y) ", pers[a].posX, pers[a].posY, a);
                        Console.WriteLine("Dir Fant{2}: ({0},{1}).", pers[a].dirX, pers[a].dirY, a);
                        Console.WriteLine();
                    }
                }
            }
        }
        public void BorraPers()
        {
            for (int i = 0; i < pers.Length; i++) //Para cada oersonaje vamos a borrar su casilla actual
            {

                Console.SetCursorPosition(pers[i].posY, pers[i].posX); //Ponemos el cursor en la pos del personaje
                Console.BackgroundColor = ConsoleColor.Black; //Ponemos el fondo negro (Siempre se va a mover a cas vacia)
                if(cas[pers[i].posX,pers[i].posY] == Casilla.Comida) //Por si los fantasmas pasan sobre una comida o vitamina
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(".");
                }
                else if(cas[pers[i].posX, pers[i].posY] == Casilla.Vitamina)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*");
                }
                else
                    Console.Write(" ");
            }
            Console.SetCursorPosition(0, FILS + 1);
        }
        public void DibujaMenu()
        {
            for (int i = 0; i < FILS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    switch (cas[i, j])
                    {
                        case Casilla.Blanco:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("  ");
                            break;
                        case Casilla.Muro:
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write("  ");
                            break;
                        case Casilla.MuroCelda:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            break;
                        case Casilla.Vitamina:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("*");
                            break;
                        default: break;
                    }

                }

                Console.WriteLine();
            }
            DibujaPers();
            Console.SetCursorPosition(0, FILS);
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;       //devolvemos los colores originales por si acaso
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void DibujaCasilla(int x,int y)
        {
            Console.SetCursorPosition(y, x);
            switch (cas[x, y])
            {
                case Casilla.Blanco:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    break;
                case Casilla.Comida:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("·");
                    break;
                case Casilla.Muro:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(" ");
                    break;
                case Casilla.MuroCelda:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(" ");
                    break;
                case Casilla.Vitamina:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("*");
                    break;
                default: break;
            }
        }
        public bool siguiente(int x, int y, int dx, int dy, out int nx, out int ny)
        {
            nx = x + dx;
            ny = y + dy;
            //Aplicamos la logica toroidal (Para filas)
            if (nx >= FILS)
                nx = 0;
            else if (nx < 0)
                nx = FILS - 1;
            //Y para columnas
            if (ny >= COLS)
                ny = 0;
            else if (ny < 0)
                ny = COLS - 1;

            //Y devolvemos si se puede mover
            return cas[nx,ny] != Casilla.Muro&& cas[nx, ny] != Casilla.MuroCelda; //Si hay muro no se mueve
        }
        public void getDims(out int fils,out int cols)
        {
            //Este metodo es auxiliar para dibujar el game over al final
            fils = FILS;
            cols = COLS;
        }

        public void muevePacman()
        {
            int nx, ny;
            if (siguiente(pers[0].posX,pers[0].posY,pers[0].dirX,pers[0].dirY,out nx,out ny)) //Si se puede mover, se mueve en la dir
            {
                pers[0].posX = nx;
                pers[0].posY = ny;
                

                //Ahora quitaremos la comida si la hubiera
                if (cas[nx, ny] == Casilla.Comida)
                {
                    cas[nx, ny] = Casilla.Blanco;
                    numComida--;
                }
                    
                //O las vitaminas, en cuyo caso enviará a los fantasmas a su pos
                else if (cas[nx, ny] == Casilla.Vitamina)
                {
                    numComida--;
                    cas[nx, ny] = Casilla.Blanco;
                    for(int i = 1; i < pers.Length; i++)
                    {
                        pers[i].posX = pers[i].defX;
                        pers[i].posY = pers[i].defY;
                    }
                }      
            }
        } 
        public bool cambiaDir(char c)
        {
            int ndx=0, ndy=0; //Variables para asignar las nuevas direcciones
            //Primero vemos cual va a ser la nueva direccion
            switch (c)
            {
                case 'u':
                    ndx = -1;
                    break;
                case 'd':
                    ndx = 1;
                    break;
                case 'l':
                    ndy = -1;
                    break;
                case 'r':
                    ndy = 1;
                    break;
            }
            int nx, ny;
            //Luego vemos si puede girar
            if (siguiente(pers[0].posX, pers[0].posY, ndx, ndy, out nx, out ny))
            {
                //En cuyo caso, cambiamos la direccion
                pers[0].dirX = ndx;
                pers[0].dirY = ndy;
                return true;
            }
            else return false;
            
        }
        public void leeInput(ref char c)
        {
            string key;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey().Key.ToString();
                switch (key)
                {
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
                    case "Escape":
                        c = 'p';
                        break;
                    default:
                        c = ' ';
                        break;
                }
            }
            while (Console.KeyAvailable)
                Console.ReadKey();

        }
        public bool hayFantasma(int x, int y)
        {
            int i = 1;
            //Simplemente, si hay un fantasma en la posicion dada, devuelve true
            while (i < pers.Length && (pers[i].posX != x || pers[i].posY != y))
                i++;
            return i < pers.Length;
        }
        public void posiblesDirs(int fant,out ListaPares l,out int cont)
        {
            //Inicializamos las direcciones
            l= new ListaPares();
            l.insertaFin(1, 0);
            l.insertaFin(0, 1);
            l.insertaFin(-1, 0);
            l.insertaFin(0, -1);
            //Inicializamos la lista de direcciones
            cont = 4;
            int dx, dy;
            int nx, ny;
            //Ahora hacemos un recorrido en esa lista
            l.iniciaRecorrido();
            while(l.dame_actual_y_avanza(out dx, out dy))
            {
             //Donde comprobamos para cada direccion de el fantasma dado si se puede mover   
                if (!(siguiente(pers[fant].posX,pers[fant].posY,dx,dy,out nx,out ny) && !hayFantasma(nx, ny)))
                {
                    //En cuyo caso añadimos la direccion a la lista de posibles direcciones y aumentamos cont
                    l.eliminaElto(dx, dy);
                    cont--;
                }
                else if (!(siguiente(pers[fant].posX, pers[fant].posY, dx, dy, out nx, out ny)))
                {
                    l.eliminaElto(-pers[fant].dirX, -pers[fant].dirY);
                    cont--;
                }
            }
            if (cont > 1)
            {
                l.eliminaElto(-pers[fant].dirX, -pers[fant].dirY);
                // (0,1) --> (-0,-1) = (0,-1) , Que es la dir contraria
                cont--;
            }

        }
        public void seleccionaDir(int fant)
        {
            //Crear la lista de dirs posibles
            ListaPares l = new ListaPares();
            int cont = 0;
            posiblesDirs(fant, out l, out cont);
            //Ya tenemos la lista de posibles dirs
            //Ahora hay que eliminar la contraria si hay mas de 1 direccion
            
            //Cogemos un elemento random de la lista
            int ran = rnd.Next(0, cont);
            //Y cambiamos la direccion
            if(cont>0)
                l.nEsimo(ran, out pers[fant].dirX, out pers[fant].dirY);

        }
        public void eliminaMuroFant()
        {
            int x, y;
            MurosFant.iniciaRecorrido();
            while (MurosFant.dame_actual_y_avanza(out x,out y))
            {
                cas[x, y] = Casilla.Blanco;
                Console.SetCursorPosition(y, x);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
            }
        }
        public void mueveFantasma(int lap)
        {
            int nx, ny;
            for(int i = 1; i < pers.Length; i++)
            {
                seleccionaDir(i);
                if (siguiente(pers[i].posX,pers[i].posY,pers[i].dirX, pers[i].dirY,out nx,out ny))
                {
                    pers[i].posX = nx;
                    pers[i].posY = ny;
                }
            }
        }
        public bool captura()
        {
            bool colision = false;
            for (int i = 1; i < pers.Length; i++)
            {
                if (pers[i].posX == pers[0].posX && pers[i].posY == pers[0].posY)
                    colision = true;
            }
            return colision;
        }
        public bool finNivel()
        {
            return numComida <= 0; //En un principio no puede bajar de cero, pero por si acaso...
        }
    }
}
