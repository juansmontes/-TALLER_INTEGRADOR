namespace CampusNet.Models;

public sealed class Graph
{
    private readonly Dictionary<string, Vertex> _vertices = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<string>> _adjacency = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyDictionary<string, Vertex> Vertices => _vertices;
    public int VertexCount => _vertices.Count;
    public int EdgeCount => _adjacency.Values.Sum(list => list.Count);

    public bool AddVertex(Vertex vertex)
    {
        ArgumentNullException.ThrowIfNull(vertex);
        if (_vertices.ContainsKey(vertex.Id)) return false;
        _vertices.Add(vertex.Id, vertex);
        _adjacency.Add(vertex.Id, new List<string>());
        return true;
    }

    public bool RemoveVertex(string id)
    {
        if (!_vertices.Remove(id)) return false;
        _adjacency.Remove(id);
        foreach (var neighbors in _adjacency.Values)
            neighbors.RemoveAll(x => string.Equals(x, id, StringComparison.OrdinalIgnoreCase));
        return true;
    }

    public bool UpdateVertex(string id, string? name, string? role)
    {
        if (!_vertices.TryGetValue(id, out var vertex)) return false;
        vertex.Update(name, role);
        return true;
    }

    public bool AddEdge(string fromId, string toId)
    {
        if (!_adjacency.TryGetValue(fromId, out var neighbors) || !_vertices.ContainsKey(toId)) return false;
        if (neighbors.Contains(toId, StringComparer.OrdinalIgnoreCase)) return false;
        neighbors.Add(toId);
        return true;
    }

    public bool RemoveEdge(string fromId, string toId) =>
        _adjacency.TryGetValue(fromId, out var neighbors) &&
        neighbors.RemoveAll(x => string.Equals(x, toId, StringComparison.OrdinalIgnoreCase)) > 0;

    public IReadOnlyList<string> GetNeighbors(string id) =>
        _adjacency.TryGetValue(id, out var neighbors) ? neighbors.AsReadOnly() : Array.Empty<string>();

    public int InDegree(string id) => _adjacency.Values.Sum(list =>
        list.Count(x => string.Equals(x, id, StringComparison.OrdinalIgnoreCase)));

    public int OutDegree(string id) => _adjacency.TryGetValue(id, out var neighbors) ? neighbors.Count : 0;

    public IReadOnlyList<Vertex> GetVerticesWithZeroInDegree() =>
        _vertices.Values.Where(v => InDegree(v.Id) == 0).ToList();

    public IReadOnlyList<Vertex> GetMostInfluential()
    {
        if (_vertices.Count == 0) return Array.Empty<Vertex>();
        int max = _vertices.Keys.Max(InDegree);
        return _vertices.Values.Where(v => InDegree(v.Id) == max).ToList();
    }

    public IReadOnlyList<Vertex> GetMostActive()
    {
        if (_vertices.Count == 0) return Array.Empty<Vertex>();
        int max = _vertices.Keys.Max(OutDegree);
        return _vertices.Values.Where(v => OutDegree(v.Id) == max).ToList();
    }

    public List<string> BreadthFirstSearch(string startId)
    {
        if (!_vertices.ContainsKey(startId)) return new List<string>();
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { startId };
        var queue = new Queue<string>();
        var order = new List<string>();
        queue.Enqueue(startId);
        while (queue.Count > 0)
        {
            string current = queue.Dequeue();
            order.Add(current);
            foreach (string neighbor in _adjacency[current])
                if (visited.Add(neighbor)) queue.Enqueue(neighbor);
        }
        return order;
    }

    public (List<string> Order, List<string> Cycles) DepthFirstSearchAll()
    {
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var active = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var order = new List<string>();
        var cycles = new List<string>();

        void Visit(string current, List<string> path)
        {
            visited.Add(current);
            active.Add(current);
            order.Add(current);
            path.Add(current);
            foreach (string neighbor in _adjacency[current])
            {
                if (!visited.Contains(neighbor)) Visit(neighbor, path);
                else if (active.Contains(neighbor))
                {
                    int index = path.FindIndex(x => string.Equals(x, neighbor, StringComparison.OrdinalIgnoreCase));
                    string cycle = index >= 0 ? string.Join(" → ", path.Skip(index).Append(neighbor)) : $"{current} → {neighbor}";
                    if (!cycles.Contains(cycle, StringComparer.OrdinalIgnoreCase)) cycles.Add(cycle);
                }
            }
            path.RemoveAt(path.Count - 1);
            active.Remove(current);
        }

        foreach (string id in _vertices.Keys)
            if (!visited.Contains(id)) Visit(id, new List<string>());
        return (order, cycles);
    }

    public bool CanReach(string fromId, string toId) =>
        BreadthFirstSearch(fromId).Any(id => string.Equals(id, toId, StringComparison.OrdinalIgnoreCase));
}
