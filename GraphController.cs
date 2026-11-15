using CampusNet.Models;
using CampusNet.Views;

namespace CampusNet.Controllers;

public sealed class GraphController
{
    private readonly Graph _graph = new();
    private readonly ConsoleView _view = new();

    public void Run()
    {
        BuildInitialNetwork();
        _view.Header("CAMPUSNET - RED SOCIAL ACADÉMICA");
        _view.Message("A → B significa que A sigue a B. La relación no tiene que ser recíproca.");
        _view.ShowAdjacency(_graph);
        foreach (string start in new[] { "U01", "U04", "U06" })
            _view.ShowTraversal($"BFS DESDE {start}", _graph.BreadthFirstSearch(start), _graph);
        _view.ShowDfs(_graph.DepthFirstSearchAll());
        _view.ShowVertices("USUARIOS SIN SEGUIDORES (GRADO DE ENTRADA 0)", _graph.GetVerticesWithZeroInDegree());
        _view.ShowVertices("USUARIOS MÁS INFLUYENTES (MAYOR GRADO DE ENTRADA)", _graph.GetMostInfluential(), v => _graph.InDegree(v.Id));
        _view.ShowVertices("USUARIOS MÁS ACTIVOS (MAYOR GRADO DE SALIDA)", _graph.GetMostActive(), v => _graph.OutDegree(v.Id));
        _view.Header("CONSULTA DE ALCANZABILIDAD");
        _view.Message($"¿U01 puede llegar a U12? {(_graph.CanReach("U01", "U12") ? "Sí" : "No")}");
        DemonstrateCrud();
        _view.Header("RESUMEN FINAL");
        _view.Message($"Usuarios: {_graph.VertexCount} | Relaciones: {_graph.EdgeCount}");
    }

    private void BuildInitialNetwork()
    {
        var users = new[]
        {
            new Vertex("U01", "Ana Torres", "Estudiante"),
            new Vertex("U02", "Carlos Ruiz", "Estudiante"),
            new Vertex("U03", "Laura Gómez", "Profesor"),
            new Vertex("U04", "Mateo Díaz", "Estudiante"),
            new Vertex("U05", "Sofía Marín", "Egresado"),
            new Vertex("U06", "Andrés López", "Profesor"),
            new Vertex("U07", "Valentina Ríos", "Estudiante"),
            new Vertex("U08", "Daniel Castro", "Estudiante"),
            new Vertex("U09", "Camila Pérez", "Egresado"),
            new Vertex("U10", "Santiago Mora", "Profesor"),
            new Vertex("U11", "Isabella Gil", "Estudiante"),
            new Vertex("U12", "Nicolás Vélez", "Egresado")
        };
        foreach (var user in users) _graph.AddVertex(user);

        (string From, string To)[] edges =
        {
            ("U01","U02"), ("U01","U03"), ("U01","U04"), ("U01","U05"),
            ("U02","U03"), ("U02","U06"), ("U02","U07"), ("U02","U08"),
            ("U03","U01"), ("U03","U05"), ("U03","U08"),
            ("U04","U05"), ("U04","U06"), ("U05","U02"), ("U05","U09"),
            ("U06","U03"), ("U06","U07"), ("U06","U10"),
            ("U07","U08"), ("U08","U09"), ("U10","U01")
        };
        foreach (var (from, to) in edges) _graph.AddEdge(from, to);
    }

    private void DemonstrateCrud()
    {
        _view.Header("OPERACIONES CRUD");
        _view.Message("Agregar usuario U13:");
        _view.Message(_graph.AddVertex(new Vertex("U13", "Mariana Ospina", "Estudiante")) ? "Usuario agregado." : "No fue posible agregarlo.");
        _view.ShowAdjacency(_graph);

        _view.Message("Actualizar U13 a rol Profesor:");
        _view.Message(_graph.UpdateVertex("U13", "Mariana Ospina", "Profesor") ? "Usuario actualizado." : "No fue posible actualizarlo.");
        _view.ShowAdjacency(_graph);

        _view.Message("Agregar relación U13 → U01:");
        _view.Message(_graph.AddEdge("U13", "U01") ? "Relación agregada." : "No fue posible agregarla.");
        _view.ShowAdjacency(_graph);

        _view.Message("Eliminar relación U13 → U01:");
        _view.Message(_graph.RemoveEdge("U13", "U01") ? "Relación eliminada." : "No se encontró la relación.");
        _view.ShowAdjacency(_graph);

        _view.Message("Eliminar usuario U13:");
        _view.Message(_graph.RemoveVertex("U13") ? "Usuario eliminado." : "No se encontró el usuario.");
        _view.ShowAdjacency(_graph);
    }
}
