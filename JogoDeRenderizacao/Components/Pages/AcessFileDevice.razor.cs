#if ANDROID
using static Android.Provider.MediaStore;
#endif

namespace JogoDeRenderizacao.Components.Pages
{
    public partial class AcessFileDevice
    {
        private string fileName { get; set; } = "download (1).png";
        private string result { get; set; } = string.Empty;
        private string imageSrc { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            var writeStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();
            var readStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

            if (readStatus != PermissionStatus.Granted || writeStatus != PermissionStatus.Granted)
            {
                // Permissões negadas
                // Console.WriteLine("Permissões de armazenamento negadas.");
                await App.Current!.MainPage!.DisplayAlert("Error", "Permissões de armazenamento negadas.", "Ok");
            }
        }

        public async Task ReadFileFromAndroidFolder()
        {
            try
            {
                string filePath = string.Empty;
#if ANDROID
                var externalStoragePath = Android.OS.Environment.ExternalStorageDirectory!.AbsolutePath;
                filePath = Path.Combine(externalStoragePath, "Android", fileName);
#else
            var androidFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Android");
            filePath = Path.Combine(androidFolderPath, fileName);
#endif
                result = filePath + "\n";
                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    //result += await File.ReadAllTextAsync(filePath);

                    // Converte os bytes para Base64
                    var base64 = Convert.ToBase64String(bytes);

                    // Monta a URL da imagem
                    imageSrc = $"data:image/png;base64,{base64}";

                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    result += "Arquivo não encontrado.";
                }
            }
            catch (Exception ex)
            {
                await App.Current!.MainPage!.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
