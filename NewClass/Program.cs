using System;
using System.Collections.Generic;
using System.IO;

namespace CMakeConfigure
{
	class MainClass
	{
		static string SRC_TEMPLATE 		= "src_template.cpp";
		static string HEADER_TEMPLATE	= "header_template.h";
		static string HEADER_ONLY_TEMPLATE = "header_only_template.h";
		static string KEYMARKER 		= "#";
		static string FILENAME_KEY 		= "#FILENAME";
		static string PROJECTNAME_KEY	= "#PROJECT_NAME";
		static string AUTHOR_KEY 		= "#AUTHOR";
		static string DATE_KEY 			= "#DATE";

		static string CLASSGUARD_KEY 	= "#CLASS_GUARD";
		static string CLASSNAME_KEY 	= "#CLASS_NAME";
		static string NAMESPACE_KEY 	= "#NAMESPACE";
		static string HEADERFILE_KEY	= "#HEADERFILENAME";

		public static string FindFolder(string rootPath, string searchPath)
		{
			var searchDirInfo = new DirectoryInfo(rootPath);

			foreach (var directoryInfo in searchDirInfo.EnumerateDirectories())
			{
				//Console.WriteLine(directoryInfo.FullName);
				if (directoryInfo.FullName.Contains(searchPath))
				{
					return directoryInfo.FullName;
				}
			}

			foreach (var directoryInfo in searchDirInfo.EnumerateDirectories())
			{
				return FindFolder(directoryInfo.FullName, searchPath);
			}
			return "";
		}

		public static List<string> ParseFile(string templatesPath, string templateFileName, string[] args)
		{
			string filename = args[2];
			string projectname = args[3];
			string authorname = args[4];
			string classname = args[5];
			string namespacename = args[6];

			string[] templateHeaderLines = File.ReadAllLines(Path.Combine(templatesPath, templateFileName));
			List<string> newFileLines = new List<string>();
			string newLine = "";
			foreach(var line in templateHeaderLines)
			{
				if (line.Contains(KEYMARKER))
				{
					newLine = line;
					if (line.Contains(FILENAME_KEY))
					{
						newLine = newLine.Replace(FILENAME_KEY, filename);
					}

					if (line.Contains(PROJECTNAME_KEY))
					{
						newLine = newLine.Replace(PROJECTNAME_KEY, projectname);
					}

					if (line.Contains(AUTHOR_KEY))
					{
						newLine = newLine.Replace(AUTHOR_KEY, authorname);
					}

					if (line.Contains(DATE_KEY))
					{
						DateTime localDate = DateTime.Now;
						newLine = newLine.Replace(DATE_KEY, localDate.ToString());
					}

					if (line.Contains(CLASSGUARD_KEY))
					{
						newLine = newLine.Replace(CLASSGUARD_KEY, classname.ToUpper()+"_H");
					}

					if (line.Contains(CLASSNAME_KEY))
					{
						newLine = newLine.Replace(CLASSNAME_KEY, classname);
					}

					if (line.Contains(NAMESPACE_KEY))
					{
						newLine = newLine.Replace(NAMESPACE_KEY, namespacename);
					}

					if (line.Contains(HEADERFILE_KEY))
					{
						newLine = newLine.Replace(HEADERFILE_KEY, filename+".h");
					}

					newFileLines.Add(newLine);
					//Console.WriteLine(newLine);
				}
				else
				{
					newFileLines.Add(line);
					//Console.WriteLine(line);
				}
			}
			return newFileLines;
		}

		public static void CreateFile(string rootPath, string folder, string filename, List<string> parsedLines)
		{
			string outputPath = FindFolder(rootPath, folder);
			string outputFileNamePath = "";
			// If the folder exists, we get the path
			if (0 < outputPath.Length)
			{
				outputFileNamePath = Path.Combine(outputPath, filename);
			}
			// If the folder doesn't exist, we create it
			else
			{
				string folderPath = Path.Combine(rootPath, folder);
				Console.WriteLine("Folder where we create the new folder: " + folderPath);
				DirectoryInfo dirInfo = Directory.CreateDirectory(folderPath);
				outputFileNamePath = Path.Combine(dirInfo.FullName, filename);
			}
				
			File.WriteAllLines(outputFileNamePath, parsedLines.ToArray());
			Console.WriteLine(filename+" generated succesfully on: "+outputFileNamePath);
		}

		public static void Main(string[] args)
		{
			if (args.Length < 8)
			{
				Console.WriteLine("CORRECT USE: mono NewClass.exe " +
					"<templates_path> <root_output_path> " +
					"<filename_no_extension> <project_name> <author_name>" +
				    "<class_name> <namespace> <project_local_path_folder>" +
				    "<optional_header_only>");
				return;
			}

			string templatesPath = args[0];
			string rootPath = args[1];
			string filename = args[2];
			string folder = args[7];

			Console.WriteLine("Num args = "+args.Length);

			if(args.Length == 9)
			{
				List<string> parsedHeaderLines = ParseFile(templatesPath, HEADER_ONLY_TEMPLATE, args);
				string headersRootPath = Path.Combine(rootPath, "include");
				string filenameWithExtension = filename + ".h";
				CreateFile(headersRootPath, folder, filenameWithExtension, parsedHeaderLines);
			}
			else
			{
				List<string> parsedHeaderLines = ParseFile(templatesPath, HEADER_TEMPLATE, args);
				/*
				foreach (var line in parsedHeaderLines)
				{
					Console.WriteLine(line);
				}
				*/
				string headersRootPath = Path.Combine(rootPath, "include");
				string filenameWithExtension = filename + ".h";
				CreateFile(headersRootPath, folder, filenameWithExtension, parsedHeaderLines);

				List<string> parsedSourceLines = ParseFile(templatesPath, SRC_TEMPLATE, args);
				string srcRootPath = Path.Combine(rootPath, "src");
				filenameWithExtension = filename + ".cpp";
				CreateFile(srcRootPath, folder, filenameWithExtension, parsedSourceLines);
			}

		}
	}
}