using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEcdsaSignButVerify.Models
{
    public class DigitalSignatureSettings
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string JsonFile { get; set; }
        public string JsonFileTampered { get; set; }
    }
}
