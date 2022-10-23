using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarClassification
{
    public class LinkedListIndex
    {
        public string Tipo { get; set; }
        public List<int> lstId = new List<int>();

        public LinkedListIndex(Data strcData)
        {
            this.Tipo = strcData.Class;
            this.lstId.Add(strcData.Id);
        }

        public static void carregaDados(List<LinkedListIndex> lstId)
        {
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
                    lstId.Add(new LinkedListIndex(data));
                }

            }
        }
    }
}
