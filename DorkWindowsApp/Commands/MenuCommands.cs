using System.Windows.Input;

namespace DorkWindowsApp.Commands
{
    public class MenuCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof (MenuCommands), new InputGestureCollection{new KeyGesture(Key.F4, ModifierKeys.Alt)});
    }
}
