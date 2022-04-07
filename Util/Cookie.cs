using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Util
{
    public static class ImageHandler
    {
        //    /// <summary>
        //    /// Salva e comprime a imagem com base no caminho, no sistema de arquivos.
        //    /// </summary>
        //    /// <param name="arquivo"></param>
        //    /// <param name="caminho"></param>
        //    public static void FromImage(this IFormFile arquivo, string caminho)
        //    {
        //        using Image novaImagem = Image.FromStream(arquivo.OpenReadStream());
        //        int altura = novaImagem.Height;
        //        int largura = novaImagem.Width;
        //        double novaAltura, novaLargura;
        //        novaAltura = altura - (altura * 0.6);
        //        novaLargura = largura - (largura * 0.6);
        //        Bitmap btmap = new((int) novaAltura, (int) novaLargura, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
        //        using Graphics novoGrafico = Graphics.FromImage(novaImagem);
        //        novoGrafico.DrawImage(novaImagem, 0, 0, (int)novaLargura, (int)novaAltura);
        //        btmap.Save(caminho);
        //    }

        //    private static ImageCodecInfo GetEncoderInfo(this string mimeType)
        //    {
        //        // Get image codecs for all image formats 
        //        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        //        // Find the correct image codec 
        //        for (int i = 0; i < codecs.Length; i++)
        //            if (codecs[i].MimeType == mimeType)
        //                return codecs[i];

        //        return null;
        //    }
        //}

        public static string ConvertToDataUriFormat(this string imagemFormatoBase64, string formato)
        {
            StringBuilder imagemBase64 = new($"data:{formato};base64,");
            imagemBase64.Append(imagemFormatoBase64);
            return imagemBase64.ToString();
        }

        //public static string[,] ParseCartCookie(string cookieName = "mdc")
        //{
        //    return ['',''];

        //}
    }
}
