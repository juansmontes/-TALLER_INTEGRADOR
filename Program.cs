using CampusNet.Controllers;

namespace CampusNet;

internal static class Program
{
    private static void Main()
    {
        var controller = new GraphController();
        controller.Run();
    }
}
