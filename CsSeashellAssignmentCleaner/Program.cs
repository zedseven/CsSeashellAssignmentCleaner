using System;
using System.IO;
using System.Linq;

namespace CsSeashellAssignmentCleaner
{
	internal class Program
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
				Console.WriteLine($"'{dir}' -> '{trashPath}'");
				Directory.Move(dir, trashPath);
				return;
			}

			foreach (string filePath in Directory.GetFiles(dir))
			{
				if (!DisallowedExtensions.Contains(Path.GetExtension(filePath)) && !DisallowedFiles.Contains(Path.GetFileName(filePath)))
					continue;
				string trashPath = Path.Combine(TrashDir, GetTrashName(Path.GetFileName(filePath)));
				Console.WriteLine($"'{filePath}' -> '{trashPath}'");
				File.Move(filePath, trashPath);
			}

			foreach(string subDir in Directory.GetDirectories(dir))
				CleanAssignmentDir(subDir);
		}

		private static string GetTrashName(string fileName)
		{
			if (!Directory.Exists(Path.Combine(TrashDir, fileName)) && !File.Exists(Path.Combine(TrashDir, fileName)))
				return fileName;
			string pathExt = Path.GetExtension(fileName);
			if (pathExt.Length > 0 && int.TryParse(pathExt.Substring(1), out int extNum))
				return GetTrashName(Path.GetFileNameWithoutExtension(fileName) + "." + (extNum + 1));
			return GetTrashName(fileName + "." + 0);
		}
	}
}
