using System;
using Catalyst;
using Mosaik.Core;

namespace Pamaxie.Catalyst
{
    class Program
    {
        static void Main(string[] args)
        {
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));
            Pipeline.For(Language.English);
            var nlp = Pipeline.For(Language.English);
            var doc = new Document("The person", Language.English);
            var process = nlp.ProcessSingle(doc);
            Console.WriteLine(doc.ToJson());
        }
    }
}
