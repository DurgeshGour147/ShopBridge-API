using System;

namespace Jewelry_Store.ProviderInterface
{
    public interface IJewelryProvider
    {
        byte[] ConvertHTMLToPDF(string htmlString);
    }
}
