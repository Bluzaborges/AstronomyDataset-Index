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