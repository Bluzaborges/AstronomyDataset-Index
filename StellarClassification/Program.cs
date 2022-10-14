namespace StellarClassification;

public static class Program
{
    public static void Main(string[] args)
    {
        ShowData();
    }

    private static void AssembleFile()
    {
        var list = new List<string[]>();
        using var stream = new StreamReader("C:\\Users\\mohrdi\\Downloads\\archive (2)\\star_classification.csv");
        var header = stream.ReadLine();
        header = header.Replace("obj_ID", "id");
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            var values = line.Split(',').Skip(1).ToArray();
            list.Add(values);
        }

        using var writer = new StreamWriter("C:\\Users\\mohrdi\\Downloads\\archive (2)\\star_classification_mod.csv");
        writer.WriteLine(header);
        for (int i = 0; i < list.Count; i++)
            writer.WriteLine($"{i},{string.Join(',', list[i])}");
    }

    private static void ShowData()
    {
        using var reader = new StreamReader("C:\\Users\\mohrdi\\Downloads\\archive (2)\\star_classification_mod.csv");
        var header = reader.ReadLine();

        while (!reader.EndOfStream)
        {
            Console.WriteLine(reader.ReadLine());
        }
    }
}