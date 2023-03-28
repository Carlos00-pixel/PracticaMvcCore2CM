using PracticaMvcCore2CM.Data;
using PracticaMvcCore2CM.Models;

namespace PracticaMvcCore2CM.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros (LibrosContext context)
        {
            this.context = context;
        }

        public List<Libro> GetLibros()
        {
            return this.context.Libros.ToList();
        }

        public List<Libro> GetLibrosSession(List<int> ids)
        {
            var consulta = from datos in this.context.Libros
                           where ids.Contains(datos.IdLibro)
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }

        public Libro FindLibro(int idlibro)
        {
            return this.context.Libros.FirstOrDefault(x => x.IdLibro == idlibro);
        }

        public Libro GetLibrosPaginados(int posicion, ref int numeroLibros)
        {
            List<Libro> libros = this.GetLibros();
            numeroLibros = libros.Count;
            Libro libro =
                libros.Skip(posicion).Take(1).FirstOrDefault();
            return libro;
        }

        public List<Libro> GetLibrosGenero(int idgenero)
        {
            var consulta = from x in this.context.Libros
                           where x.IdGenero == idgenero
                           select x;
            return consulta.ToList();
        }

        private int GetMaxIdPedido()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdPedido) + 1;
            }
        }

        private int GetMaxIdFactura()
        {
            if (this.context.Pedidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Pedidos.Max(z => z.IdFactura) + 1;
            }
        }

        public async Task AgregarProductoAsync (int idlibro, int idusuario)
        {
            Pedido prod = new Pedido();

            int maximo = this.GetMaxIdPedido();

            prod.IdPedido = maximo;
            prod.IdFactura = this.GetMaxIdFactura();
            prod.IdLibro = idlibro;
            prod.IdUsuario = idusuario;
            prod.Fecha = DateTime.Now;
            prod.Cantidad = 1;

            this.context.Pedidos.Add(prod);

            await this.context.SaveChangesAsync();
        }

        /*
         public async Task AgregarProductoAsync (int idlibro)
        {
            List<Pedido> prod = new List<Pedido>();

            int maximo = this.GetMaxIdPedido();

            foreach(Pedido in prod)
            {
                Pedido ped = new Pedido();
                ped.IdPedido = maximo;
                ped.IdFactura = this.GetMaxIdFactura();
                ped.IdLibro = idlibro;
                ped.Fecha = DateTime.Now;
                ped.Cantidad = 1;

                prod.Add(ped);
            }
            this.context.Pedidos.Add(prod);

            await this.context.SaveChangesAsync();
        }
         */

        public List<VistaPedido> GetPedidos(int idpedido)
        {
            var consulta = from x in this.context.VistaPedidos
                           where x.IdVistaPedidos == idpedido
                           select x;
            return consulta.ToList();
        }
    }
}
