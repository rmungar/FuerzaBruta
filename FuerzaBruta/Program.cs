using System.Security.Cryptography;
using System.Text;

class Program
{
    public static void Main(string[] args)
    {
        
        List<string> lines = File.ReadLines("C:\\Users\\usuario\\RiderProjects\\FuerzaBruta\\FuerzaBruta\\passwords.txt").ToList();
        List<string> passwordList = lines.ToList() ;
        
        
        String aCrackear = passwordList[RandomNumberGenerator.GetInt32(0, passwordList.Count)];
        
        String hashedPassword = GetHashString(aCrackear);
        
        for (int i = 0; i < lines.Count; i++)
        {
            if (GetHashString(lines[i]) == hashedPassword)
            {
                Console.Write(lines[i]);
            }
        }
    }
    
    // STACK OVERFLOW UNE AMBAS PARA PODER GENERAR UN HASH DE UN STRING
    public static byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
    
    
    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
}