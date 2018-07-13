using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GeoFriends.Negocio;
using GeoFriends.Amigo;

namespace GeoFriends
{
    class Program
    {
        static void Main(string[] args)
        {
            iniciaTelaGeoFriends();
        }

        #region <<< Menu Principal >>>
        private static void iniciaTelaGeoFriends()
        {
            /* 
                Menu Principal do GeoFriends - Localização de Amigos
            */
            int opcao = 0;
            string strTela = "+------------------------------------+\n" +
                             "| GeoFriends - Localicação de Amigos |\n" +
                             "| Author: Claudenir Vieira           |\n" +
                             "| Data: 11/07/2018                   |\n" +
                             "+------------------------------------+\n\n" +
                             "1-Cadastrar Amigos\n" +
                             "2-Listar Amigos\n" +
                             "3-Buscar Amigos Próximos\n" +
                             "0-Sair\n\n" +
                             "Selecione a opção desejada ou [ENTER] para sair: ";
            while (true)
            {
                Console.Write(strTela);
                opcao = Convert.ToInt32("0" + Console.ReadLine());
                if (opcao == 0) break;
                switch (opcao)
                {
                    case 1:
                        cadastroGeoFriends();
                        break;
                    case 2:
                        listaGeoFriends();
                        break;
                    case 3:
                        buscaFriendsProximos();
                        break;
                    default:
                        Console.WriteLine("\nOpção Inválida, tente novamente");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }
        #endregion

        #region <<< Cadastro de Amigos >>>
        private static void cadastroGeoFriends()
        {
            /* 
                Menu Cadastro de Amigos
            */
            string sNome;
            string sEndereco;
            string sBairro;
            string sCidade;
            string sCep;
            string sLatitude;
            string sLongitude;

            string strTela = "+------------------------------------+\n" +
                             "| GeoFriends - Localicação de Amigos |\n" +
                             "| Author: Claudenir Vieira           |\n" +
                             "| Data: 11/07/2018                   |\n" +
                             "+------------------------------------+\n\n" +
                             "Cadastro de Amigos\n\n" +
                             "Informe os dados ou [ENTER] no Nome para sair: \n\n" +
                             "Nome: ";

            while (true)
            {
                Console.Clear();
                Console.Write(strTela);
                sNome = Console.ReadLine();
                if (String.IsNullOrEmpty(sNome)) break;
                Console.Write("Endereço: ");
                sEndereco = Console.ReadLine();
                Console.Write("Bairro: ");
                sBairro = Console.ReadLine();
                Console.Write("Cidade: ");
                sCidade = Console.ReadLine();
                Console.Write("Cep: ");
                sCep = Console.ReadLine();
                Console.Write("Latitude: ");
                sLatitude = Console.ReadLine();
                Console.Write("Logitude: ");
                sLongitude = Console.ReadLine();

                eAmigo novoAmigo = new eAmigo();
                novoAmigo.Nome = sNome;
                novoAmigo.Endereco = sEndereco;
                novoAmigo.Bairro = sBairro;
                novoAmigo.Cidade = sCidade;
                novoAmigo.CEP = sCep;
                novoAmigo.Latitude = Convert.ToDouble(sLatitude.Replace(".",","));
                novoAmigo.Longitude = Convert.ToDouble(sLongitude.Replace(".", ","));

                if ((String.IsNullOrEmpty(sEndereco + sBairro + sCidade) && (String.IsNullOrEmpty(sCep))))
                {
                    Console.WriteLine("\nDados incompletos, Tente incluir novamente!");
                }
                else
                {
                    AmigoProcesso AmigoNovoProcesso = new AmigoProcesso();
                    AmigoNovoProcesso.inserirAmigo(novoAmigo);
                    if(AmigoNovoProcesso.Erro == false)
                        Console.WriteLine("\nAmigo incluído com sucesso!");
                }
                Console.ReadKey();
            }
            Console.Clear();
        }
        #endregion

        #region <<< Lista de Amigos >>>
        private static void listaGeoFriends()
        {
            string strTela = "+------------------------------------+\n" +
                             "| GeoFriends - Localicação de Amigos |\n" +
                             "| Author: Claudenir Vieira           |\n" +
                             "| Data: 11/07/2018                   |\n" +
                             "+------------------------------------+\n\n" +
                             "Relação de Amigos\n\n";

            Console.Clear();
            Console.Write(strTela);
            string sLinha = "";
            string sCab = "         Nome:    Endereço:Bairro:  Cidade:  CEP:     Latitude:Longitude:          ";
            int nind;
            Char sSeparador = '|';
            foreach (string chave in ConfigurationManager.AppSettings)
            {
                sLinha = ConfigurationManager.AppSettings[chave];
                String[] sCampos = sLinha.Split(sSeparador);
                nind = 1;
                foreach (var Campo in sCampos)
                {
                    if(nind <=7)
                        Console.WriteLine(sCab.Substring((nind * 9), 9) + " " + Campo);
                    nind++;
                }
                Console.WriteLine("------------------------------------");
            }
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        #region <<< Busca Amigos Próximos >>>
        private static void buscaFriendsProximos()
        {
            string sNome = "";
            string sAcao = "N";
            string strTela = "+------------------------------------+\n" +
                             "| GeoFriends - Localicação de Amigos |\n" +
                             "| Author: Claudenir Vieira           |\n" +
                             "| Data: 11/07/2018                   |\n" +
                             "+------------------------------------+\n\n" +
                             "Busca de Amigos Próximos\n\n" +
                             "Informe o nome do Amigo correspondente com o endereço atual ou [ENTER] para sair.\n\n" +
                             "Nome: ";

            while (true)
            {
                Console.Clear();
                Console.Write(strTela);
                sNome = Console.ReadLine();
                if (String.IsNullOrEmpty(sNome)) break;
                AmigoProcesso AmigoNovoProcesso = new AmigoProcesso();
                string[] sDados = new string[] { sNome, "", "", "", "", "", "" };
                AmigoNovoProcesso.consultaAmigoLocalizacao(sDados);

                if((String.IsNullOrEmpty(sDados[5])) || (String.IsNullOrEmpty(sDados[6])))
                {
                    Console.WriteLine("\nAmigo para Consulta não existe, tente novamente!");
                    Console.ReadKey();
                }
                else
                {
                    Console.Write("\nConfirmar consulta para " + sDados[0] + " [Informar S ou N]: ");
                    sAcao = Console.ReadLine();
                    if ((sAcao.ToUpper() != "S") && ((sAcao.ToUpper() != "N")))
                        Console.WriteLine("\nOpção invalida!");
                    else
                    {
                        if (sAcao.ToUpper() == "S")
                        {
                            AmigoNovoProcesso.consultaAmigosProximos(sDados);
                            Console.ReadKey();
                        }
                    }
                }
            }
            Console.Clear();
        }
        #endregion
    }
}
