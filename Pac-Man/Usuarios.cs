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
        public void Save(int level, int score)
        {
            nivel = Math.Max(nivel,level);
            puntuacion = Math.Max(puntuacion,score);
        }
        public int Score() { return puntuacion; }
        public int Level() { return nivel; }
        static public bool GetUser(string name,List<Usuarios> users,out Usuarios user)
        {
            user = new Usuarios();
            int i = 0;
            while (i < users.Count && users[i].nombre != name)
                i++;
            if (i < users.Count)
            {
                user = users[i];
            }
            return i < users.Count;
        }
        static public void Sort(ref List<Usuarios> users)
        {
            //Bubble sort
            int n = users.Count; // numero de eltos del vector
                              // v[0] ya está ordenado, para cada uno desde 1 hasta n:
            for (int i = 1; i < n; i++)
            { // inv: v [0.. i−1] esta ordenado
              // insertamos v[i ] ordenadamente en el subvector v[0..i−1]
                Usuarios tmp = users[i]; // guardamos v[i]
                int j = i-1;
                // desplazamos eltos a la decha abriendo hueco para v[i]
                while ((j >= 0) && (users[j].puntuacion< tmp.puntuacion))
                {
                    users[j + 1] = users[j];
                    j--;
                }
                users[j + 1] = tmp;
            }
        }
        void swap(Usuarios user1,Usuarios user2)
        {
            Usuarios aux = user1;
            user1 = user2;
            user2 = aux;
        }
    }
}
