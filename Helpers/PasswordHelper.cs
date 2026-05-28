namespace Oxygen.Helpers
{
    public class PasswordHelper
    {
        // convert password to hash
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //Checking the password
        public static bool VerifyPassword(
            string password,
            string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                passwordHash);
        }
    }
}