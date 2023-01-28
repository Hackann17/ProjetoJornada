using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetoJornada.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoJornada.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Logar()
        {
            return View();
        }

        public IActionResult SenhaPerdida()
        {
            return View();
        }

        public IActionResult Catalogo()
        {
            return View(Produto.Catalogo());

        }
    }
}
