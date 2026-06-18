using Entities;
using Org.BouncyCastle.Crypto.Generators;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Services
{
    public class PasswordService : IPasswordService
    {
        public PasswordService() { }
        public PasswordEntity getStrengthByPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            int strength = result.Score;
            PasswordEntity passwordEntity = new PasswordEntity();
            passwordEntity.Password = password;
            passwordEntity.Strength = strength;
            return passwordEntity;
        }
        // 1. הצפנת סיסמה בעת הרשמה
        public string HashPassword(string password)
        {
            // הפונקציה מייצרת Salt ייחודי אוטומטית ומחזירה מחרוזת משולבת
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // 2. אימות סיסמה בעת התחברות
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // הפונקציה יודעת לפרק את ה-hashedPassword, לחלץ את ה-Salt ולבדוק התאמה
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}

