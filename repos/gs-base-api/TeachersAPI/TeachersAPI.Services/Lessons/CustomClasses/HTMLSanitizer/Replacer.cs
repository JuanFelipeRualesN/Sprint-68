using System;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using TeachersAPI.Utils;

namespace TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses.HTMLSanitizer
{
	/// <summary>
	/// To replace some elements of the HTML.
	/// </summary>
	public static class Replacer
	{
		/// <summary>
		/// Replace the HTTP sources with the path.
		/// </summary>
		/// <param name="htmlToReplace">HTML full string.</param>
		/// <param name="pathToReplace">The path to replace.</param>
		/// <returns>Returns the HTML with the replaced sources.</returns>
		public static string ReplaceHTTPSource(string htmlToReplace, string pathToReplace)
		{
			return Regex.Replace(htmlToReplace, @pathToReplace, "");
		}

		/// <summary>
		/// Replaces the Wiris with the image source path.
		/// </summary>
		/// <param name="htmlToReplace">The HTML as a string.</param>
		/// <param name="imgSourcePath">The image source path to replace.</param>
		/// <param name="wirisServiceURL">The wiris service URL that converts the SVG to image.</param>
		/// <returns>Returns the HTML with the replaced sources.</returns>
		public static string ReplaceWiris(string htmlToReplace, string imgSourcePath, string wirisServiceURL)
		{
			// First read the HTML string as a HTML Doc to edit
			var doc = new HtmlDocument();
			doc.LoadHtml(htmlToReplace);

			// Find math formulas
			var mathMLList = doc.DocumentNode.SelectNodes("//math");
			// If nothing was found, just return the string as soon as posible
			if (mathMLList == null || mathMLList.Count == 0)
				return htmlToReplace;
			// Else, for each one, ask wiris services the convertion
			foreach (var math in mathMLList)
			{
				try
				{
					// URL 
					string url = wirisServiceURL + HttpUtility.UrlEncode("<math>" + math.InnerHtml.ToString() + "</math>");
					string filePath = imgSourcePath + "/" + Guid.NewGuid() + ".png";
                    // Download the img
                    var localFilename = Path.Combine(AppContext.BaseDirectory, @"~/" + filePath);
                    
					using (WebClient client = new WebClient())
					{
						client.Encoding = Encoding.UTF8;
						client.DownloadFile(url, localFilename);
					}
					math.Name = "img";
					math.InnerHtml = "";
					math.SetAttributeValue("src", filePath);
					math.SetAttributeValue("xmlns", "");
				}
				catch (Exception ex)
				{
					try
					{
                        // Ignore and let the Wiris Formula if it wasn't converted
                        string fileName = Path.Combine(AppContext.BaseDirectory, "MyLogs.txt"); // env: IWebHostEnvironment

                        StreamWriter sw = new StreamWriter(fileName);
						sw.WriteLine(DateTime.Now.ToString() + " " + "Wiris cannot be converted "
							+ ex.ToString() + " ENCODE: " + HttpUtility.UrlEncode("<math>" + math.InnerHtml.ToString() + "</math>"));
						sw.Close();
					}
					catch (Exception)
					{
						// Just ignoge. 
					}
				}
			}

			return doc.DocumentNode.OuterHtml;
		}

		/// <summary>
		/// To convert a multi line to a single line.
		/// </summary>
		/// <param name="stringToReplace">The string to replace.</param>
		/// <returns>Returns the multi line string as a single line.</returns>
		public static string MultilineToSingleLine(string stringToReplace)
		{
			return Regex.Replace(stringToReplace, @"\r\n?|\n", " ");
		}

