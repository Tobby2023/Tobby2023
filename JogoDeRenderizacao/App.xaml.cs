namespace JogoDeRenderizacao
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            // Verificar se o URI é do seu esquema
            if (uri.Scheme.Equals("meuapp", StringComparison.OrdinalIgnoreCase))
            {
                // Extrair parâmetros do URI, se houver
                var parametros = System.Web.HttpUtility.ParseQueryString(uri.Query);

                // Navegar para uma página específica ou executar uma ação
                
                base.OnAppLinkRequestReceived(uri);
            }
        }
    }
}
