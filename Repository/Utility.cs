using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Repository
{
    public interface IUtility
    {
        string GenerateNumber();
    }
    public class Utility : IUtility
    {
        public string GenerateNumber()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var Charsarr = new char[4];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            return new String(Charsarr);
        }
    }
}
