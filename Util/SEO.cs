using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Midas.Util
{
    public static class SEO
    {
        public static string UrlSlug(this string recurso)
        {
            string item = RemoveAcento(recurso).ToLower();
            item = Regex.Replace(item, @"[^a-z0-9\s-]", "");
            item = Regex.Replace(item, @"\s+", " ").Trim();
            item = item.Substring(0, item.Length <= 45 ? item.Length : 45).Trim();
            return Regex.Replace(item, @"\s", "-");

        }

        public static string RemoveAcento(this string texto)
        {
            byte[] bytesTexto = System.Text.Encoding.GetEncoding(28593).GetBytes(texto);
            return System.Text.Encoding.ASCII.GetString(bytesTexto);
        }
    }
}
