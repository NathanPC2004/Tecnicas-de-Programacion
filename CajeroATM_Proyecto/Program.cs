using System;
using System.Collections.Generic;
using System.IO;
using CajeroATM_Proyecto;

namespace CajeroAutomatico
{
    class Program
    {
        static Dictionary<string, Usuario> usuarios = new();
        static string dataPath = "usuarios.txt";

        static void Main(string[] args)
        {
            CargarUsuarios();
            Console.WriteLine("Bienvenido al Cajero Automático");

            string tarjeta;
            do
            {
                Console.Write("Ingrese su número de tarjeta (16 dígitos): ");
                tarjeta = Console.ReadLine();
            } while (tarjeta.Length != 16 || !long.TryParse(tarjeta, out _));

            if (!usuarios.ContainsKey(tarjeta))
            {
                Console.WriteLine("Usuario nuevo. Cree su NIP (4 dígitos):");
                string nuevoNip = CrearNip();
                usuarios[tarjeta] = new Usuario(tarjeta, nuevoNip);
                GuardarUsuarios();
            }

            string nip;
            do
            {
                Console.Write("Ingrese su NIP (4 dígitos): ");
                nip = Console.ReadLine();
            } while (nip.Length != 4 || !int.TryParse(nip, out _));

            if (usuarios[tarjeta].Autenticar(nip))
            {
                MenuPrincipal(usuarios[tarjeta]);
            }
            else
            {
                Console.WriteLine("NIP incorrecto.");
            }
        }

        static void MenuPrincipal(Usuario usuario)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Retiro de efectivo");
                Console.WriteLine("2. Estado de cuenta");
                Console.WriteLine("3. Pago de servicios");
                Console.WriteLine("4. Depósitos");
                Console.WriteLine("5. Cambio de NIP");
                Console.WriteLine("6. Transferencia");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");

                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1: usuario.Retirar(); break;
                    case 2: usuario.MostrarEstadoCuenta(); break;
                    case 3: usuario.PagarServicios(); break;
                    case 4: usuario.Depositar(); break;
                    case 5: usuario.CambiarNip(); break;
                    case 6:
                        string destino;
                        do
                        {
                            Console.Write("Ingrese tarjeta destino (16 dígitos): ");
                            destino = Console.ReadLine();
                        } while (destino.Length != 16 || !long.TryParse(destino, out _));

                        if (!usuarios.ContainsKey(destino))
                        {
                            usuarios[destino] = new Usuario(destino, "0000"); // Usuario ficticio con NIP por defecto
                        }
                        usuario.Transferir(usuarios[destino]);
                        break;
                }

                GuardarUsuarios();
                Console.WriteLine("Presione una tecla para continuar...");
                Console.ReadKey();

            } while (opcion != 0);
        }

        static string CrearNip()
        {
            Console.Write("NIP: ");
            string nip1 = Console.ReadLine();
            Console.Write("Confirmar NIP: ");
            string nip2 = Console.ReadLine();

            if (nip1.Length == 4 && nip1 == nip2 && int.TryParse(nip1, out _))
                return nip1;

            Console.WriteLine("Los NIPs no coinciden o no tienen 4 dígitos. Intente de nuevo.");
            return CrearNip();
        }

        static void CargarUsuarios()
        {
            if (File.Exists(dataPath))
            {
                foreach (var linea in File.ReadAllLines(dataPath))
                {
                    Usuario user = Usuario.DesdeLinea(linea);
                    usuarios[user.Tarjeta] = user;
                }
            }
        }

        static void GuardarUsuarios()
        {
            List<string> datos = new();
            foreach (var u in usuarios.Values)
                datos.Add(u.ConvertirLinea());
            File.WriteAllLines(dataPath, datos);
        }
    }
}
