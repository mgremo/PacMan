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
            Tablero tab = new Tablero("level06.dat");
            tab.Dibuja();
            char c = ' ';
            bool captura = false;
            int lap = 100;
            tab.SetLap(3000);
            int lapFantAct = 3000;
            bool FlagFant = false;
            while (tab.finNivel() && !captura)
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

                //captura = tab.captura();

                tab.mueveFantasma(0);
                if (!captura)
                    captura = tab.captura();
                //tab.Dibuja();
                tab.DibujaPers();
                
                System.Threading.Thread.Sleep(lap);
            }
            //Hola soy un comentario
            //Aqui miniman comentando
            //Otro commit en master
            //Aqui tendria que estar en miniman 2
            //Espero que miniman 2 no cambie esto
            //miman 2 cambio esto
        }
    }
}
