using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pac_Man
{
    public class Usuarios
    {
        public string nombre { get; }
        int nivel;
        int puntuacion;
        public Usuarios(string name, int level, int score)
            {
                nombre = name;
                nivel = level;
                puntuacion = score;
            }
        public Usuarios()
        {
            nombre = "Usuario";
            nivel = 0;
            puntuacion = 0;
        }
        static public void vuelcaLista(List<Usuarios> users,string fileName)
        {
            try
            {
                StreamWriter file = new StreamWriter(fileName);
                file.WriteLine(users.Count);
                foreach (Usuarios user in users)
                {
                    file.WriteLine(user.nombre);
                    file.WriteLine(user.nivel);
                    file.WriteLine(user.puntuacion);
                }
                file.Close();
            }
            catch
            {
                throw new Exception("Error al guardar la lista");
            }
        }
        static public List<Usuarios> cargaUsuarios(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            //Numero de usuarios registrados
            int nUsers = int.Parse(file.ReadLine());
            List<Usuarios> users = new List<Usuarios>();
            //Añadimos n usuarios
            for (int i = 0; i < nUsers; i++)
            {
                try
                {
                    users.Add(new Usuarios(file.ReadLine(), int.Parse(file.ReadLine()), int.Parse(file.ReadLine())));
                }
                catch
                {
                    throw new Exception("Error leyendo el usuario " + i);
                }
            }
            file.Close();
            return users;
        }
    }
}
