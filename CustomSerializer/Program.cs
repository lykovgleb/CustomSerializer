namespace Serializer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var list = new ListRand();
            var nod1 = new ListNode() { Data = "A" };
            var nod2 = new ListNode() { Data = "B" };
            var nod3 = new ListNode() { Data = "C" };
            var nod4 = new ListNode() { Data = "D" };
            var nod5 = new ListNode() { Data = "E" };

            nod1.Next = nod2;
            nod2.Next = nod3;
            nod2.Prev = nod1;
            nod2.Rand = nod2;
            nod3.Next = nod4;
            nod3.Prev = nod2;
            nod3.Rand = nod5;
            nod4.Next = nod5;
            nod4.Prev = nod3;
            nod4.Rand = nod1;
            nod5.Prev = nod4;

            list.Head = nod1;
            list.Tail = nod5;
            list.Count = 5;

            var currentNode = list.Head;
            for (var i = 0; i < list.Count; i++)
            {
                Console.WriteLine(currentNode.Data + " " + (currentNode.Rand == null ? null : currentNode.Rand.Data));
                if (i != list.Count - 1)
                {
                    currentNode = currentNode.Next;
                }
            }

            var newList = new ListRand();

            using (FileStream s = new FileStream("serializeFile.txt", FileMode.OpenOrCreate))
            {
                list.Serialize(s);
            }

            using (FileStream a = new FileStream("serializeFile.txt", FileMode.OpenOrCreate))
            {
                newList.Deserialize(a);
            }

            var newNode = newList.Head;
            for (var i = 0; i < newList.Count; i++)
            {
                Console.WriteLine(newNode.Data + " " + (newNode.Rand == null ? null : newNode.Rand.Data));
                if (i != newList.Count - 1)
                {
                    newNode = newNode.Next;
                }
            }

            Console.WriteLine(list.Head.GetHashCode());
            Console.WriteLine(newList.Head.GetHashCode());
        }
    }
}