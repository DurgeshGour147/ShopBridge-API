using iText.Html2pdf;
using Jewelry_Store.ProviderInterface;
using System;
using System.IO;

namespace Jewelry_store.Provider
{
    public class JewelryProvider : IJewelryProvider
    {
        public byte[] ConvertHTMLToPDF(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString))
                return null;
            try
            {
                MemoryStream stream = new MemoryStream();
                HtmlConverter.ConvertToPdf(htmlString, stream);
                return stream.ToArray();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
        }
    }
}
