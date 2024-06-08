using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Panda5Maui.Views;
using Panda5Maui.Models;
using Panda5Maui.ViewModels;

namespace Panda5Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        HandleServices(builder);

        Routing.RegisterRoute(nameof(CalendarView), typeof(CalendarView));
        Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));

        return builder.Build();
    }

    private static void HandleServices(MauiAppBuilder builder)
    {
        //User1 "Dummy@Dummy.com", "sixcharacters"
        //User2 "fihahiv405@cnurbano.com", "sixcharacters"
        const string APIKEY = "AIzaSyB9NgkZ3aOPT-t5pmxnciQlgkODepubmXg";
        const string AUTHDOMAIN = "panda5-668c8.firebaseapp.com";

        FirebaseDataProvider firebaseDataProvider = new FirebaseDataProvider(APIKEY, AUTHDOMAIN);
        builder.Services.AddSingleton<ILoginRequiredService>(firebaseDataProvider);
        builder.Services.AddSingleton<IDatabaseProvider>(firebaseDataProvider);
        builder.Services.AddSingleton<IEventProvider>(services => new DatabaseEventProvider(services!.GetService<IDatabaseProvider>()));

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<CalendarViewModel>();

        builder.Services.AddSingleton(services => new LoginView()
        {
            BindingContext = services.GetService<LoginViewModel>()
        });
        builder.Services.AddSingleton(services => new RegisterView()
        {
            BindingContext = services.GetService<RegisterViewModel>()
        });
        builder.Services.AddSingleton(services => new CalendarView()
        {
            BindingContext = services.GetService<CalendarViewModel>()
        });
    }
}
