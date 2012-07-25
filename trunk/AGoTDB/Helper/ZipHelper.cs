using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace AGoTDB.Forms
{
	public static class ZipHelper
	{
		public static List<string> UnZipFile(string zipFilePath)
		{
			try
			{
				if (!File.Exists(zipFilePath))
					return null;

				var result = new List<string>();
				string baseDirectoryPath = Path.GetDirectoryName(zipFilePath);
				using (var zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
				{
					ZipEntry entry;
					while ((entry = zipStream.GetNextEntry()) != null)
					{
						if (entry.IsFile)
						{
							if (entry.Name.Length > 0)
							{
								string filePath = string.Format(@"{0}\{1}", baseDirectoryPath, entry.Name);
								using (var streamWriter = File.Create(filePath))
								{
									var data = new byte[2048];
									while (true)
									{
										int size = zipStream.Read(data, 0, data.Length);
										if (size > 0)
											streamWriter.Write(data, 0, size);
										else
											break;
									}
									streamWriter.Close();
								}
								result.Add(filePath);
							}
						}
						else if (entry.IsDirectory)
						{
							string directoryPath = string.Format(@"{0}\{1}", baseDirectoryPath, entry.Name);
							if (!Directory.Exists(directoryPath))
							{
								Directory.CreateDirectory(directoryPath);
							}
						}
					}
					zipStream.Close();
				}
				return result;
			}
			catch (Exception ex)
			{
				return null;
			}
		}  
	}
}