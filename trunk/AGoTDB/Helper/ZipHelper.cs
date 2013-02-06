using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;

namespace AGoTDB.Helper
{
	public static class ZipHelper
	{
		public static List<string> UnZipFile(string zipFilePath, string destinationFolderPath = null, string filePattern = null)
		{
			try
			{
				if (!File.Exists(zipFilePath))
					return null;

				var regex = filePattern != null
					? FindFilesPatternToRegex.Convert(filePattern)
					: null;

				var result = new List<string>();
				string baseDirectoryPath = destinationFolderPath ?? Path.GetDirectoryName(zipFilePath);
				using (var zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
				{
					ZipEntry entry;
					while ((entry = zipStream.GetNextEntry()) != null)
					{
						if (entry.IsFile)
						{
							if (entry.Name.Length > 0)
							{
								if (regex != null && !regex.IsMatch(entry.Name))
									continue;

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

	static class FindFilesPatternToRegex
	{
		private static readonly Regex _hasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
		private static readonly Regex _ilegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
		private static readonly Regex _catchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
		private const string NonDotCharacters = @"[^.]*";

		public static Regex Convert(string pattern)
		{
			if (pattern == null)
				throw new ArgumentNullException();

			pattern = pattern.Trim();

			if (pattern.Length == 0)
				throw new ArgumentException("Pattern is empty.");
			if (_ilegalCharactersRegex.IsMatch(pattern))
				throw new ArgumentException("Patterns contains ilegal characters.");

			bool hasExtension = _catchExtentionRegex.IsMatch(pattern);
			bool matchExact = false;
			if (_hasQuestionMarkRegEx.IsMatch(pattern))
			{
				matchExact = true;
			}
			else if (hasExtension)
			{
				matchExact = _catchExtentionRegex.Match(pattern).Groups[1].Length != 3;
			}
			string regexString = Regex.Escape(pattern);
			regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
			regexString = Regex.Replace(regexString, @"\\\?", ".");
			if (!matchExact && hasExtension)
			{
				regexString += NonDotCharacters;
			}
			regexString += "$";
			return new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}
	}
}