using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoJornada.Models
{
    public class Produto
    {

        private string nome, descricao;
        private int quant, id;
        private double preco;
        /*MYSQL DO SENAI LOGO ABAIXO*/
        static string conexao = "Server=ESN509VMYSQL;Database=pjgd;User id=aluno;Password=Senai1234";
        private byte[] imgArquivo;

        //Construtor 
        public Produto(string nome, string descricao, double preco, int quant, int id, byte[] imgArquivo)
        {
            this.nome = nome;
            this.descricao = descricao;
            this.preco = preco;
            this.quant = quant;
            this.id = id;
            this.imgArquivo = imgArquivo;
        }

        //Getters e Setters
        public string Nome
        {
            get => nome; set => nome = value;
        }
        public string Descricao
        {
            get => descricao; set => descricao = value;
        }
        public double Preco
        {
            get => preco; set => preco = value;
        }
        public int Quant
        {
            get => quant; set => quant = value;
        }
        public int Id
        {
            get => id; set => id = value;
        }
        public byte[] ImgArquivo
        {
            get => imgArquivo; set => imgArquivo = value;
        }



        /*     ESCREVER MÉTODOS LOGO ABAIXO   */


        //Cadastro do Produto

        public string Cadastrar()
        {
            //Buscar no Banco de Dados
            MySqlConnection con = new MySqlConnection(conexao);

            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("INSERT INTO Produto(nome, preco, quantidade, descricao, img) VALUES(@nome, @preco, @quantidade, @descricao, @img)", con);
                qry.Parameters.AddWithValue("@nome", nome);
                qry.Parameters.AddWithValue("@preco", preco);
                qry.Parameters.AddWithValue("@quantidade", quant);
                qry.Parameters.AddWithValue("@descricao", descricao);
                qry.Parameters.AddWithValue("@img", imgArquivo);

                qry.ExecuteNonQuery();
                con.Close();

                return "Cadastro Concluido!";


            }
            catch (Exception ex)
            {
                return "ERRO: " + ex.Message;
            }
        }


        //METODODO INATIVAR OS PRODUTOS
        public string Inativar(int ID_Produto)
        {
            MySqlConnection con = new MySqlConnection(conexao);
            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("UPDATE Produto SET Quantidade = 0 WHERE IDProduto = @ID_Produto", con);
                qry.Parameters.AddWithValue("@ID_Produto", ID_Produto);

                qry.ExecuteNonQuery();
                con.Close();

                return "Produto Inativado!";


            }
            catch (Exception ex)
            {
                return "ERRO: " + ex.Message;
            }
        }

        public string ExcluirCarrinho(int id)
        {
            //Conexão com o banco de dados
            MySqlConnection con = new MySqlConnection(conexao);
            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("DELETE FROM carrinho WHERE ID_Produto = @id", con);
                qry.Parameters.AddWithValue("@id", id);
                qry.ExecuteNonQuery();
                con.Close();
                return "Produto excluído do carrinho com sucesso";
            }
            catch (Exception ex)
            {
                return "ERRO: " + ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        //METODO DO CATALOGO
        public static List<Produto> Catalogo()
        {
            List<Produto> lista = new List<Produto>();
            //Buscar no Banco de Dados
            MySqlConnection con = new MySqlConnection(conexao);

            con.Open();
            MySqlCommand qry = new MySqlCommand("SELECT * FROM Produto", con);
            MySqlDataReader leitor = qry.ExecuteReader();
            while (leitor.Read())
            {
                string nome = leitor["Nome"].ToString();
                double preco = double.Parse(leitor["Preco"].ToString());
                int id = int.Parse(leitor["IDProduto"].ToString());
                int quantidade = int.Parse(leitor["Quantidade"].ToString());
                byte[] imgArquivo = (byte[])leitor["Img"];
                Convert.ToBase64String(imgArquivo);
                Produto p = new Produto(nome, null, preco, quantidade, id, imgArquivo: imgArquivo);
                lista.Add(p);

            }
            con.Close();
            return lista;




        }

        //puxar descriçao do produto

        public static Produto Puxardescricao(int id)
        {
            MySqlConnection con = new MySqlConnection(conexao);
            Produto p = null;
            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("select * from produto where IDProduto = @id", con);
                qry.Parameters.AddWithValue("@id", id);

                MySqlDataReader leitor = qry.ExecuteReader();


                while (leitor.Read())
                {
                    string nome = leitor["Nome"].ToString();
                    string descricao = leitor["Descricao"].ToString();
                    double preco = double.Parse(leitor["Preco"].ToString());
                    int quant = int.Parse(leitor["Quantidade"].ToString());
                    byte[] imgArquivo = (byte[])leitor["img"];
                    Convert.ToBase64String(imgArquivo);


                    p = new Produto(nome, descricao, preco, quant, id, imgArquivo: imgArquivo);
                    /*Produto(string nome, string descricao, float preco, int quant, int id, byte[] imgArquivo)*/

                }
                return p;
            }

            catch (Exception)
            {
                return p;

            }

            finally
            {

                con.Close();

            }


        }

        //realizar a adição no carrinho 
        public static void AdicionarAoCarrinho(string cpf, int IDProduto, string pNome, double pPreco)
        {
            // esse metodo ira verificar a tabela de carrinho , e se nao houver tal item com esses parametros na mesma ira adiciona - lo

            MySqlConnection con = new MySqlConnection(conexao);

            //com o objeto completo pegamos todos os seus valores aplicamos a variaveis e adivionamao s na tabel ade carrinho 

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("select * from carrinho where CPFCliente = @cpf and ID_Produto = @IDProduto ", con);
                qry.Parameters.AddWithValue("@cpf", cpf);
                qry.Parameters.AddWithValue("@idProduto", IDProduto);

                MySqlDataReader leitor = qry.ExecuteReader();

                if (!leitor.HasRows)
                {
                    //matar o leitor
                    leitor.Close();
                    //gerar o comando de adicionar na tabela
                    MySqlCommand query = new MySqlCommand("insert into carrinho(ID_Produto, CPFCliente, nome, preco) VALUES(@idProduto,@cpf,@pNome,@pPreco)", con);

                    query.Parameters.AddWithValue("@idProduto", IDProduto);
                    query.Parameters.AddWithValue("@cpf", cpf);
                    query.Parameters.AddWithValue("@pNome", pNome);
                    query.Parameters.AddWithValue("@pPreco", pPreco);

                    query.ExecuteNonQuery();

                    con.Close();

                }
            }

            finally
            {
                con.Close();


            }

        }


        //carrinho
        public static List<Produto> Carrinho(string cpf)
        {
            MySqlConnection con = new MySqlConnection(conexao);
            List<Produto> listacarrinho = new List<Produto>();

            try
            {

                con.Open();
                MySqlCommand qry = new MySqlCommand("select * from carrinho where CPFCliente = @cpf ", con);
                qry.Parameters.AddWithValue("@cpf", cpf);


                MySqlDataReader leitor = qry.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        //nome , preco , quantidadeselecionada 
                        string nome = leitor["Nome"].ToString();
                        double preco = double.Parse(leitor["Preco"].ToString());
                        int quant = int.Parse(leitor["Quantidade"].ToString());
                        int id = int.Parse(leitor["ID_Produto"].ToString());

                        Produto p = new Produto(nome, null, preco, quant, id, null);

                        listacarrinho.Add(p);

                    }
                    con.Close();

                    return listacarrinho;

                }
                return listacarrinho;

            }
            catch (Exception)
            {
                return listacarrinho;
            }
            finally
            {
                con.Close();
            }

        }
    }
}
    







