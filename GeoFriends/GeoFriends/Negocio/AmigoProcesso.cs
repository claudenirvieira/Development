using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Configuration;
using GeoFriends.Amigo;

namespace GeoFriends.Negocio
{
    class AmigoProcesso
    {
        public bool Erro { get; set; }

        public void inserirAmigo(eAmigo novoAmigo)
        {
            try
            {
                string sValor = "";
                sValor += novoAmigo.Nome + "|";
                sValor += novoAmigo.Endereco + "|";
                sValor += novoAmigo.Bairro + "|";
                sValor += novoAmigo.Cidade + "|";
                sValor += novoAmigo.CEP + "|";
                sValor += novoAmigo.Latitude.ToString() + "|";
                sValor += novoAmigo.Longitude.ToString() + "|";
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Add(novoAmigo.Nome, sValor);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nErro ao incluir Amigo.\nDescrição: " + ex.Message + "\nTente novamente.");
                this.Erro = true;
            }
        }

        public string[] consultaAmigoLocalizacao(string[] args)
        {
            string sLinha = "";
            Char sSeparador = '|';
            foreach (string chave in ConfigurationManager.AppSettings)
            {
                sLinha = ConfigurationManager.AppSettings[chave];
                String[] sCampos = sLinha.Split(sSeparador);
                if (sCampos[0].ToUpper() == args[0].ToUpper())
                {
                    args[1] = sCampos[1];
                    args[2] = sCampos[2];
                    args[3] = sCampos[3];
                    args[4] = sCampos[4];
                    args[5] = sCampos[5];
                    args[6] = sCampos[6];
                    break;
                }
            }
            return args;
        }

        public void consultaAmigosProximos(string[] args)
        {
            try
            {
                string sLinha = "";
                string sLinhaAmigos;
                double sKM = 0;
                int nCont = 0;
                Char sSeparador = '|';
                GeoCoordinate geoAmigoLocalAtual = new GeoCoordinate(Convert.ToDouble(args[5]), Convert.ToDouble(args[6]));
                List<OrdenaKilometragem> AmigoProximo = new List<OrdenaKilometragem>();

                foreach (string chave in ConfigurationManager.AppSettings)
                {
                    sLinha = ConfigurationManager.AppSettings[chave];
                    String[] sCampos = sLinha.Split(sSeparador);
                    if (args[0].ToUpper() != sCampos[0].ToUpper())
                    {
                        GeoCoordinate geoAmigoProximo = new GeoCoordinate(Convert.ToDouble(sCampos[5]), Convert.ToDouble(sCampos[6]));
                        sKM = geoAmigoLocalAtual.GetDistanceTo(geoAmigoProximo);
                        AmigoProximo.Add(new OrdenaKilometragem(sCampos[0], Convert.ToDouble(sKM)));
                    }
                }
                AmigoProximo.OrderBy(p => p.Km);
                Console.WriteLine("");
                foreach (OrdenaKilometragem n in AmigoProximo)
                {
                    string[] sDados = new string[] { n.Amigo, "", "", "", "", "", "" };
                    consultaAmigoLocalizacao(sDados);
                    sLinhaAmigos = "Nome: " + sDados[0] + "\n" +
                        "Endereço: " + sDados[1] + "\n" +
                        "Bairro:   " + sDados[2] + "\n" +
                        "Cidade:   " + sDados[3] + "\n" +
                        "CEP:      " + sDados[4] + "\n" +
                        "-----------------------------------------";
                    Console.WriteLine(sLinhaAmigos);
                    nCont++;
                    if (nCont >= 3) break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nErro ao localizar Amigo mais próximo.\nDescrição: " + ex.Message + "\nTente novamente.");
                this.Erro = true;
            }
        }

        public class OrdenaKilometragem
        {
            public string Amigo { get; set; }
            public double Km { get; set; }
            public OrdenaKilometragem()
            {
            }
            public OrdenaKilometragem(String Amigo, double Km)
            {
                this.Amigo = Amigo;
                this.Km = Km;
            }
        }
    }
}
