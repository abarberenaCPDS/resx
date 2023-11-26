import java.util.*;
import java.io.*;
import java.nio.charset.*;
import java.security.*;
import java.security.interfaces.*;
import java.security.spec.*;
import java.nio.file.*;

class verify {
  private static final char[] HEX_ARRAY = "0123456789ABCDEF".toCharArray();

public static String bytesToHex(byte[] bytes) {
    char[] hexChars = new char[bytes.length * 2];
    for (int j = 0; j < bytes.length; j++) {
        int v = bytes[j] & 0xFF;
        hexChars[j * 2] = HEX_ARRAY[v >>> 4];
        hexChars[j * 2 + 1] = HEX_ARRAY[v & 0x0F];
    }
    return new String(hexChars);
}

public static PublicKey readPublicKey(File file) throws Exception {
    String key = new String(Files.readAllBytes(file.toPath()), Charset.defaultCharset());

    String publicKeyPEM = key
      .replace("-----BEGIN PUBLIC KEY-----", "")
      .replaceAll(System.lineSeparator(), "")
      .replace("-----END PUBLIC KEY-----", "");
    System.out.println("--> Public Key\t" + publicKeyPEM);
    byte[] encoded = Base64.getDecoder().decode(publicKeyPEM);

    KeyFactory keyFactory = KeyFactory.getInstance("EC");
    X509EncodedKeySpec keySpec = new X509EncodedKeySpec(encoded);
    return (PublicKey) keyFactory.generatePublic(keySpec);
}    
    
  public static void main(String[] args) throws Exception
  {
    // Check how many arguments were passed in
    if (args.length < 2) {
      System.out.println("Usage: java verify {input-filepath} {x-clearcapital-signature}");
      System.exit(0);
    }

    String pubKeyFile = "pub8.pem";

    System.out.println("\n=== Running ===");
    final byte[] input = Files.readAllBytes(Paths.get(args[0]));
    System.out.println("--> Input\t"+ args[0]);

    byte[] signature = Base64.getDecoder().decode(args[1].getBytes());
    // byte[] signature = args[1].getBytes("UTF-8");
    System.out.println("--> Signature\t"+ args[1]);
  
    Signature ecdsaVerify = Signature.getInstance("SHA256withECDSA");
    ecdsaVerify.initVerify(readPublicKey(new File(pubKeyFile)));
    System.out.println("--> PubKey File\t" + pubKeyFile);


    System.out.println("--> Ecdsa ready\t" + ecdsaVerify);
    ecdsaVerify.update(input);
    System.out.println("--> Ecdsa updated...");
    boolean result = ecdsaVerify.verify(signature);
    System.out.println("--> Ecdsa verified..." + result);
    
    System.out.println("\n");
  }
}
