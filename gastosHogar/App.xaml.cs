using gastos_hogar.Business;
using gastos_hogar.Dtos;

namespace gastosHogar
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }
        public static SaldoDto SaldoActivo { get; set; }
        public App()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            services.AddSingleton<GastoBO>();


            Services = services.BuildServiceProvider();

            var appData = App.Services.GetRequiredService<GastoBO>();
            SaldoActivo = appData.GetSaldoActivo();

            MainPage = new AppShell();
        }
    }
}
