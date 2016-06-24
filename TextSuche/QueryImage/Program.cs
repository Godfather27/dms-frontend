using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

using System.Linq;

using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;

using Newtonsoft.Json;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;

using FSDirectory = Lucene.Net.Store.FSDirectory;
using Version = Lucene.Net.Util.Version;

using Accord;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.Math;
using Accord.Statistics.Kernels;
using AForge;
using Accord.Math.Distances;

namespace QueryImage
{
	class MainClass
	{
		internal static readonly DirectoryInfo INDEX_DIR = new DirectoryInfo ("index");

		public static void Main (string[] args)
		{
			if (args.Length == 0) {
				Index ();
			}

			if (args.Length == 1) {
				Query (args [0]);
			}
		}

		private static void Index ()
		{
			string[] files = Directory.GetFiles ("X:\\MMT\\4. Semester\\Allgemein\\Digital Media Systems\\div-2014\\devset\\xml");
			try {
				using (var writer = new IndexWriter (FSDirectory.Open (INDEX_DIR), new StandardAnalyzer (Version.LUCENE_30), true, IndexWriter.MaxFieldLength.LIMITED)) {
					for (int i = 0; i < files.Length; i++) {
						string location = Path.GetFileNameWithoutExtension (files [i]);
						Document doc;

						using (XmlReader xmlReader = XmlReader.Create (files [i])) {
							while (xmlReader.Read ()) {
								if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "photo")) {
									if (xmlReader.HasAttributes) {
										string description = xmlReader.GetAttribute ("description");
										string id = xmlReader.GetAttribute ("id");
                                        string url = xmlReader.GetAttribute("url_b");
										doc = new Document ();
										doc.Add (new Field ("name", id, Field.Store.YES, Field.Index.NO));
										doc.Add (new Field ("location", location, Field.Store.YES, Field.Index.NO));
										doc.Add (new Field ("content", description, Field.Store.YES, Field.Index.ANALYZED));
                                        doc.Add(new Field("url_b", url, Field.Store.YES, Field.Index.ANALYZED));
                                        writer.AddDocument (doc);
									}
								}
							}
						}
					}
					Console.Out.WriteLine ("Optimizing...");
					writer.Optimize ();
					writer.Commit ();	
					writer.Dispose ();
				}
			} catch (IOException e) {
				Console.Out.WriteLine (" caught a " + e.GetType () + "\n with message: " + e.Message);
			}
		}


		private static void Query (string qry)
		{
			qry = qry.ToLower ();
			try {
				string[] results = new string[10];


				IndexReader reader = IndexReader.Open (FSDirectory.Open (INDEX_DIR), true);
				//Console.Out.WriteLine ("Number of indexed docs: " + reader.NumDocs ());

				IndexSearcher searcher = new IndexSearcher (FSDirectory.Open (INDEX_DIR));
				Term searchTerm = new Term ("content", qry);
				TermQuery query = new TermQuery (searchTerm);

				TopScoreDocCollector topDocColl = TopScoreDocCollector.Create (10, true);
				searcher.Search (query, topDocColl);
				TopDocs topDocs = topDocColl.TopDocs ();
				//Console.Out.WriteLine ("Number of hits: " + topDocs.TotalHits);
				ScoreDoc[] docarray = topDocs.ScoreDocs;

				for (int i = 0; i < docarray.Length; i++) {
                    results[i] = searcher.Doc(docarray[i].Doc).GetField("url_b").StringValue;
                    //results [i] = searcher.Doc (docarray [i].Doc).GetField ("location").StringValue +  "/" + searcher.Doc (docarray [i].Doc).GetField ("name").StringValue + ".jpg";
                }

				Console.Out.WriteLine (JsonConvert.SerializeObject (results));
			} catch (IOException e) {
				Console.Out.WriteLine (" caught a " + e.GetType () + "\n with message: " + e.Message);
			}

		}

	}
}
