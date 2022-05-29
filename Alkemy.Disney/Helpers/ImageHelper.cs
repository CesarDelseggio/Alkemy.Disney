using Alkemy.Disney.Controllers.Images;
using Microsoft.AspNetCore.Http;

namespace Alkemy.Disney.Helpers
{
    /// <summary>
    /// Esta clase guarda una en una de sus propiedades la url base de la api, con la cual luego 
    /// construye las urls de las imagenes de las distintas entidades.
    /// Las urls devueltas tendran el formato https://domain/images/genres
    /// </summary>
    public static class ImageHelper
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static HttpContext Current => httpContextAccessor?.HttpContext;

        public static string AppBaseUrl => $"{Current?.Request.Scheme}://{Current?.Request.Host}{Current?.Request.PathBase}";

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            httpContextAccessor = contextAccessor;
        }

        public static string ImagesURL() {
            var imgControler = nameof(ImagesController).Replace("Controller", "");
            return $"{AppBaseUrl}/{imgControler}";
        }

        public static string GenresURL { get { return $"{ImagesURL()}/{nameof(ImagesController.Genres)}"; } }

        public static string CaractersURL { get { return $"{ImagesURL()}/{nameof(ImagesController.Caracters)}"; } }

        public static string MoviesURL { get { return $"{ImagesURL()}/{nameof(ImagesController.Movies)}"; } }
    }
}
