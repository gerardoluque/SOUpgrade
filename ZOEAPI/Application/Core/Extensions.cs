namespace API.Application.Core.Extensions
{
    /// <summary>
    /// Contiene métodos de extensión para trabajar con cadenas.
    /// </summary>
    public static class ExtensionPlaceHolder
    {
        /// <summary>
        /// Indica si una cadena es nula, vacía o solo espacios en blanco.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return value == null || string.IsNullOrEmpty(value.Trim()) ? true : false;
        }

        /// <summary>
        /// Indica si una cadena no es nula, vacía o solo espacios en blanco.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string? value)
        {
            return !IsNullOrWhiteSpace(value);
        }
    }
}
