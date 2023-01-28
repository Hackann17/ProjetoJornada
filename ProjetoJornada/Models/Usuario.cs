using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjetoJornada.Models
{
    public class Usuario
    {

        private string nome, senha, email, cpf, nomeUsuario;
        private int adm;
        /*MYSQL DO SENAI LOGO ABAIXO*/
        static string conexao = "Server=ESN509VMYSQL;Database=pjgd;User id=aluno;Password=Senai1234";

        //Construtor
        public Usuario(string cpf, string nome, string email, string nomeUsuario, string senha, int adm)
        {
            this.cpf = cpf;
            this.nome = nome;
            this.email = email;
            this.nomeUsuario = nomeUsuario;
            this.senha = senha;
            this.adm = adm;
        }

        //Encapsulamentos
        public string Nome
        {
            get => nome; set => nome = value;
        }
        public string Senha
        {
            get => senha; set => senha = value;
        }
        public string Email
        {
            get => email; set => email = value;
        }
        public string Cpf
        {
            get => cpf;
        }
        public string NomeUsuario
        {
            get => nomeUsuario; set => nomeUsuario = value;
        }
        public int Adm
        {
            get => adm; set => adm = value;
        }



        /*     ESCREVER MÉTODOS LOGO ABAIXO   */


        //Adicionar Usuário

        public string Adicionar()
        {
            //Buscar no BD
            MySqlConnection con = new MySqlConnection(conexao);
            //Adicionar no banco de dados

            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("INSERT INTO Usuario(cpf, nome, senha, nomeUsuario, email) VALUES(@cpf, @nome, @senha, @nomeUsuario, @email)", con);
                qry.Parameters.AddWithValue("@cpf", cpf);
                qry.Parameters.AddWithValue("@nome", nome);
                qry.Parameters.AddWithValue("@senha", senha);
                qry.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                qry.Parameters.AddWithValue("@email", email);
                qry.ExecuteNonQuery();
                con.Close();

                return "Cadastro Concluido!";


            }
            catch (Exception ex)
            {
                return "ERRO: " + ex.Message;
            }
        }

        public string Alterar(string cpf, string senha)
        {
            //Conexao com o banco de dados
            MySqlConnection con = new MySqlConnection(conexao);

            try
            {
                con.Open();
                MySqlCommand qry = new MySqlCommand("UPDATE usuario SET senha = @senha WHERE CPF = @cpf", con);
                qry.Parameters.AddWithValue("@cpf", cpf);
                qry.Parameters.AddWithValue("@senha", senha);
                qry.ExecuteNonQuery();

                return "Alteração realizada com sucesso!";
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

        //metodo de listar os usuários

        public static List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            MySqlConnection con = new MySqlConnection(conexao);


            //Buscar no Banco de Dados e adicionando à lista, para uso posterior na View "Listar Cliente"

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("SELECT * FROM Usuario", con);
                MySqlDataReader leitor = qry.ExecuteReader();

                while (leitor.Read())
                {
                    Usuario u = new Usuario(leitor["cpf"].ToString(), leitor["nome"].ToString(), leitor["email"].ToString(), leitor["nomeUsuario"].ToString(), null, int.Parse(leitor["adm"].ToString()));

                    lista.Add(u);

                }

                con.Close();
                return lista;
            }
            catch (Exception)
            {
                return null;
            }

        }


        //metodo opara conferir se o usuario existe ou nao 
        public static Usuario Logar(string nomeUsuario, string senha)
        {
            //tenho que encontrar o ususario com os parametros de entrada , preencher seu objeto e retornar para o controller para ser serializado e adiocionado a sessão

            MySqlConnection con = new MySqlConnection(conexao);
            Usuario u = null;

            try
            {
                con.Open();

                MySqlCommand qry = new MySqlCommand("select * from Usuario where nomeUsuario = @nomeUsuario and senha = @senha  ", con);
                qry.Parameters.AddWithValue("@nomeUsuario", nomeUsuario);
                qry.Parameters.AddWithValue("@senha", senha);

                MySqlDataReader leitor = qry.ExecuteReader();


                if (leitor.HasRows)
                {

                    while (leitor.Read())
                    {

                        string cpf = leitor["cpf"].ToString();
                        string nome = leitor["nome"].ToString();
                        string email = leitor["email"].ToString();
                        int adm = int.Parse(leitor["adm"].ToString());

                        u = new Usuario(cpf, nome, email, nomeUsuario, senha, adm);

                    }
                    //retornando objeto com ususario exitente
                    return u;

                }
                //retornando objeto vazio
                return u;

            }

            catch (Exception ex)
            {
                return u;
            }

            finally
            {
                con.Close();
            }
        }
    }
}
