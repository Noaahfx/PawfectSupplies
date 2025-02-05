using System;
using System.Linq;

namespace PawfectSupplies.Utilities
{
    public static class RatingHelper
    {
        public static string GenerateStarRatingHTML(decimal rating)
        {
            int fullStars = (int)Math.Floor(rating);
            decimal fractionalPart = rating - fullStars;
            string starsHTML = "";

            // Full stars
            for (int i = 0; i < fullStars; i++)
            {
                starsHTML += "<i class='fas fa-star text-yellow-500'></i>";
            }

            // Half-filled star if rating has a decimal part
            if (fractionalPart > 0)
            {
                starsHTML += "<i class='fas fa-star-half-alt text-yellow-500'></i>";
            }

            // Empty stars to complete 5
            int remainingStars = 5 - (fullStars + (fractionalPart > 0 ? 1 : 0));
            for (int i = 0; i < remainingStars; i++)
            {
                starsHTML += "<i class='far fa-star text-yellow-500'></i>";
            }

            return starsHTML;
        }
    }
}
