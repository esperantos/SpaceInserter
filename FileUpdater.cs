using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace SpaceInserter
{
	public class FileUpdater
	{
		private readonly char[] _digits = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};

		public void UpdateFile(string source, string toDirectory)
		{
			XmlDocument xmlDocument = ReadXml(source);
			int updated = Process(xmlDocument);
			if (updated != 2)
				Console.Write("!!!  ");
			Console.WriteLine(string.Format("Обработан файл {0}. Переписано {1} Номеров.", source, updated));
// ReSharper disable AssignNullToNotNullAttribute
			WriteXml(Path.Combine(toDirectory, Path.GetFileName(source)), xmlDocument);
// ReSharper restore AssignNullToNotNullAttribute
		}

		private int Process(XmlNode xmlNode)
		{
			XmlAttribute attr = FindAttribute(xmlNode, "СерНомДок");
			return UpdateValue(attr) + xmlNode.ChildNodes.Cast<XmlNode>().Sum(child => Process(child));
		}

		private int UpdateValue(XmlAttribute attr1)
		{
			if (attr1 != null && attr1.Value.Length == 10 && attr1.Value.All(c => _digits.Contains(c)))
			{
				attr1.Value = attr1.Value.Insert(4, " ");
				attr1.Value = attr1.Value.Insert(2, " ");
				return 1;
			}
			return 0;
		}

		private static XmlAttribute FindAttribute(XmlNode xmlNode, string attrName)
		{
			XmlAttribute firstOrDefault = null;
			if (xmlNode != null && xmlNode.Attributes != null)
			{
				firstOrDefault = xmlNode.Attributes.Cast<XmlAttribute>().FirstOrDefault(a => a.Name == attrName);
			}
			return firstOrDefault;
		}


		private static XmlDocument ReadXml(string path)
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				fileStream.Seek(0, SeekOrigin.Begin);
				xmlDocument.Load(fileStream);
			}
			return xmlDocument;
		}

		private static void WriteXml(string path, XmlDocument xmlDocument)
		{
			using (var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
			{
				XmlWriterSettings settings = new XmlWriterSettings
					{Indent = true, OmitXmlDeclaration = false, NewLineOnAttributes = false};
				using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, settings))
				{
					xmlDocument.WriteTo(xmlWriter);
				}
			}
		}
	}
}