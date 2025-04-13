
namespace PluginSystem.Core
{
    public static class PluginHelper
    {
        // Генерация уникального ID плагина на основе его имени и версии
        public static Guid GeneratePluginId(IPlugin plugin)
        {
            // Генерация хэша на основе имени и версии плагина
            var hash = plugin.Name.GetHashCode() ^ plugin.Version.GetHashCode();

            // Используем хэш для создания GUID
            return new Guid(BitConverter.GetBytes(hash).Concat(BitConverter.GetBytes(hash)).ToArray());
        }

        public static Guid GeneratePluginId(string[] metadata)
        {
            // Объединяем все строки в одну
            string combined = string.Join("|", metadata);

            // Получаем SHA256 хэш (32 байта) — точно хватит
            using var sha = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined));

            // Берём первые 16 байт из хэша для GUID
            byte[] guidBytes = hash.Take(16).ToArray();

            return new Guid(guidBytes);
        }

        // Дополнительные хелпер-методы можно добавить здесь
    }
}
