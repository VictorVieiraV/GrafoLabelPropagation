namespace GrafoLabelPropagation
{
    public class Grafo
    {
        private int[,] MatrizAdjacencia;
        private List<List<int>> ListaAdjacencia;
        private int numVertices;

        public Grafo(int numVertices)
        {
            this.numVertices = numVertices;

            // Inicialização da Matriz de Adjacência
            MatrizAdjacencia = new int[numVertices, numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = 0; j < numVertices; j++)
                {
                    MatrizAdjacencia[i, j] = 0;
                }
            }

            // Inicialização da Lista de Adjacência
            ListaAdjacencia = new List<List<int>>();
            for (int i = 0; i < numVertices; i++)
            {
                ListaAdjacencia.Add(new List<int>());
            }
        }

        public void CriarGrafo()
        {
            // Criação de um grafo com X vértices
            // Implementação básica para criação do grafo
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = i + 1; j < numVertices; j++)
                {
                    Console.WriteLine("Existe uma aresta entre o vértice {0} e o vértice {1}? (S/N)", i, j);
                    string input = Console.ReadLine();

                    if (input.ToUpper() == "S")
                    {
                        AddAresta(i, j);
                    }
                }
            }
        }

        public void AddAresta(int origem, int destino)
        {
            // Criação de uma aresta entre dois vértices

            // Adiciona a aresta na Matriz de Adjacência
            MatrizAdjacencia[origem, destino] = 1;
            MatrizAdjacencia[destino, origem] = 1;

            // Adiciona a aresta na Lista de Adjacência
            ListaAdjacencia[origem].Add(destino);
            ListaAdjacencia[destino].Add(origem);
        }

        public void RemoveAresta(int origem, int destino)
        {
            // Remoção de uma aresta entre dois vértices

            // Remove a aresta da Matriz de Adjacência
            MatrizAdjacencia[origem, destino] = 0;
            MatrizAdjacencia[destino, origem] = 0;

            // Remove a aresta da Lista de Adjacência
            ListaAdjacencia[origem].Remove(destino);
            ListaAdjacencia[destino].Remove(origem);
        }

        public void PonderarVertice(int vertice, string valor)
        {
            // Ponderação de um vértice
            string[] verticeValor = new string[numVertices];
            verticeValor[vertice] = valor;
        }

        public void PonderarAresta(int origem, int destino, string valor)
        {
            // Ponderação de uma aresta
            string[,] edgeLabels = new string[numVertices, numVertices];

            edgeLabels[origem, destino] = valor;
            edgeLabels[destino, origem] = valor;
        }

        public bool VerticeEAdjacente(int vertice1, int vertice2)
        {
            // Checagem de adjacência entre vértices
            // Verifica a adjacência na Matriz de Adjacência
            if (MatrizAdjacencia[vertice1, vertice2] == 1)
                return true;
            else
                return false;
        }

        public bool ArestaEAdjacente(int origem1, int destino1, int origem2, int destino2)
        {
            // Checagem de adjacência entre arestas
            // Verifica a adjacência na Matriz de Adjacência
            if (MatrizAdjacencia[origem1, destino1] == 1 && MatrizAdjacencia[origem2, destino2] == 1)
                return true;
            else
                return false;
        }

        public bool ArestaEIncidenteAVertice(int vertice, int origem, int destino)
        {
            // Checagem de incidência entre uma aresta e um vértice
            // Verifica a incidência na Matriz de Adjacência
            if (MatrizAdjacencia[vertice, origem] == 1 || MatrizAdjacencia[vertice, destino] == 1)
                return true;
            else
                return false;
        }

        public bool ExisteAresta()
        {
            // Checagem da existência de arestas
            // Verifica se há alguma aresta na Matriz de Adjacência
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = 0; j < numVertices; j++)
                {
                    if (MatrizAdjacencia[i, j] == 1)
                        return true;
                }
            }
            return false;
        }

        public int GetNumVertices()
        {
            // Checagem da quantidade de vértices
            return numVertices;
        }

        public int GetNumArestas()
        {
            // Checagem da quantidade de arestas
            int count = 0;
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = i + 1; j < numVertices; j++)
                {
                    if (MatrizAdjacencia[i, j] == 1)
                        count++;
                }
            }
            return count;
        }

        public bool GrafoEstaVazio()
        {
            // Checagem de grafo vazio
            // Verifica se não há vértices no grafo
            if (numVertices == 0)
                return true;
            else
                return false;
        }

        public bool GrafoEstaCompleto()
        {
            // Checagem de grafo completo
            // Verifica se todas as combinações de vértices estão conectadas por arestas
            for (int i = 0; i < numVertices; i++)
            {
                for (int j = i + 1; j < numVertices; j++)
                {
                    if (MatrizAdjacencia[i, j] != 1)
                        return false;
                }
            }
            return true;
        }

        public void SaveGrafoToCSV()
        {
            // Salvar o grafo em um arquivo CSV

            string filePath = "C:\\Users\\PC\\Desktop\\Grafos\\grafin2.csv";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Escrever o cabeçalho do arquivo CSV
                writer.WriteLine("Source,Target");

                // Escrever as arestas do grafo no formato CSV
                for (int i = 0; i < numVertices; i++)
                {
                    for (int j = i + 1; j < numVertices; j++)
                    {
                        if (MatrizAdjacencia[i, j] == 1)
                        {
                            writer.WriteLine($"{i},{j}");
                        }
                    }
                }
            }
        }

        public void LoadGrafoFromCSV()
        {
            // Carregar um grafo a partir de um arquivo CSV

            string filePath = "C:\\Users\\PC\\Desktop\\Grafos\\grafin2.csv";
            // Limpar o grafo existente
            LimparGrafo();

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Ignorar a linha do cabeçalho
                reader.ReadLine();

                // Ler as arestas do arquivo CSV e adicioná-las ao grafo
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] vertices = line.Split(',');

                    if (vertices.Length >= 2)
                    {
                        int source = int.Parse(vertices[0]);
                        int target = int.Parse(vertices[1]);
                        AddAresta(source, target);
                    }
                }
            }
        }

        public void LimparGrafo()
        {
            // Limpar o grafo, removendo todas as arestas e vértices

            MatrizAdjacencia = new int[numVertices, numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                ListaAdjacencia[i].Clear();
            }
        }
    }
}
