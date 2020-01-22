using System;
using System.IO;
using System.Linq;

namespace CsSeashellAssignmentCleaner
{
	class Program
	{
		private static readonly string[] DisallowedDirectories = { "common" };
		private static readonly string[] DisallowedFiles = { ".project-settings.txt" };
		private static readonly string[] DisallowedExtensions = { ".log" };
		private static string TrashDir;

		private static void Main(string[] args)
		{
			if (args.Length != 1)
				return;

			string dirPath = args[0];

			if (!Directory.Exists(dirPath))
				return;

			TrashDir = Path.Combine(Environment.CurrentDirectory, "trash");
			Directory.CreateDirectory(TrashDir);

			Console.WriteLine("Trashing the following files:");
			CleanAssignmentDir(dirPath);
			Console.WriteLine($"Finished cleaning '{dirPath}'.");
			Console.ReadKey();
		}

		private static void CleanAssignmentDir(string dir)
		{
			if (DisallowedDirectories.Contains(Path.GetFileName(dir)))
			{
				string trashPath = Path.Combine(TrashDir, GetTrashName(Path.GetFileName(dir)));
				Directory.Move(dir, trashPath);
				Console.WriteLine($"'{dir}' -> '{trashPath}'");
				return;
			}

			foreach (string filePath in Directory.GetFiles(dir))
			{
				if (!DisallowedExtensions.Contains(Path.GetExtension(filePath)) && !DisallowedFiles.Contains(Path.GetFileName(filePath)))
					continue;
				//File.Delete(filePath);
				string trashPath = Path.Combine(TrashDir, GetTrashName(Path.GetFileName(filePath)));
				File.Move(filePath, trashPath);
				Console.WriteLine($"'{filePath}' -> '{trashPath}'");
			}

			foreach(string subDir in Directory.GetDirectories(dir))
				CleanAssignmentDir(subDir);
		}

		private static string GetTrashName(string fileName)
		{
			if (!File.Exists(Path.Combine(TrashDir, fileName)))
				return fileName;
			if (int.TryParse(Path.GetExtension(fileName), out int extNum))
				return GetTrashName(Path.GetFileNameWithoutExtension(fileName) + "." + (extNum + 1));
			return GetTrashName(fileName + "." + 0);
		}
	}
}
