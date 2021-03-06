﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

using Accord;
using Accord.Imaging;
using Accord.MachineLearning;
using Accord.Math;
using Accord.Statistics.Kernels;
using AForge;

using Newtonsoft.Json;

namespace QueryImage
{
	class MainClass
	{
		internal static readonly DirectoryInfo INDEX_DIR = new DirectoryInfo ("index");

		public static void Main (string[] args)
		{
			if (args.Length == 2) {
				if (args [0] == "index") {
					Index ();
				}

				if (args [0] == "file") {
					loadImageFromDisk (args [1]);
				}

				if (args [0] == "bow") {
					createBoW();
				}
			}

			if (args.Length == 1) {
				praseInputToDouble (args [0]);
			}
			//ImageFeatures ();
			//CreateVectorAndAppendToFile("/home/konrad/Downloads/div-2014/devset/img/acropolis_athens/132872354.jpg");
			//CreateVectorAndAppendToFile("/home/konrad/Downloads/div-2014/devset/img/acropolis_athens/512080179.jpg");
			//CreateVectorAndAppendToFile("/home/konrad/Downloads/div-2014/devset/img/acropolis_athens/2064729793.jpg");
			/*get10bestResultsforVector(new double[6] {
				53,554,8,15,26,2
			});*/
		}

		public static void praseInputToDouble(string doubleString) {
			string[] vecStr = doubleString.Split(',');
			double[] vec = new double[vecStr.Length];

			for (int i = 0; i < vecStr.Length; i++) {
				vec [i] = Double.Parse(vecStr[i]);
			}

			get10bestResultsforVector (vec);
		}

		public static void loadImageFromDisk(string input_file) {
			BagOfVisualWords bow = BagOfVisualWords.Load ("bagOfWords");
			string file = Directory.GetCurrentDirectory () + "/" + input_file;
			double[] featureVector = bow.GetFeatureVector((Bitmap)Bitmap.FromFile(file));
			get10bestResultsforVector (featureVector);
		}


		public static void Index() {
			string[] files = Directory.GetFiles (Directory.GetCurrentDirectory() +"/img/", "*.jpg", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++) {
				CreateVectorAndAppendToFile (files [i]);
			}
		}

		public static void createBoW() {
			string[] files = Directory.GetFiles ("testset");
			Dictionary<string, Bitmap> testImages = new Dictionary<string, Bitmap>();

			for (int i = 0; i < files.Length; i++) {
				string name = Path.GetFileNameWithoutExtension (files [i]);
				testImages.Add(name, (Bitmap)Bitmap.FromFile(files[i]));
			}

			int numberOfWords = 6; // number of cluster centers: typically >>100

			// Create a Binary-Split clustering algorithm
			BinarySplit binarySplit = new BinarySplit(numberOfWords);

			// Create bag-of-words (BoW) with the given algorithm
			BagOfVisualWords surfBow = new BagOfVisualWords(binarySplit);

			// Compute the BoW codebook using training images only
			Bitmap[] bmps = new Bitmap[testImages.Count];
			testImages.Values.CopyTo(bmps, 0);
			surfBow.Compute(bmps);		

			surfBow.Save ("bagOfWords");
		}

		private static void CreateVectorAndAppendToFile(string input_file) {
			BagOfVisualWords bow = BagOfVisualWords.Load ("bagOfWords");

			double[] featureVector = bow.GetFeatureVector((Bitmap)Bitmap.FromFile(input_file));
			string dir = Path.GetDirectoryName (input_file);
			string[] split = dir.Split ('/');

			using (StreamWriter sw = File.AppendText("bow.txt")) 
			{
				sw.WriteLine( split[split.Length - 1]+","+Path.GetFileNameWithoutExtension(input_file) + "," + String.Join(",", featureVector));
			}	
			Console.WriteLine ("done");

		}

		private static void get10bestResultsforVector(double[] compareVector) {
			List<Tuple<double, string>> results= new List<Tuple<double, string>> ();

			using (StreamReader sr = new StreamReader("bow.txt")) 
			{
				while (sr.Peek() >= 0) 
				{
					string line = sr.ReadLine ();
					string[] split = line.Split (',');
					string location = split [0];
					string id = split [1];
					double[] vector = new double[6];
					for (int i = 0; i < 6; i++) {
						vector [i] = Double.Parse(split [i + 2]);
					}
					double dist = Distance.Cosine (compareVector, vector);
					if (dist < 0.2) {
						string fileName = location + "/" + id + ".jpg";
						results.Add (Tuple.Create(dist, fileName));
					}
				}
			}
			results.Sort ();

			string[] output = new string[20];

			int max = 20;

			if(results.Count < 20) {
				max = results.Count;
			}

			for (int i = 0; i < max; i++) {
				output[i] = results[i].Item2;
			}
			Console.Out.WriteLine (JsonConvert.SerializeObject (output));
		}
	}
}
