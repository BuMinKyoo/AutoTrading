using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using AutoGetMoney.View;
using AutoGetMoney.ViewModel;

namespace AutoGetMoney
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public new static App Current => (App)Application.Current;

        public App()
        {
            Services = ConfigureServices();
        }

        private IServiceProvider ConfigureServices()
        {
            // ServiceCollection
            // 의존성을 등록하기 위한 컨테이너. 이 컨테이너는 응용 프로그램이 실행되는 동안 사용될 서비스와 그 구현을 저장한다
            var services = new ServiceCollection();

            // AddSingleton
            // 해당 서비스가 싱글턴으로 동작하도록 지정한다.
            // 애플리케이션이 실행되는 동안 한 번만 생성되고, 이후 모든 요청에 대해 동일한 인스턴스가 사용되도록 한다

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();

            // Views
            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });

            return services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainView = Services.GetRequiredService<MainWindow>();
            mainView.Show();
        }
    }
}
