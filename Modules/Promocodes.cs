using Ideas.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Ideas.Modules
{
    public class Promocodes
    {
        private static Random random = new Random();
        private static string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private List<string> discounts = new List<string>
        {
            "-5%",
            "-10%",
            "-15%",
            "-30%",
            "-50%"
        };
        private readonly ProductReviewContext _context;

        public Promocodes(ProductReviewContext context)
        {
            _context = context;
        }

        public static string GenerateRandomCode(int length = 12)
        {
            StringBuilder promoCode = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                promoCode.Append(characters[index]);
            }

            return promoCode.ToString();
        }

        public static string SelectRandomString(List<string> disounts)
        {
            if (disounts == null || disounts.Count == 0)
            {
                throw new ArgumentException("The list of strings cannot be null or empty.");
            }

            int index = random.Next(disounts.Count);
            return disounts[index];
        }

        public Promocode CreatePromocode()
        {
            Promocode promocode = new Promocode();

            promocode.Expires = DateTime.UtcNow.AddDays(7);
            promocode.Code = GenerateRandomCode();
            promocode.Discount = SelectRandomString(discounts);

            _context.Promocodes.Add(promocode);
            return promocode;
        }
    }
}