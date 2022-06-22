using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
      public void Inserir(Usuario u)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Add(u);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Usuario u)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario usuario = bc.Usuarios.Find(u.Id);
                usuario.Id = u.Id;
                usuario.Nome = u.Nome;
                usuario.Login = u.Login;
                usuario.Senha = u.Senha;
                usuario.Tipo = u.Tipo;

                bc.SaveChanges();
            }
        }

        public ICollection<Usuario> ListarTodos(FiltrosUsuarios filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> query;
                
                if(filtro != null)
                {
                    //definindo dinamicamente a filtragem
                    switch(filtro.TipoFiltro)
                    {
                        case "Nome":
                            query = bc.Usuarios.Where(u => u.Nome.Contains(filtro.Filtro));
                        break;

                        case "Login":
                            query = bc.Usuarios.Where(u => u.Login.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Usuarios;
                        break;
                    }
                }
                else
                {
                    // caso filtro não tenha sido informado
                    query = bc.Usuarios;
                }
                
                //ordenação padrão
                return query.OrderBy(u => u.Nome).ToList();
            }
        }

        public Usuario ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }  

        public void Excluir (int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                               
                bc.Usuarios.Remove(bc.Usuarios.Find(id));
                bc.SaveChanges();
            }
        }
    }
}