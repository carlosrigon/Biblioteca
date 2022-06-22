using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            u.Senha = Criptografia.TextoCriptografado(u.Senha);

            UsuarioService usuarioService = new UsuarioService();

            usuarioService.Inserir(u);
            ViewData["Mensagem"] = "Cadastro realizado com sucesso!";
            
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            FiltrosUsuarios objFiltro = null;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosUsuarios();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            UsuarioService usuarioService = new UsuarioService();
            return View(usuarioService.ListarTodos(objFiltro));
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            UsuarioService us = new UsuarioService();
            Usuario u = us.ObterPorId(id);
            return View(u);
        }

        [HttpPost]
        public IActionResult Edicao(Usuario u)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            u.Senha = Criptografia.TextoCriptografado(u.Senha);

            UsuarioService usuarioService = new UsuarioService();

            usuarioService.Atualizar(u);
            ViewData["Mensagem"] = "Cadastro atualizado com sucesso!";
            
            return RedirectToAction("Listagem");
        }

        public IActionResult Excluir(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            UsuarioService us = new UsuarioService();
            Usuario u = us.ObterPorId(id);
            
            return View(u);
        }

        [HttpPost]
        public IActionResult Excluir(string decisao, int id)
        {
            Usuario u = new UsuarioService().ObterPorId(id);
            
            if (decisao=="EXCLUIR"){
                ViewData["Mensagem"] = "Exclusão do usuário " + new UsuarioService().ObterPorId(id).Nome + " realizada com sucesso.";

                new UsuarioService().Excluir(id);
                
                return RedirectToAction("Listagem");
            } else {
                ViewData["Mensagem"] = "Exclusão cancelada.";
                return RedirectToAction("Listagem");
            }

        }

        public IActionResult needAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}