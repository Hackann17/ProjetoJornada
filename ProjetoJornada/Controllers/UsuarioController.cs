using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProjetoJornada.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetoJornada.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Cliente()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Cadastrar(string cpf, string nome, string senha, string nomeUsuario, string email)
        {
            Usuario u = new Usuario(cpf, nome, senha, nomeUsuario, email, 0);

            TempData["msg"] = u.Adicionar();
            return RedirectToAction("Cadastro", "Home");


        }

        //direcionamento para o metodo de conferir o login
        [HttpPost]

        public IActionResult Logar(string nomeUsuario, string senha)
        {
            Usuario u = Usuario.Logar(nomeUsuario, senha);


            if (u != null)
            {
                if (u.Adm == 1)
                {
                    //Gerando a sessao

                    //guardando o usuario serializado na sessao 
                    HttpContext.Session.SetString("admin", JsonConvert.SerializeObject(u));

                    return RedirectToAction("PainelAdm", "Admin");

                }

                //guandando o usario na sessao 
                HttpContext.Session.SetString("cliente", JsonConvert.SerializeObject(u));

                return RedirectToAction("Cliente", "Usuario");//apontar para a tela do usuario aqui

            }
            else
            {
                //se o objeto do ususario for vazio ele ira direto para a tela de login

                return RedirectToAction("Logar", "Home");
            }


        }



        //LEMBRAR DE FAZER UM BOTAO DE DESLOGAR
        public IActionResult Deslogar()
        {
            //área deslogar o usuario , é necessario matar a sessao

            HttpContext.Session.Remove("admin");
            return RedirectToAction("Index", "Home");


        }
        public IActionResult Deslogar2()
        {
            //área deslogar o usuario , é necessario matar a sessao
            HttpContext.Session.Remove("cliente");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Carrinho()
        {
            if (HttpContext.Session.GetString("cliente") != null)
            {
                return RedirectToAction("Carrinho", "Produto");

            }

            return RedirectToAction("Logar", "Home");
        }

        [HttpPost]
        public IActionResult Alterar(string senha)
        {
            Usuario c = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("cliente"));
            Usuario u = new Usuario(c.Cpf, null, null, null, senha, 0);
            u.Alterar(c.Cpf, senha);
            TempData["alterado"] = "Alteração realizada com sucesso!";
            return RedirectToAction("Cliente", "Usuario");
        }

    }
}
