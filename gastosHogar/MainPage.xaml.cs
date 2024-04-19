using gastos_hogar;
using gastos_hogar.Business;
using gastos_hogar.Dtos;
using System.Globalization;

namespace gastosHogar
{
    public partial class MainPage : ContentPage
    {
        SaldoDto saldoInfo;
        GastoBO pGastoBo;


        public MainPage()
        {
            InitializeComponent();
            pGastoBo = App.Services.GetRequiredService<GastoBO>();
            saldoInfo = App.SaldoActivo;

            var cultureInfo = new CultureInfo("es-CL");


            // Formatear el valor como una cadena en el formato de moneda chilena, incluyendo el símbolo de la moneda
            var valorFormateado = string.Format(cultureInfo, "{0:C}", saldoInfo.Saldo);

            txtMainSaldo.FormattedText = new FormattedString
            {
                Spans =
            {
                new Span { Text = "Saldo Actual", FontAttributes = FontAttributes.None },
                new Span { Text = "\n", FontAttributes = FontAttributes.None }, // Añadir un salto de línea
                new Span { Text = valorFormateado, FontAttributes = FontAttributes.Bold }


            }
            };
        }

        async void btnCapital_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CapitalView());
        }

        async void btnEgreso_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EgresosPage());
        }
    }

}
