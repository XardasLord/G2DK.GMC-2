using System.Threading.Tasks;
using System.Windows.Input;

namespace GothicModComposer.UI.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
}