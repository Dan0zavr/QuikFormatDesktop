using PDFReader;


try
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;

    if (args.Length < 3)
    {
        Console.Error.WriteLine("Недостаточно аргументов");
        return 1;
    }

    string mode = args[0];
    string inputFile = args[1];
    string outputDir = args[2];

    Enum.TryParse(mode, true, out Priority priority);

    string result = PDFReaderEntry.Convert(inputFile, outputDir, priority);

    Console.WriteLine(result);

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Ошибка: {ex.Message}");
    return 1;
}
