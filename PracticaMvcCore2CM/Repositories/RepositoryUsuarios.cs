using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2CM.Data;
using PracticaMvcCore2CM.Models;

namespace PracticaMvcCore2CM.Repositories
{
    public class RepositoryUsuarios
    {
        private LibrosContext context;

        public RepositoryUsuarios(LibrosContext context)
        {
            this.context = context;
        }

        public Usuario FindUsuario(int idusuario)
        {
            Usuario user = this.context.Usuarios.FirstOrDefault(x => x.IdUsuario == idusuario);
            return user;
        }

        public async Task<Usuario> FindEmailAsync
            (string email)
        {
            Usuario user =
                await this.context.Usuarios.FirstOrDefaultAsync
                (x => x.Email == email);
            return user;
        }

        public async Task<Usuario> ExisteUsuario
            (string email, string password)
        {
            Usuario user = await this.FindEmailAsync(email);
            var usuario = await this.context.Usuarios.Where
                (x => x.Email == email && x.Pass == password).FirstOrDefaultAsync();
            return usuario;
        }
    }
}
