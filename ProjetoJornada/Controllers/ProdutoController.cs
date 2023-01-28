using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoJornada.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoJornada.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(string nome, string descricao, double preco, int quant, int id)
        {

            foreach (IFormFile arquivo in Request.Form.Files)
            {

                string tipoArquivo = arquivo.ContentType;
                if (tipoArquivo.Contains("png") ||
                    tipoArquivo.Contains("jpeg") || tipoArquivo.Contains("gif"))
                {
                    MemoryStream s = new MemoryStream();
                    arquivo.CopyToAsync(s);
                    byte[] imgArquivo = s.ToArray();
                    Produto p = new Produto(nome, descricao, preco, quant, id, imgArquivo);

                    TempData["msg"] = p.Cadastrar();
                }

            }


            return RedirectToAction("CadastroProduto", "Admin");
        }



        public IActionResult Inativar(int ID_Produto)
        {
            Produto p = new Produto(null, null, 0, 0, ID_Produto, null);
            p.Inativar(ID_Produto);
            return RedirectToAction("PainelAdm", "Admin");

        }

        //metodo do catalogo 

        public IActionResult Teladescricao(int id)
        {
            //pegar  produto respectivo ao id, serializar em json, jogar em um temp data e depois disserialixzar na view usando o bjeto da forma que quiser

            Produto p = Produto.Puxardescricao(id);

            if (p != null)
            {

                TempData["Produto"] = JsonConvert.SerializeObject(p);

                return View();

            }
            //informaçoes pegas

            return RedirectToAction("Catalogo", "Home");
        }


        public IActionResult AdicionarAoCarrinho(string cpf, int IDProduto, string pNome, float pPreco)
        {
            if (cpf != null)
            {

                Produto.AdicionarAoCarrinho(cpf, IDProduto, pNome, pPreco);
                return RedirectToAction("Teladescricao", "Produto", new
                {
                    id = IDProduto
                });
            }

            return RedirectToAction("Logar", "Home");

        }
        public IActionResult Carrinho()
        {
            Usuario u = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("cliente"));

            return View(Produto.Carrinho(u.Cpf));

        }

        public IActionResult ExcluirCarrinho(int id)
        {
            Produto p = new Produto(null, null, 0, 0, id, null);
            p.ExcluirCarrinho(id);
            TempData["excluido"] = "Produto excluído do carrinho com sucesso";
            return RedirectToAction("Carrinho");
        }

        public IActionResult FinalizarCompra()
        {
            return View();
        }


    }
}
