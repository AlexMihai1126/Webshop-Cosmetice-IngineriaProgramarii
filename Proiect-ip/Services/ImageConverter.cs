using Proiect_ip.Data;
using Proiect_ip.Models;
using System;
using System.Threading.Tasks;

namespace Proiect_ip.Services
{
    public class ImageConverter(Proiect_ipContext context)
    {
        private readonly Proiect_ipContext _context = context;

        public async Task<string?> GetImageSrcAsync(int productId)
        {
            var product = await _context.Produse.FindAsync(productId);

            if (product == null || product.ImageData == null || product.ImageData.Length == 0)
            {
                return null;
            }

            var imageBase64 = Convert.ToBase64String(product.ImageData);
            return $"data:{product.ImageType};base64,{imageBase64}";
        }
    }
}
