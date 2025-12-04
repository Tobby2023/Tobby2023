using System.Diagnostics;

namespace CSharpPython;

class Program
{
    static void Main(string[] args)
    {
        // Caminho para o arquivo Python que você deseja executar
        string scriptPath = @"..\..\..\ScriptPython.py";

        // Caminho para o interpretador Python (substitua conforme necessário)
        string pythonPath = @"C:\Users\X280i5\AppData\Local\Microsoft\WindowsApps\python3.11.exe";

        // Verificar se os arquivos existem
        if (!System.IO.File.Exists(scriptPath))
        {
            Console.WriteLine("O arquivo de script Python não foi encontrado.");
            return;
        }

        if (!System.IO.File.Exists(pythonPath))
        {
            Console.WriteLine("O interpretador Python não foi encontrado.");
            return;
        }

        // Número a ser passado para o script Python
        int number = 5; // Exemplo de número

        // Configura o processo para executar o script Python com o número como parâmetro
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = pythonPath,
            Arguments = $"\"{scriptPath}\" \"{number}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                // Armazena a saída em um array de strings
                string[] stringArray = output.Split(',');

                // Exibe os elementos armazenados (para verificar se está funcionando)
                Console.WriteLine("Array retornado do Python:");
                foreach (var item in stringArray)
                {
                    Console.WriteLine(item.Trim()); // Remove espaços em branco extra
                }
            }
            else
            {
                Console.WriteLine("Erro ao executar o script Python.");
                Console.WriteLine("Erro:");
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exceção ao executar o script Python: {ex.Message}");
        }
    }
}
