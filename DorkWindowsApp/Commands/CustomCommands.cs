using System.Windows.Input;

namespace DorkWindowsApp.Commands
{
    public class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof (CustomCommands), new InputGestureCollection{new KeyGesture(Key.F4, ModifierKeys.Alt)});
    }
}
