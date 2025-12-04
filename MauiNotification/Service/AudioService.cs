using System.Reflection;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using Vosk;

namespace MauiNotification.Service
{
    public class AudioService : IDisposable
    {
        private VoskRecognizer _voskRecognizer;
        private SpeechRecognitionEngine _windowsRecognizer;
        private SpeechSynthesizer _synthesizer;
        private string _partialResult = string.Empty;

        public event Action<string> RecognitionResultUpdated;
        public event Action<string> FinalResultReceived;

        public AudioService()
        {
            InitializeVosk();
            InitializeSynthesizer();
        }

        public async Task InitializeVosk()
        {
            try
            {
                // Configura o caminho para as DLLs nativas
                Environment.SetEnvironmentVariable("VOSK_PATH",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "vosk"));

                // Verifica se o modelo já foi copiado
                var targetDir = Path.Combine(FileSystem.Current.AppDataDirectory, "models");
                var modelTargetPath = Path.Combine(targetDir, "vosk-model-small-pt-0-3");

                if (Directory.Exists(modelTargetPath))
                {
                    await CopyModelToAppData();
                }

                // Força o carregamento da biblioteca nativa
                LoadNativeLibrary();

                Model model = new Model(modelTargetPath);
                _voskRecognizer = new VoskRecognizer(model, 16000.0f);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar Vosk: {ex}");
                throw;  // Re-lança a exceção para depuração
            }
        }

        private void LoadNativeLibrary()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var dllName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "vosk.dll" :
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "libvosk.so" :
                         "libvosk.dylib";

            var resourcePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(dllName));

            if (resourcePath != null)
            {
                using var stream = assembly.GetManifestResourceStream(resourcePath);
                using var memStream = new MemoryStream();
                stream.CopyTo(memStream);

                var tempPath = Path.Combine(Path.GetTempPath(), dllName);
                File.WriteAllBytes(tempPath, memStream.ToArray());
                NativeLibrary.Load(tempPath);
            }
        }

        private async Task CopyModelToAppData()
        {
            var targetDir = Path.Combine(FileSystem.Current.AppDataDirectory, "models", "vosk-model-small-pt-0-3");
            Directory.CreateDirectory(targetDir);

            // Usando FileSystemUtils para copiar toda a pasta recursivamente
            var sourceDir = Path.Combine(Environment.CurrentDirectory, "Resources", "Raw", "models", "vosk-model-small-pt-0-3");

            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, targetDir));
            }

            foreach (string newPath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourceDir, targetDir), true);
            }
        }

        private void InitializeSynthesizer()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();

                // Configuração da voz em português
                foreach (var voice in _synthesizer.GetInstalledVoices())
                {
                    if (voice.VoiceInfo.Culture.Name.StartsWith("pt"))
                    {
                        _synthesizer.SelectVoice(voice.VoiceInfo.Name);
                        break;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public async Task StartListeningAsync()
        {
            _windowsRecognizer = new SpeechRecognitionEngine();
            _windowsRecognizer.SetInputToDefaultAudioDevice();

            var grammar = new DictationGrammar();
            _windowsRecognizer.LoadGrammar(grammar);

            _windowsRecognizer.SpeechRecognized += (s, e) =>
            {
                FinalResultReceived?.Invoke(e.Result.Text);
            };

            _windowsRecognizer.SpeechHypothesized += (s, e) =>
            {
                RecognitionResultUpdated?.Invoke(e.Result.Text);
            };

            _windowsRecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Speak(string text)
        {
            _synthesizer.SpeakAsync(text);
        }

        public void StopListening()
        {
            // Implementação para parar o reconhecimento
        }
        public void SetRate(int rate)
        {
            _synthesizer.Rate = rate - 100; // -10 a 10
        }

        public void SetVolume(int volume)
        {
            _synthesizer.Volume = volume;
        }

        public void Dispose()
        {
            _voskRecognizer?.Dispose();
            _synthesizer?.Dispose();
            _windowsRecognizer?.Dispose();
        }
    }
}
