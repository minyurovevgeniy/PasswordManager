using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PasswordManager
{
    internal static class CryptographyClass
    {
        
        /// <summary>
        /// Сериализует список в JSON, шифрует и сохраняет в файл.
        /// Формат файла: [IV (12 байт)][AuthTag (16 байт)][CipherData]
        /// </summary>
        public static void EncryptAndSave<T>(List<T> data, string filePath, byte[] Key, byte[] Iv)
        {
            // Шаг 1: Сериализация списка в JSON-строку
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,              // Красивый отформатированный JSON (для файлов/логов)
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Опционально: поля в camelCase]
            };

            string jsonArray = JsonSerializer.Serialize(data, options);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonArray);

            const int tagSize = 16;
                
            // Шаг 2: Шифрование (AES-GCM)
            using (var aes = new AesGcm(Key, tagSize))
            {
                byte[] cipherBytes = new byte[jsonBytes.Length];
                byte[] authTag = new byte[tagSize]; // Тег аутентификации

                
                for (int i = 0; i < authTag.Length; i++)
                {
                    authTag[i] = 2;
                }
                
                aes.Encrypt(Iv, jsonBytes, cipherBytes, authTag);

                // Шаг 3: Запись в файл в формате: IV + AuthTag + Данные
                using (var fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write))
                {
                    fileStream.Write(Iv, 0, Iv.Length);
                    fileStream.Write(authTag, 0, authTag.Length);
                    fileStream.Write(cipherBytes, 0, cipherBytes.Length);
                }
            }
        }

        /// <summary>
        /// Читает файл, расшифровывает и возвращает список.
        /// </summary>
        public static List<T> LoadAndDecrypt<T>(string filePath, byte[] Key)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден");

            byte[] fileBytes = File.ReadAllBytes(filePath);

            const int ivSize = 12;
            const int tagSize = 16;

            if (fileBytes.Length < ivSize + tagSize)
                throw new InvalidDataException("Файл поврежден или имеет неверный формат");

            byte[] storedIv = new byte[ivSize];
            byte[] storedTag = new byte[tagSize];
            byte[] encryptedData = new byte[fileBytes.Length - ivSize - tagSize];

            Array.Copy(fileBytes, 0, storedIv, 0, ivSize);
            Array.Copy(fileBytes, ivSize, storedTag, 0, tagSize);
            Array.Copy(fileBytes, ivSize + tagSize, encryptedData, 0, encryptedData.Length);

            // Расшифровка
            byte[] decryptedBytes;
            using (var aes = new AesGcm(Key,tagSize))
            {
                decryptedBytes = new byte[encryptedData.Length];                
                try
                {
                    aes.Decrypt(storedIv, encryptedData, storedTag, decryptedBytes);
                }
                catch (CryptographicException ex)
                {
                    throw new Exception("Ошибка расшифровки: неверный ключ, IV или файл поврежден.", ex);
                }
            }

            // Преобразование байтов обратно в JSON-строку
            string jsonString = Encoding.UTF8.GetString(decryptedBytes);

            // Десериализация JSON в List<T>
            return JsonSerializer.Deserialize<List<T>>(jsonString)
                   ?? throw new InvalidDataException("Не удалось десериализовать JSON");
        }
        
    }
}
