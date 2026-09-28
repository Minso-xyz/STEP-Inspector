using STEPInspector;
using System.Text.Json;

if (args.Length ==0)
{
    Console.WriteLine("STEP file path is required.");
    return;
}

string filePath = @"ravioli.stp";

StepAnalyzer analyzer = new StepAnalyzer();
StepMetadata metadata = analyzer.Analyze(filePath);

string json = JsonSerializer.Serialize(metadata);