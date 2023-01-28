using Microsoft.AspNetCore.Mvc;
using ProjetoJornada.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoJornada.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult CadastroProduto()
        {
            return View();
        }

        public IActionResult PainelAdm()
        {
            return View();
        }

        public IActionResult ListarClientes()
        {
            return View(Usuario.Listar());
        }
        
    }
}
