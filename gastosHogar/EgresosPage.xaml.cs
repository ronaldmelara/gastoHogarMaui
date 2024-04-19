using gastos_hogar.Business;
using gastos_hogar.Dtos;
using gastosHogar;

namespace gastos_hogar;

public partial class EgresosPage : ContentPage
{

	CategoriasBO pCategoriasBO;
	GastoBO pGastoBo;
	SaldoDto saldoInfo;
	public EgresosPage()
	{
		InitializeComponent();
		pCategoriasBO = new CategoriasBO();
		pGastoBo = App.Services.GetRequiredService<GastoBO>();
		saldoInfo = App.SaldoActivo;

		settings();
		events();
	}

	private void settings()
	{
		var categorias = pCategoriasBO.GetCategorias();
		ddlCategoriaGasto.ItemsSource = categorias;
		ddlCategoriaGasto.ItemDisplayBinding = new Binding("Categoria");
		ddlSubCategoriaGasto.IsEnabled = false;
		ddlSubCategoriaGasto.Items.Clear();

	}

	private void events()
	{
		ddlCategoriaGasto.SelectedIndexChanged += (sender, e) =>
		{
			if(ddlCategoriaGasto.SelectedItem != null)
			{
                var selectedItem = (CategoriaGastoDto)ddlCategoriaGasto.SelectedItem;

                var subCategorias = pCategoriasBO.GetSubCategorias(selectedItem.IdCtaegoria);
                ddlSubCategoriaGasto.IsEnabled = (subCategorias.Any());
                ddlSubCategoriaGasto.SelectedItem = null;
                if (subCategorias.Any())
                {
                    ddlSubCategoriaGasto.ItemsSource = subCategorias;
                    ddlSubCategoriaGasto.ItemDisplayBinding = new Binding("Categoria");
                }
            }
			
			
		};

	
		

	}

    void btnGuardar_Clicked(System.Object sender, System.EventArgs e)
    {
		if (Validate())
		{
            int idCategoria = (ddlSubCategoriaGasto.IsEnabled ? ((CategoriaGastoDto)ddlSubCategoriaGasto.SelectedItem).IdCtaegoria : ((CategoriaGastoDto)ddlCategoriaGasto.SelectedItem).IdCtaegoria);
            float monto = String.IsNullOrEmpty(txtMonto.Text) ? 0.0f : float.Parse(txtMonto.Text);
            DateTime fecha = dpFecha.Date;
            int idSaldo = saldoInfo.IdSaldo;


            ResultDto result = pGastoBo.PutEgreso(idCategoria, monto, fecha, idSaldo);
            if (!string.IsNullOrEmpty(result.Message))
            {
                ShowFloatingAlert(result.Message);
            }
            dpFecha.Date = DateTime.Today;
            ddlCategoriaGasto.SelectedItem = null;
            ddlSubCategoriaGasto.SelectedItem = null;
			ddlSubCategoriaGasto.IsEnabled = false;
            txtMonto.Text = string.Empty;
		}
		else
		{
			ShowFloatingAlert("Debe completar los campos");
		}
        

    }

	private bool Validate()
	{

		bool subCatSeleted = ddlSubCategoriaGasto.IsEnabled;



		return (ddlCategoriaGasto.SelectedItem != null && (subCatSeleted ? ddlSubCategoriaGasto.SelectedItem != null : true)
			&& !string.IsNullOrEmpty(txtMonto.Text));
	}

    private async void ShowFloatingAlert(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            // Mostrar la alerta flotante
            await App.Current.MainPage.DisplayAlert("Alerta", message, "OK");
        });
    }
}
