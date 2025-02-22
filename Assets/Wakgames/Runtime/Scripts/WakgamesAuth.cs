using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WakSDK
{
    public static class WakgamesAuth
    {
        private const string Characters = "abcdefghijklmnopqrstuvwxyz123456789";

        private static char RandomCharacter => Characters[UnityEngine.Random.Range(0, Characters.Length)];
    
        public static string GenerateCodeVerifier()
        {
            var nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = RandomCharacter;
            }

            return new string(nonce);
        }

        public static string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            string b64Hash = Convert.ToBase64String(hash);
            string code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }

        public static string GenerateCsrfState()
        {
            var nonce = new char[16];
            for (int i = 0; i < nonce.Length; i++)
            {
                nonce[i] = RandomCharacter;
            }

            return new string(nonce);
        }
    }
}
