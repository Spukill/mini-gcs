using System.Windows;

namespace MiniGcs.App;

// Code-behind for App.xaml. A XAML file and its .xaml.cs are two halves of the SAME
// class: `partial` lets a C# class be split across files, and at build time the XAML
// is turned into C# (obj/.../App.g.cs) and merged with this file. Empty for now; later
// this is where startup logic goes (e.g. creating services before the window opens).
public partial class App : Application
{
}
