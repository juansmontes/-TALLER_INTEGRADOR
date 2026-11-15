using CampusNet.Models;

namespace CampusNet.Views;

public sealed class ConsoleView
{
    public void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 68));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 68));
    }
    public void Message(string message) => Console.WriteLine(message);

    public void ShowAdjacency(Graph graph)
    {
        Header("LISTA DE ADYACENCIA");
        foreach (var vertex in graph.Vertices.Values)
        {
            var neighbors = graph.GetNeighbors(vertex.Id);
            string list = neighbors.Count == 0 ? "(sin seguidos)" :
                string.Join(", ", neighbors.Select(id => $"{id} - {graph.Vertices[id].Name}"));
            Console.WriteLine($"{vertex.Id} - {vertex.Name} [{vertex.Role}] → {list}");
        }
        Console.WriteLine($"Total: {graph.VertexCount} usuarios | {graph.EdgeCount} relaciones");
    }

    public void ShowTraversal(string title, IReadOnlyList<string> order, Graph graph)
    {
        Header(title);
        Console.WriteLine("Orden de visita: " + string.Join(" → ", order));
        Console.WriteLine($"Vértices alcanzados: {order.Count}");
        Console.WriteLine("Nombres: " + string.Join(", ", order.Select(id => graph.Vertices[id].Name)));
    }

    public void ShowVertices(string title, IEnumerable<Vertex> vertices, Func<Vertex, int>? metric = null)
    {
        Header(title);
        var list = vertices.ToList();
        if (list.Count == 0) { Console.WriteLine("(ninguno)"); return; }
        foreach (var vertex in list)
            Console.WriteLine($"{vertex}{(metric is null ? "" : $" | métrica: {metric(vertex)}")}");
    }

    public void ShowDfs((List<string> Order, List<string> Cycles) result)
    {
        Header("DFS COMPLETO Y DETECCIÓN DE CICLOS");
        Console.WriteLine("Orden de descubrimiento: " + string.Join(" → ", result.Order));
        Console.WriteLine("Ciclos detectados:");
        if (result.Cycles.Count == 0) Console.WriteLine("No se detectaron ciclos.");
        else foreach (string cycle in result.Cycles) Console.WriteLine(" • " + cycle);
    }
}
