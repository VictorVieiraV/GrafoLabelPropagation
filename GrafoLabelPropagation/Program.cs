using GrafoLabelPropagation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Digite o número de vértices do grafo:");
        int numVertices = int.Parse(Console.ReadLine());

        Grafo grafo = new Grafo(numVertices);
        grafo.CriarGrafo();

        grafo.AddAresta(0, 1);
        grafo.AddAresta(1, 2);
        grafo.AddAresta(2, 0);
        grafo.RemoveAresta(2, 0);
        grafo.PonderarVertice(0, "PonderarVertice");
        grafo.PonderarAresta(0, 1, "PonderarAresta");

        bool eAdjacente = grafo.VerticeEAdjacente(0, 1);
        Console.WriteLine("Vertices 0 e 1 são adjacentes? " + eAdjacente);

        bool eArestaAdjacente = grafo.ArestaEAdjacente(0, 1, 1, 2);
        Console.WriteLine("Aresta 0,1 é adjacente a 1,2? " + eArestaAdjacente);

        bool eIncidente = grafo.ArestaEIncidenteAVertice(0, 1, 2);
        Console.WriteLine("Aresta 1,2 é incidente ao vertice 0? " + eIncidente);

        bool eVazio = grafo.GrafoEstaVazio();
        Console.WriteLine("O grafo é vazio? " + eVazio);

        bool eComplete = grafo.GrafoEstaCompleto();
        Console.WriteLine("O grafo é completo? " + eComplete);

        bool existeAresta = grafo.ExisteAresta();
        Console.WriteLine("No grafo existem arestas? " + existeAresta);

        int numTotalVertices = grafo.GetNumVertices();
        Console.WriteLine("Número total vertices: " + numTotalVertices);

        int numTotalArestas = grafo.GetNumArestas();
        Console.WriteLine("Número total arestas: " + numTotalArestas);
    }
}