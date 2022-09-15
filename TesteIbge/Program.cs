
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NomesIbge
{
    public class ResultadoFrequencia
    {
        public string Nome { get; set; }
        public int Frequencia { get; set; }
        public string Estado { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var csv = File.ReadAllLines("C:\\nomes.csv");
            var uf = GetUf();
            var jsonUf = JsonConvert.DeserializeObject<List<Classes.Root>>(uf);
            var lista = new List<ResultadoFrequencia>();


            var idMg = jsonUf.Where(i => i.sigla == "MG").FirstOrDefault().id;
            var idEs = jsonUf.Where(i => i.sigla == "ES").FirstOrDefault().id;
            var idRs = jsonUf.Where(i => i.sigla == "RS").FirstOrDefault().id;


            for (int i = 1; i < csv.Length; i++)
            {
                var nome = csv[i];

                var resMg = GetNome(nome, idMg.ToString());
                var resEs = GetNome(nome, idEs.ToString());
                var resRs = GetNome(nome, idRs.ToString());

                var desResMg = JsonConvert.DeserializeObject<List<Classes.Nomes>>(resMg);
                var desResEs = JsonConvert.DeserializeObject<List<Classes.Nomes>>(resEs);
                var desResRs = JsonConvert.DeserializeObject<List<Classes.Nomes>>(resRs);

                var totalFrequenciaMg = desResMg.Count > 0 ? desResMg[0]?.res.Sum(x => x.frequencia) : 0;
                var totalFrequenciaEs = desResEs.Count > 0 ? desResEs[0]?.res.Sum(x => x.frequencia) : 0;
                var totalFrequenciaRs = desResRs.Count > 0 ? desResRs[0]?.res.Sum(x => x.frequencia) : 0;

                var max = totalFrequenciaMg > totalFrequenciaEs ? 1 : 2;
                max = max == 1 ? (totalFrequenciaMg > totalFrequenciaRs ? 1 : 3) : (totalFrequenciaEs > totalFrequenciaRs ? 2 : 3);

                switch (max)
                {
                    case 1:
                        var objMg = new ResultadoFrequencia()
                        {
                            Estado = "MG",
                            Frequencia = Convert.ToInt32(totalFrequenciaMg),
                            Nome = nome
                        };
                        lista.Add(objMg);

                        break;
                    case 2:
                        var objEs = new ResultadoFrequencia()
                        {
                            Estado = "ES",
                            Frequencia = Convert.ToInt32(totalFrequenciaEs),
                            Nome = nome
                        };
                        lista.Add(objEs);
                        break;
                    case 3:
                        var objRs = new ResultadoFrequencia()
                        {
                            Estado = "RS",
                            Frequencia = Convert.ToInt32(totalFrequenciaRs),
                            Nome = nome
                        };
                        lista.Add(objRs);
                        break;
                }
            }

            foreach (var item in lista)
            {
                Console.WriteLine($"O nome {item.Nome} é mais comum no estado de {item.Estado} com frenquencia de: {item.Frequencia}");
            }

        }
        public static string GetUf()
        {
            var client = new RestClient("https://servicodados.ibge.gov.br/api/v1/localidades/estados");
            var request = new RestRequest();
            RestResponse response = client.Execute(request);

            return response.Content;
        }
        public static string GetNome(string nome, string uf)
        {
            var client = new RestClient("https://servicodados.ibge.gov.br/api/v2/censos/nomes/" + nome + "?localidade=" + uf);
            var request = new RestRequest();
            RestResponse response = client.Execute(request);

            return response.Content;
        }
    }
}
