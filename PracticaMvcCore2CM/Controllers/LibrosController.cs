using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2CM.Extensions;
using PracticaMvcCore2CM.Filters;
using PracticaMvcCore2CM.Models;
using PracticaMvcCore2CM.Repositories;

namespace PracticaMvcCore2CM.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;

        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Libros(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 0;
            }
            int numeroLibros = 0;
            Libro libro = this.repo.GetLibrosPaginados
                (posicion.Value, ref numeroLibros);
            ViewData["DATOS"] = "Libros " + (posicion + 1)
                + " de " + numeroLibros;
            int siguiente = posicion.Value + 1;
            if (siguiente >= numeroLibros)
            {
                siguiente = 0;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 0)
            {
                anterior = numeroLibros - 1;
            }
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(libro);

        }

        public IActionResult LibrosPorGenero(int idgenero)
        {
            List<Libro> libros = this.repo.GetLibrosGenero(idgenero);
            return View(libros);
        }

        //[AuthorizeUsuarios]
        public IActionResult DetallesLibros(int idlibro, int? idcarrito)
        {
            if (idcarrito != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idcarrito.Value);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            Libro libro = this.repo.FindLibro(idlibro);
            return View(libro);
        }

        public IActionResult ProductosCarrito(int? ideliminar)
        {
            List<int> idsCarrito =
                HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (idsCarrito == null)
            {
                ViewData["MENSAJE"] = "No existen libros en carrito";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    idsCarrito.Remove(ideliminar.Value);
                    if (idsCarrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("CARRITO", idsCarrito);
                    }
                }
                List<Libro> libros = this.repo.GetLibrosSession(idsCarrito);
                return View(libros);
            }
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> PedidosRealizados(int idusuario, int idlibro)
        {
            //INSERTAMOS EN PEDIDOS TODO LO DE SESSION
            //LIMPIAMOS SESSION

            //MUESTRAS LA VISTA DE SQL VISTA PEDIDOS CON UN WHERE DEL USUARIO

            idusuario = int.Parse(HttpContext.User.FindFirst("Id")?.Value);
            await this.repo.AgregarProductoAsync(idlibro, idusuario);
            List<VistaPedido> pedidos = this.repo.GetPedidos(idlibro);
            return View(pedidos);
        }
    }
}
