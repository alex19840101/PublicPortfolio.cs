﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace LiteAuthService.Core.Auth
{
    /// <summary>
    /// Класс для хэширования
    /// ((т.к. PasswordHasher возвращает разные хэши одного пароля))
    /// </summary>
    public static class SHA256Hasher
    {
        public static string GetHash(string stringForHashing)
        {
            byte[] hash;

            using (SHA256 sha256hasher = new SHA256Managed())
            {
                hash = sha256hasher.ComputeHash(Encoding.UTF8.GetBytes(stringForHashing.ToString()));
                sha256hasher.Clear();
            }
            return Convert.ToBase64String(hash);
        }
    }
}
