using Microsoft.AspNetCore.Components.Web;

namespace CompoRoteiro.Shared
{
    public partial class EditorCena
    {
        private string key = string.Empty;
        private void atualizar(KeyboardEventArgs e)
        {
            key = e.Key;
            StateHasChanged();
        }
    }
}
