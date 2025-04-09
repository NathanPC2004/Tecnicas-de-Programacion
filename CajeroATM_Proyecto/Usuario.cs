using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CajeroATM_Proyecto
{
    class Usuario
    {
        public string Tarjeta { get; }
        private string nip;
        private decimal saldo = 0;
        private List<string> transacciones = new();

        public Usuario(string tarjeta, string nip)
        {
            Tarjeta = tarjeta;
            this.nip = nip;
        }

        public bool Autenticar(string intentoNip) => intentoNip == nip;

        public void Retirar()
        {
            Console.Write("Monto a retirar: ");
            decimal monto = decimal.Parse(Console.ReadLine());
            if (monto <= saldo)
            {
                saldo -= monto;
                transacciones.Add($"Retiro: -${monto}");
                Console.WriteLine("Retiro exitoso.");
            }
            else
                Console.WriteLine("Saldo insuficiente.");
        }

        public void MostrarEstadoCuenta()
        {
            Console.WriteLine($"Saldo disponible: ${saldo}");
            Console.WriteLine("Últimas transacciones:");
            foreach (var t in transacciones)
                Console.WriteLine(t);
        }

        public void PagarServicios()
        {
            Console.WriteLine("1. Luz\n2. Agua\n3. Internet");
            Console.Write("Seleccione servicio: ");
            string servicio = Console.ReadLine();
            Console.Write("Ingrese número de convenio: ");
            string convenio = Console.ReadLine();
            Console.Write("Monto a pagar: ");
            decimal monto = decimal.Parse(Console.ReadLine());

            if (monto <= saldo)
            {
                saldo -= monto;
                transacciones.Add($"Pago {servicio} (${monto}) - Convenio {convenio}");
                Console.WriteLine("Pago realizado con éxito.");
            }
            else
                Console.WriteLine("Saldo insuficiente.");
        }

        public void Depositar()
        {
            Console.Write("Ingrese monto a depositar: ");
            decimal monto = decimal.Parse(Console.ReadLine());
            saldo += monto;
            transacciones.Add($"Depósito: +${monto}");
            Console.WriteLine("Depósito realizado con éxito.");
        }

        public void CambiarNip()
        {
            Console.Write("Nuevo NIP (4 dígitos): ");
            string nuevo = Console.ReadLine();
            Console.Write("Confirme nuevo NIP: ");
            string confirmacion = Console.ReadLine();
            if (nuevo == confirmacion && nuevo.Length == 4 && int.TryParse(nuevo, out _))
            {
                nip = nuevo;
                Console.WriteLine("NIP actualizado con éxito.");
            }
            else
                Console.WriteLine("Los NIPs no coinciden o no tienen 4 dígitos.");
        }

        public void Transferir(Usuario destino)
        {
            Console.Write("Monto a transferir: ");
            decimal monto = decimal.Parse(Console.ReadLine());
            if (monto <= saldo)
            {
                saldo -= monto;
                destino.saldo += monto;
                transacciones.Add($"Transferencia a {destino.Tarjeta}: -${monto}");
                destino.transacciones.Add($"Transferencia de {Tarjeta}: +${monto}");
                Console.WriteLine("Transferencia exitosa.");
            }
            else
                Console.WriteLine("Saldo insuficiente.");
        }

        public string ConvertirLinea()
        {
            return $"{Tarjeta}|{nip}|{saldo}|{string.Join(",", transacciones)}";
        }

        public static Usuario DesdeLinea(string linea)
        {
            var partes = linea.Split('|');
            var u = new Usuario(partes[0], partes[1]);
            u.saldo = decimal.Parse(partes[2]);
            if (partes.Length > 3)
                u.transacciones = new List<string>(partes[3].Split(','));
            return u;
        }
    }
}
