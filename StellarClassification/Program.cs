namespace StellarClassification;

public struct Data
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

public struct Index
{
    public int Address;
    public decimal Value;
}

public static class Program
{
    public static void Main(string[] args)
    {
        int option = 0;

        int DataRegisterSize = SaveAsBinary();
        int RedshiftIndexRegisterSize = RedshiftIndex(DataRegisterSize);

        while (option != 5)
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
                    SearchForId(DataRegisterSize);
                    break;
                case 3:
                    ShowRedshiftIndex();
                    break;
                case 4:
                    SearchForRedshiftValue(RedshiftIndexRegisterSize, DataRegisterSize);
                    break;
            }
        }
    }

    private static int SaveAsBinary()
    {

        Console.WriteLine("Convertendo dataset...");

        using var reader = new StreamReader("files\\star_classification.csv");
        using var binaryStream = File.Create("files\\star_classification.dat");
        using var writer = new BinaryWriter(binaryStream);

        var header = reader.ReadLine();
        int registerSize = 0;

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',').Skip(0).ToArray();

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].PadLeft(20);
            }

            writer.Write(string.Join(',', values));

            registerSize = System.Text.ASCIIEncoding.ASCII.GetByteCount(string.Join(',', values));
        }

        Console.WriteLine("Arquivo binário do dataset criado com sucesso.");
        Console.Clear();

        return registerSize + 2;
    }

    private static int RedshiftIndex(int DataRegistrySize)
    {

        Console.WriteLine("Criando índice do campo redshift...");

        using var dataBinaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
        using var dataReader = new BinaryReader(dataBinaryStream);

        var header = dataReader.ReadString();

        long numberOfRegisters = dataReader.BaseStream.Length / DataRegistrySize;

        Index[] redShiftIndex = new Index[numberOfRegisters];

        int i = 0;
        while (dataReader.BaseStream.Position != dataReader.BaseStream.Length)
        {
            var line = dataReader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();

            redShiftIndex[i].Address = int.Parse(values[0]);
            redShiftIndex[i].Value = Decimal.Parse(values[14], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);

            i++;
        }

        MergeSort(redShiftIndex, 0, (int)numberOfRegisters - 1);

        using var RedshiftIndex = File.Create("files\\red_shift_index.dat");
        using var indexWriter = new BinaryWriter(RedshiftIndex);

        int sizeOfRegister = 0;

        for (i = 0; i < numberOfRegisters - 1; i++)
        {
            string register = $"{redShiftIndex[i].Address.ToString().PadLeft(5)};{redShiftIndex[i].Value.ToString().PadLeft(20)}";
            indexWriter.Write(register);
            sizeOfRegister = System.Text.ASCIIEncoding.ASCII.GetByteCount(register);
        }

        Console.WriteLine("Índice de redshift criado com sucesso.");
        Console.Clear();

        return sizeOfRegister + 1;
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

        Data data = new Data();

        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        Console.WriteLine("   ID            CLASSE         REDSHIFT                    MJD      ALPHA             DELTA");

        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            var line = reader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();

            data.Id = int.Parse(values[0]);
            data.Alpha = values[1];
            data.Delta = values[2];
            //data.U = values[3];
            //data.G = values[4];
            //data.R = values[5];
            //data.I = values[6];
            //data.Z = values[7];
            //data.Run_id = values[8];
            //data.Rerun_id = values[9];
            //data.Cam_col = values[10];
            //data.Field_id = values[11];
            //data.Spec_obj_id = values[12];
            data.Class = values[13];
            data.Redshift = values[14];
            //data.Plate = values[15];
            data.MJD = values[16];
            //data.Fiber_id = values[17];

            Console.WriteLine($"{data.Id.ToString().PadLeft(5)}{data.Class}{data.Redshift}{data.MJD}{data.Alpha}{data.Delta}");
        }

        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        Console.WriteLine("Aperte qualquer tecla para continuar");
        Console.ReadLine();
        Console.Clear();
    }

    private static void SearchForId(int TamanhoRegistro)
    {
        using var binaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
        using var reader = new BinaryReader(binaryStream);

        Console.Write("Digite o ID: ");
        int Id = int.Parse(Console.ReadLine());

        var header = reader.ReadString();

        if (BinarySearchForId(reader, Id, 0, reader.BaseStream.Length / TamanhoRegistro - 1, TamanhoRegistro) == -1)
        {
            Console.WriteLine("ID não encontrado.");
        }

        Console.WriteLine("Aperte qualquer tecla para continuar");
        Console.ReadLine();
        Console.Clear();
    }

    private static void ShowRedshiftIndex()
    {
        using var binaryStream = File.Open("files\\red_shift_index.dat", FileMode.Open);
        using var reader = new BinaryReader(binaryStream);

        Console.WriteLine("------------------------------");

        Console.WriteLine("    ID        REDSHIFT");

        while (reader.BaseStream.Position != reader.BaseStream.Length)
        {
            var line = reader.ReadString();
            var values = line.Split(';').Skip(0).ToArray();

            Console.WriteLine($"{values[0].PadLeft(6)}{values[1]}");
        }

        Console.WriteLine("------------------------------");
        Console.WriteLine("Aperte qualquer tecla para continuar");
        Console.ReadLine();
        Console.Clear();
    }

    private static void SearchForRedshiftValue(int RedshiftIndexRegisterSize, int DataRegisterSize)
    {
        using var indexBinaryStream = File.Open("files\\red_shift_index.dat", FileMode.Open);
        using var indexReader = new BinaryReader(indexBinaryStream);
        using var dataBinaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
        using var dataReader = new BinaryReader(dataBinaryStream);

        Console.Write("Digite um valor de redshit: ");
        decimal redShift = decimal.Parse(Console.ReadLine());

        int foundId = RedshiftBinarySearch(indexReader, redShift, 0, indexReader.BaseStream.Length / RedshiftIndexRegisterSize - 1, RedshiftIndexRegisterSize);

        if (foundId != -1)
        {
            dataReader.BaseStream.Seek(foundId * DataRegisterSize, SeekOrigin.Begin);
            var line = dataReader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();
            WriteRegister(foundId, values[13], values[14], values[16], values[1], values[2]);
        } else
        {
            Console.WriteLine("Valor nao encontrado");
        }

        Console.WriteLine("Aperte qualquer tecla para continuar");
        Console.ReadLine();
        Console.Clear();
    }

    private static void Merge(Index[] array, int start, int middle, int end)
    {
        int n1 = middle - start + 1;
        int n2 = end - middle;
        int i, j;
        Index[] arrayAux1 = new Index[n1];
        Index[] arrayAux2 = new Index[n2];

        for (i = 0; i < n1; ++i)
        {
            arrayAux1[i].Address = array[start + i].Address;
            arrayAux1[i].Value = array[start + i].Value;
        }

        for (j = 0; j < n2; ++j)
        {
            arrayAux2[j].Address = array[middle + 1 + j].Address;
            arrayAux2[j].Value = array[middle + 1 + j].Value;
        }

        i = 0;
        j = 0;
        int k = start;

        while(i < n1 && j < n2)
        {
            if (arrayAux1[i].Value <= arrayAux2[j].Value)
            {
                array[k].Value = arrayAux1[i].Value;
                array[k].Address = arrayAux1[i].Address;
                i++;
            } else
            {
                array[k].Value = arrayAux2[j].Value;
                array[k].Address = arrayAux2[j].Address;
                j++;
            }
            k++;
        }

        while(i < n1)
        {
            array[k].Value = arrayAux1[i].Value;
            array[k].Address = arrayAux1[i].Address;
            i++;
            k++;
        }

        while(j < n2)
        {
            array[k].Value = arrayAux2[j].Value;
            array[k].Address = arrayAux2[j].Address;
            j++;
            k++;
        }
        
    }

    private static void MergeSort(Index[] array, int start, int end)
    {
        if (start < end)
        {
            int middle = start + (end - start) / 2;

            MergeSort(array, start, middle);
            MergeSort(array, middle + 1, end);
            Merge(array, start, middle, end);
        }
    }

    private static int BinarySearchForId(BinaryReader reader, int id, long inferior, long superior, int registerSize)
    {

        if (superior >= inferior)
        {
            long metade = inferior + (superior - inferior) / 2;

            reader.BaseStream.Seek(metade * registerSize, SeekOrigin.Begin);

            var line = reader.ReadString();
            var values = line.Split(',').Skip(0).ToArray();
            int foundId = int.Parse(values[0]);

            if (foundId == id)
            {
                WriteRegister(foundId, values[13], values[14], values[16], values[1], values[2]);
                return 1;
            }

            if (foundId > id)
            {
                return BinarySearchForId(reader, id, inferior, metade - 1, registerSize);
            }

            return BinarySearchForId(reader, id, metade + 1, superior, registerSize);
        }

        return -1;
    }

    private static int RedshiftBinarySearch(BinaryReader reader, decimal redShift, long inferior, long superior, int registerSize)
    {

        if (superior >= inferior)
        {
            long metade = inferior + (superior - inferior) / 2;

            reader.BaseStream.Seek(metade * registerSize, SeekOrigin.Begin);

            var line = reader.ReadString();
            var values = line.Split(';').Skip(0).ToArray();
            decimal foundRedShift = decimal.Parse(values[1]);

            if (foundRedShift == redShift)
            {
                return int.Parse(values[0]);
            }

            if (foundRedShift > redShift)
            {
                return RedshiftBinarySearch(reader, redShift, inferior, metade - 1, registerSize);
            }

            return RedshiftBinarySearch(reader, redShift, metade + 1, superior, registerSize);
        }

        return -1;
    }

    private static void WriteRegister(int id, string type, string redshift, string mjd, string alpha, string delta)
    {
        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        Console.WriteLine("   ID            CLASSE         REDSHIFT                    MJD      ALPHA             DELTA");
        Console.WriteLine($"{id.ToString().PadLeft(5)}{type}{redshift}{mjd}{alpha}{delta}");
        Console.WriteLine("---------------------------------------------------------------------------------------------------------");
    }

    private static void Menu()
    {
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("1. Mostrar todos os dados.");
        Console.WriteLine("2. Pesquisar por id.");
        Console.WriteLine("3. Mostrar indice de redshift.");
        Console.WriteLine("4. Pesquisar por valor de redshift.");
        Console.WriteLine("5. Sair.");
        Console.WriteLine("-------------------------------------------");
    }
}