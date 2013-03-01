using System;
using System.IO;

namespace SpaceInserter
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine(
					"неверное использование! Надо так:\nSpaceInserter <путь к папке с исходными файлами> <папка куда положить модифицированный вариант>\n или\n\nSpaceInserter <путь к файлу> <папка куда положить модифицированный вариант>");
				return 500;
			}

			string sourcePath = args[0];
			string destinationPath = args[1];

			if (!Directory.Exists(destinationPath))
				Directory.CreateDirectory(destinationPath);
			var fileUpdater = new FileUpdater();
			if (File.Exists(sourcePath))
			{
				fileUpdater.UpdateFile(sourcePath, destinationPath);
				return 0;
			}

			if (Directory.Exists(sourcePath))
			{
				string[] files = Directory.GetFiles(sourcePath, "*.xml");
				if (files.Length == 0)
					Console.WriteLine(string.Format("В папке \"{0}\" не найдено доверенностей", sourcePath));
				foreach (string file in files)
				{
					try
					{
						fileUpdater.UpdateFile(file, destinationPath);
					}
					catch (Exception e)
					{
						Console.WriteLine(string.Format("Ошибка при обработки файла {0}.\n {1}\n", file, e));
					}
				}
				return 0;
			}
			Console.WriteLine(string.Format("Папка или файл {0} не найден", sourcePath));
			return 404;
		}
	}
}