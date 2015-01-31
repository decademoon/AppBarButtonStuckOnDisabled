using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AppBarButtonStuckOnDisabled
{
    class DelegateCommand : ICommand
    {
        Action execute;
        Func<bool> canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
 	        return canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
 	        execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    class ViewModel
    {
        public DelegateCommand Task1Command { get; set; }
        public DelegateCommand Task2Command { get; set; }

        bool task1Loading;
        bool task2Loading;

        public ViewModel()
        {
            Task1Command = new DelegateCommand(task1, () => !task1Loading);
            Task2Command = new DelegateCommand(task2, () => !task2Loading);
        }

        async void task1()
        {
            task1Loading = true;
            Task1Command.RaiseCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine("Task1 start");
            await Task.Delay(2000);
            System.Diagnostics.Debug.WriteLine("Task1 finish");

            task1Loading = false;
            Task1Command.RaiseCanExecuteChanged();
        }

        async void task2()
        {
            task2Loading = true;
            Task2Command.RaiseCanExecuteChanged();

            System.Diagnostics.Debug.WriteLine("Task2 start");
            await Task.Delay(2000);
            System.Diagnostics.Debug.WriteLine("Task2 finish");

            task2Loading = false;
            Task2Command.RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        public Page1()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // "fix" the second button
            but2.ClearValue(AppBarButton.IsEnabledProperty);
        }
    }
}
