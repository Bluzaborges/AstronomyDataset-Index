using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarClassification
{

    public class Node
    {
        public List<int> Id;
        public double Mjd;
        public int Height;
        public Node Left;
        public Node Right;
        public Node(double mjd, int id)
        {
            Mjd = mjd;
            Id = new List<int>();
            Id.Add(id);
            Height = 1;
        }
    }

    public class TreeIndex
    {
        public Node root;
        
        public int height(Node N)
        {
            if (N == null)
                return 0;

            return N.Height;
        }

        public int max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public Node rightRotate(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = max(height(y.Left), height(y.Right)) + 1;
            x.Height = max(height(x.Left), height(x.Right)) + 1;

            return x;
        }

        public Node leftRotate(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = max(height(x.Left), height(x.Right)) + 1;
            y.Height = max(height(y.Left), height(y.Right)) + 1;

            return y;
        }

        public int getBalance(Node N)
        {
            if (N == null)
                return 0;

            return height(N.Left) - height(N.Right);
        }

        public Node insert(Node node, double mjd, int id)
        {

            if (node == null)
                return (new Node(mjd, id));

            if (mjd == node.Mjd)
            {
                node.Id.Add(id);
                return node;
            }

            if (mjd < node.Mjd)
                node.Left = insert(node.Left, mjd, id);
            else if (mjd > node.Mjd)
                node.Right = insert(node.Right, mjd, id);
            else
                return node;


            node.Height = 1 + max(height(node.Left), height(node.Right));

            int balance = getBalance(node);

            if (balance > 1 && mjd < node.Left.Mjd)
                return rightRotate(node);

            if (balance < -1 && mjd > node.Right.Mjd)
                return leftRotate(node);

            if (balance > 1 && mjd > node.Left.Mjd)
            {
                node.Left = leftRotate(node.Left);
                return rightRotate(node);
            }

            if (balance < -1 && mjd < node.Right.Mjd)
            {
                node.Right = rightRotate(node.Right);
                return leftRotate(node);
            }

            return node;
        }

        public void InOrder(Node node)
        {
            if (node != null)
            {
                InOrder(node.Left);
                for (int i = 0; i < node.Id.Count; i++)
                {
                    DateTime date = DateTime.FromOADate((node.Mjd - 2415018.5) + 2400000.5);
                    Console.WriteLine($"{node.Id[i].ToString().PadLeft(6)}{date.ToString().PadLeft(24)}");
                }
                InOrder(node.Right);
            }
        }

        public void SearchByDate(Node node, double mjd, BinaryReader reader, int dataRegisterSize)
        {

            double tolerance = 5;

            if (node != null)
            {
                SearchByDate(node.Left, mjd, reader, dataRegisterSize);
                if (node.Mjd >= mjd - tolerance && node.Mjd <= mjd + tolerance)
                {
                    for (int i = 0; i < node.Id.Count; i++)
                    {
                        reader.BaseStream.Seek(node.Id[i] * dataRegisterSize, SeekOrigin.Begin);
                        var line = reader.ReadString();
                        var values = line.Split(',').Skip(0).ToArray();
                        Console.WriteLine($"{node.Id[i].ToString().PadLeft(5)}{values[13]}{values[14]}{values[16]}{values[1]}{values[2]}");
                        
                    }
                }
                SearchByDate(node.Right, mjd, reader, dataRegisterSize);
            }
        }

        public static void LoadData(TreeIndex treeIndex)
        {
            Console.WriteLine("Criando índice do campo mjd...");

            using var binaryStream = File.Open("files\\star_classification.dat", FileMode.Open);
            using var reader = new BinaryReader(binaryStream);

            var header = reader.ReadString();

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                var line = reader.ReadString();
                var values = line.Split(',').Skip(0).ToArray();

                int id = int.Parse(values[0]);
                double mjd = double.Parse(values[16]);

                treeIndex.root = treeIndex.insert(treeIndex.root, mjd, id);
            }

            Console.WriteLine("Índice de mjd criado com sucesso.");
            Console.Clear();
        }

        public Node MaxMjdValue(Node node)
        {
            Node current = node;

            while (current.Right != null)
            {
                current = current.Right;
            }
            return current;
        }
        public Node MinMjdValue(Node node)
        {
            Node current = node;

            while (current.Left != null)
            {
                current = current.Left;
            }
            return current;
        }

    }
}