		/// <summary>
		/// Replaces the IMG source of each img tag by the ls_src_http_location setting
		/// </summary>
		/// <param name="html">The string as html.</param>
		/// <returns>Returns the string with the sources replaced.</returns>
		public static string ReplaceSrc(string html)
		{
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			try
			{
				if (doc.DocumentNode.SelectNodes("//img[@src]") != null && doc.DocumentNode.SelectNodes("//img[@src]").Count > 0)
				{
					foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img[@src]"))
					{
						string src = img.GetAttributeValue("src", null);
						if (src.StartsWith("data:image") || src.Contains("base64"))
							continue;

						SignedUrl(img);
					}
				}
				if (HasAudio(doc))
				{
					HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//audio[@src]");
					SignedUrl(nodes);
				}
				if (HasVideo(doc))
				{
					HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//video[@src]");
					SignedUrl(nodes);
				}
				// Replace style 
				if (doc.DocumentNode.SelectNodes("//body") != null && doc.DocumentNode.SelectNodes("//body").Count > 0)
				{
					foreach (HtmlNode bodyNode in doc.DocumentNode.SelectNodes("//body"))
					{
						if (bodyNode.Attributes.Contains("style") && bodyNode.Attributes["style"].Value.Contains("background-image:url"))
						{
							string style = bodyNode.Attributes["style"].Value;
							string oldImg = Regex.Match(style, @"(?<=\().+?(?=\))").Value;
							string fileName = Path.GetFileName(oldImg);
							string filePath = $"{fileName}";
							string signedUrl = AzureAssets.GetFileFromAzureStorage(filePath);
							string newStyle = style.Replace(oldImg, signedUrl);

							bodyNode.Attributes.Remove("style");
							bodyNode.Attributes.Add("style", newStyle);
						}
					}
				}

				if (doc.DocumentNode.SelectNodes("//div") != null && doc.DocumentNode.SelectNodes("//div").Count > 0)
				{
					foreach (HtmlNode bodyNode in doc.DocumentNode.SelectNodes("//div"))
					{
						if (bodyNode.Attributes.Contains("style") && bodyNode.Attributes["style"].Value.Contains("background-image:url"))
						{
							string style = bodyNode.Attributes["style"].Value;
							string oldImg = Regex.Match(style, @"(?<=\().+?(?=\))").Value;
							string fileName = Path.GetFileName(oldImg);
							string filePath = $"{fileName}";
							string signedUrl = AzureAssets.GetFileFromAzureStorage(filePath);
							string newStyle = style.Replace(oldImg, signedUrl);

							bodyNode.Attributes.Remove("style");
							bodyNode.Attributes.Add("style", newStyle);
						}
					}
				}
			}
			catch (Exception ex)
			{

			}

			return doc.DocumentNode.OuterHtml;
		}

        /// <summary>
        /// Replaces the HTML entities.
        /// </summary>
        /// <param name="html">The HTML to replace the entities.</param>
        /// <returns>Returns the replaced HTML.</returns>
        public static string ReplaceHTMLEntities(string html)
        {
            return WebUtility.HtmlDecode(html);
        }

        /// <summary>
        /// Fixes the HTML string by removing extra commas in 'font-family' attributes.
        /// </summary>
        /// <param name="html">The HTML string to be fixed.</param>
        /// <returns>The fixed HTML string.</returns>
        public static string FixFontFamily(string html)
        {
            // Regular expression to find 'font-family: ... ,;'
            string pattern = @"font-family:\s*[^;]*,\s*;";

            // Check if the pattern matches any part of the HTML string
            if (Regex.IsMatch(html, pattern))
            {
                // Replacement pattern to remove the extra comma
                MatchEvaluator replacement = delegate (Match m)
                {
                    return m.Value.Replace(",;", ";");
                };

                // Use Regex.Replace with a MatchEvaluator to apply the replacement
                string fixedHtml = Regex.Replace(html, pattern, replacement);

                return fixedHtml;
            }
            else
            {
                // Return the original HTML string if no matches are found
                return html;
            }
        }
		
		/// <summary>
		/// Replaces the double quotes by single quotes.
		/// </summary>
		/// <param name="html">The HTML string.</param>
		/// <returns>Returns the replaced HTML.</returns>
		public static string ReplaceDoubleQuotesBySingleQuote(string html)
		{
			if (!string.IsNullOrWhiteSpace(html))
			{
				html = html.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
				return html.Replace("\"", "'");
			}
			else
				return "";
		}

		/// <summary>
		/// Determines if the document contains audio elements.
		/// </summary>
		/// <param name="doc">The HTML document to check.</param>
		/// <returns>True if the document contains audio elements, otherwise false.</returns>
		private static bool HasAudio(HtmlDocument doc)
		{
			return doc.DocumentNode.SelectNodes("//audio[@src]") != null && doc.DocumentNode.SelectNodes("//audio[@src]").Count > 0;
		}

		/// <summary>
		/// Determines if the document contains video elements.
		/// </summary>
		/// <param name="doc">The HTML document to check.</param>
		/// <returns>True if the document contains video elements, otherwise false.</returns>
		private static bool HasVideo(HtmlDocument doc)
		{
			return doc.DocumentNode.SelectNodes("//video[@src]") != null && doc.DocumentNode.SelectNodes("//video[@src]").Count > 0;
		}

		/// <summary>
		/// Generates a signed URL for the given HTML node.
		/// </summary>
		/// <param name="node">The HTML node to process.</param>
		private static void SignedUrl(HtmlNode node)
		{
			string src = node.GetAttributeValue("src", null);
			string fileName = Path.GetFileName(src);
			string filePath = $"{fileName}";
			string signedUrl = AzureAssets.GetFileFromAzureStorage(filePath);

			node.SetAttributeValue("src", signedUrl);
		}

		/// <summary>
		/// Generates signed URLs for the given collection of HTML nodes.
		/// </summary>
		/// <param name="nodes">The collection of HTML nodes to process.</param>
		private static void SignedUrl(HtmlNodeCollection nodes)
		{
			foreach (HtmlNode node in nodes)
				SignedUrl(node);
		}
	}
}
