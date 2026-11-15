# CampusNet — Grafos dirigidos (MVC en C#)

**Universidad de Manizales — Facultad de Ciencias e Ingeniería**  
**Asignatura:** Programación III  
**Actividad:** Taller integrador — Grafos dirigidos (MVC en C#)

## Integrante único
**JUAN SEBASTIÁN MONTES MARÍN** — autor y desarrollador del proyecto.

## Descripción
CampusNet simula una red social académica dirigida. Cada vértice representa un usuario y cada arista `A → B` significa que A sigue a B. La relación no necesariamente es recíproca.

## Requisitos
- SDK de .NET 8.
- Git para publicar el proyecto en GitHub.

## Ejecución
Desde la carpeta donde está `CampusNet.csproj`:

```bash
dotnet restore
dotnet run
```

Para compilar:

```bash
dotnet build
```

## Arquitectura MVC
- **Models/Vertex.cs:** datos y validación del usuario.
- **Models/Edge.cs:** representación de una relación dirigida.
- **Models/Graph.cs:** lista de adyacencia, grados, BFS, DFS, detección de ciclos y alcanzabilidad.
- **Views/ConsoleView.cs:** impresión de resultados en consola.
- **Controllers/GraphController.cs:** coordina los casos de uso.
- **Program.cs:** invoca únicamente al Controller.

## Funcionalidades
- 12 usuarios iniciales y 21 relaciones dirigidas.
- Prevención de vértices y aristas duplicados.
- Impresión de lista de adyacencia.
- Tres recorridos BFS desde usuarios diferentes.
- DFS completo y detección de ciclos dirigidos.
- Usuarios con grado de entrada cero, más influyentes y más activos.
- Consulta de alcanzabilidad mediante BFS.
- CRUD: agregar, actualizar y eliminar usuarios; agregar y eliminar relaciones.

## Datos de prueba
La red incluye el ciclo dirigido `U01 → U02 → U03 → U01`. Los usuarios U11 y U12 comienzan con grado de entrada cero. U01 y U02 tienen grado de salida igual o superior a 4.

## Evidencias
Agregar capturas reales de la ejecución del programa antes de la entrega.

## Repositorio
Publicar el proyecto en un repositorio público de GitHub. El repositorio debe compilar y ejecutarse sin cambios adicionales.
