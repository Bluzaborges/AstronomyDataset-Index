namespace StellarClassification;

public struct Dados
{
    public int Id { get; set; }
    public string Alpha { get; set; }
    public string Delta { get; set; }
    public string U { get; set; }
    public string G { get; set; }
    public string R { get; set; }
    public string I { get; set; }
    public string Z { get; set; }
    public string Run_id { get; set; }
    public string Rerun_id { get; set; }
    public string Cam_col { get; set; }
    public string Field_id { get; set; }
    public string Spec_obj_id { get; set; }
    public string Class { get; set; }
    public string Redshift { get; set; }
    public string Plate { get; set; }
    public string MJD { get; set; }
    public string Fiber_id { get; set; }

}

public static class Program
{
    public static void Main(string[] args)
    {
        int option = 0;

        int TamanhoRegistro = SaveAsBinary();

        while (option != 3)
        {
            Menu();
            Console.Write("Digite a opção: ");
            option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    ShowData();
                    break;
                case 2:
                    SearchForId(TamanhoRegistro);
                    break;
            }
        }
    }

    private static void AssembleFile()
    {
        var list = new List<string[]>();
        using var stream = new StreamReader("files\\star_classification.csv");
        var header = stream.ReadLine();
        header = header.Replace("obj_ID", "id");
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            var values = line.Split(',').Skip(1).ToArray();
            list.Add(values);
        }

        using var writer = new StreamWriter("files\\star_classification_mod.csv");
        writer.WriteLine(header);
        for (int i = 0; i < list.Count; i++)
            writer.WriteLine($"{i},{string.Join(',', list[i])}");
    }

    private static void ShowData()
    {
        using var binaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
        using var reader = new BinaryReader(binaryStream);

        var header = reader.ReadString();

        Dados dados = new Dados();

        Console.WriteLine("   ID    ALPHA              DELTA                   REDSHIFT                 CLASSE");

        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            var line = reader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();

            dados.Id = int.Parse(values[0]);
            dados.Alpha = values[1];
            dados.Delta = values[2];
            dados.U = values[3];
            dados.G = values[4];
            dados.R = values[5];
            dados.I = values[6];
            dados.Z = values[7];
            dados.Run_id = values[8];
            dados.Rerun_id = values[9];
            dados.Cam_col = values[10];
            dados.Field_id = values[11];
            dados.Spec_obj_id = values[12];
            dados.Class = values[13];
            dados.Redshift = values[14];
            dados.Plate = values[15];
            dados.MJD = values[16];
            dados.Fiber_id = values[17];

            Console.WriteLine($"{dados.Id.ToString().PadLeft(5)}{dados.Alpha}{dados.Delta}{dados.Redshift}{dados.Class}");
        }
    }

    private static void Menu()
    {
        Console.WriteLine("1. Mostrar todos os dados.");
        Console.WriteLine("2. Pesquisar por id.");
        Console.WriteLine("3. Sair.");
    }

    private static void SearchForId(int TamanhoRegistro)
    {
        using var binaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
        using var reader = new BinaryReader(binaryStream);

        Console.Write("Digite o id: ");
        int Id = int.Parse(Console.ReadLine());

        var header = reader.ReadString();

        if (BinarySearch(reader, Id, 0, reader.BaseStream.Length / TamanhoRegistro - 1, TamanhoRegistro) == -1)
        {
            Console.WriteLine("Id nao encontrado");
        }
    }

    private static int SaveAsBinary()
    {
        using var reader = new StreamReader("files\\star_classification.csv");
        using var binaryStream = File.Create("files\\star_classification.dat");
        using var writer = new BinaryWriter(binaryStream);

        var header = reader.ReadLine();
        int TamanhoRegistro = 0;
        int cont = 0;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',').Skip(0).ToArray();

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].PadLeft(20);
            }

            writer.Write(string.Join(',', values));

            TamanhoRegistro = System.Text.ASCIIEncoding.ASCII.GetByteCount(string.Join(',', values));
        }

        Console.WriteLine("Arquivo binario criado com sucesso.");

        return TamanhoRegistro + 2;
    }

    private static int BinarySearch(BinaryReader reader, int Id, long Inferior, long Superior, int TamanhoRegistro)
    {

        if (Superior >= Inferior)
        {
            long metade = Inferior + (Superior - Inferior) / 2;

            reader.BaseStream.Seek(metade*TamanhoRegistro, SeekOrigin.Begin);

            var line = reader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();
            int foundId = int.Parse(values[0]);

            if (foundId == Id)
            {
                Console.WriteLine("   ID    ALPHA              DELTA                   REDSHIFT                 CLASSE");
                Console.WriteLine($"{foundId.ToString().PadLeft(5)}{values[1]}{values[2]}{values[14]}{values[13]}");
                return 1;
            }

            if (foundId > Id)
            {
                return BinarySearch(reader, Id, Inferior, metade - 1, TamanhoRegistro);
            }

            return BinarySearch(reader, Id, metade + 1, Superior, TamanhoRegistro);
        }

        return -1;
    }
}