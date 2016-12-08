using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for pwMixer
/// </summary>
public class pwMixer
{
    public static string CreateSalt()
    {
        Random rand = new Random();
        int random_number = rand.Next(0, 100);
        var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        var buff = new byte[random_number];
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }
}