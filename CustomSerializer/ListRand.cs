using System.Text;

namespace Serializer
{
    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public async void Serialize(FileStream s)
        {
             
            var serialiazedData = "";
            var currentNode = Head;
            while (currentNode != null)
            {
                //Запись информации о Data, собственном hash коде, hash коде Rand элемента списка
                //Литералы \v для разделения информации внутри одного нода
                //Литералы \n для разделения разных нодов
                //Проверка на null для случаев, если Rand не задан
                serialiazedData += currentNode.Data + @"\v"
                    + currentNode.GetHashCode() + @"\v"
                    + (currentNode.Rand == null ? null : currentNode.Rand.GetHashCode()) + @"\n";
                currentNode = currentNode.Next;
            }

            //Сериализация получившейся строки
            var buffer = Encoding.Default.GetBytes(serialiazedData);
            await s.WriteAsync(buffer);
        }

        public async void Deserialize(FileStream s)
        {

            //Десериализация строки
            var buffer = new byte[s.Length];
            await s.ReadAsync(buffer);
            var textFromFile = Encoding.Default.GetString(buffer);

            //Разделение строки на подстроки с информацией об отдельных нодах
            var deserialiazedData = textFromFile.Split(@"\n", StringSplitOptions.RemoveEmptyEntries);
            Count = deserialiazedData.Length;

            //Словари используются для восстановления Rand ссылок
            var hashAndNodesDictionary = new Dictionary<string, ListNode>();
            var hashAndRandomHashDictionary = new Dictionary<string, string>();
            var prevNode = new ListNode();

            for (var i = 0; i < Count; i++)
            {
                //Разделение подстроки на Data, "старый" собственный hash код, "старый" Rand hash код
                var fields = deserialiazedData[i].Split(@"\v");

                var node = new ListNode()
                {
                    Data = fields[0]
                };

                if (i == 0)
                {
                    Head = node;
                }
                else
                {
                    //Восстановление prev и next ссылок за счет порядка сериализации и десериализации от head к tail
                    node.Prev = prevNode;
                    prevNode.Next = node;
                }

                if (i == Count - 1)
                {
                    Tail = node;
                }

                prevNode = node;
                hashAndNodesDictionary.Add(fields[1], node);
                hashAndRandomHashDictionary.Add(fields[1], fields[2]);
            }

            //Восстановление Rand ссылок через сопостовление "старых" hash кодов
            foreach (var i in hashAndNodesDictionary)
            {
                var randHash = hashAndRandomHashDictionary[i.Key];
                if (!string.IsNullOrEmpty(randHash))
                {
                    i.Value.Rand = hashAndNodesDictionary[randHash];
                }
            }
        }
    }
}