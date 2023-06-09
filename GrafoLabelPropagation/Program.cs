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

        bool isAdjacent = grafo.VerticeEAdjacente(0, 1);
        Console.WriteLine("0 e 1 são adjacentes? " + isAdjacent);

        bool isComplete = grafo.GrafoEstaCompleto();
        Console.WriteLine("O grafo é completo? " + isComplete);
    }
}