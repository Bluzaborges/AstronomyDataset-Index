using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarClassification
{
    public class ListIndex
    {
        public string Tipo { get; set; }
        public List<int> lstId = new List<int>();

        public ListIndex(Data strcData)
        {
            this.Tipo = strcData.Class;
            this.lstId.Add(strcData.Id);
        }

        public static void carregaDados(List<ListIndex> lstId)
        {
            Console.WriteLine("Criando índice do campo classe...");

            using var binaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
            using var reader = new BinaryReader(binaryStream);

            var header = reader.ReadString();

            Data data = new Data();

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var line = reader.ReadString();
                var values = line.Split(',').Skip(0).ToArray();

                data.Id = int.Parse(values[0]);
                data.Class = values[13].Trim().Replace("/", "").Replace("\"", "");

                if (lstId.Exists(indice => indice.Tipo == data.Class))
                {
                    lstId.Find(indice => indice.Tipo == data.Class).lstId.Add(data.Id);
                }
                else
                {
                    lstId.Add(new ListIndex(data));
                }

            }

            Console.WriteLine("Índice de classe criado com sucesso.");
            Console.Clear();
        }
    }
}
