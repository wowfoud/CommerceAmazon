using Commerce.Amazon.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Amazon.Domain.Helpers
{
    public class TokenManager
    {
        public string GenerateToken(DataUser dataUser)
        {
            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] idUser = GetBytes(dataUser.IdUser.ToString("0000"));
            byte[] userId = GetBytes(dataUser.UserId.ToString());
            byte[] data = new byte[_time.Length + idUser.Length + userId.Length];

            System.Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            System.Buffer.BlockCopy(idUser, 0, data, _time.Length, idUser.Length);
            System.Buffer.BlockCopy(userId, 0, data, _time.Length + idUser.Length, userId.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        public DataUser DecodeToken(string token)
        {
            DataUser dataUser = new DataUser();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] idUser = data.Skip(8).Take(4).ToArray();
            byte[] userId = data.Skip(12).ToArray();

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                throw new Exception("token expired");
            }
            dataUser.IdUser = int.Parse(GetString(idUser));
            dataUser.UserId = GetString(userId);
            return dataUser;
        }
        
        public TokenValidation ValidateToken(DataUser dataUser, string token)
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] idUser = data.Skip(8).Take(4).ToArray();
            byte[] userId = data.Skip(12).ToArray();

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }

            if (dataUser.IdUser.ToString("0000") != GetString(idUser))
            {
                result.Errors.Add(TokenValidationStatus.WrongGuid);
            }

            if (dataUser.UserId.ToString() != GetString(userId))
            {
                result.Errors.Add(TokenValidationStatus.WrongUser);
            }

            return result;
        }

        private static string GetString(byte[] reason) => Encoding.ASCII.GetString(reason);

        private static byte[] GetBytes(string reason) => Encoding.ASCII.GetBytes(reason);
    }
    public class TokenValidation
    {
        public bool Validated { get { return Errors.Count == 0; } }
        public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
    }

    public enum TokenValidationStatus
    {
        Expired,
        WrongUser,
        WrongPurpose,
        WrongGuid
    }
}
