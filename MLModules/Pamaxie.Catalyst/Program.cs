using System;
using Catalyst;
using Mosaik.Core;

namespace Pamaxie.Catalyst
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //TODO: This will be responsible for NLP this will only be interesting later on.
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));
            Pipeline.For(Language.English);
            //Pipeline nlp = Pipeline.For(Language.English);
            Document doc = new Document("The person", Language.English);
            //IDocument process = nlp.ProcessSingle(doc);
            Console.WriteLine(doc.ToJson());
        }
    }
}
