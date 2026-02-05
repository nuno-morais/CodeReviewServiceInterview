using CodeReviewServiceInterview.Models;
using CodeReviewServiceInterview.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace CodeReviewServiceInterview.Services;

static class MessagesService
{
    private static MessagesRepository _messagesRepository = new MessagesRepository();
    private const string Key = "1234567890123456";
    private const string IV = "abcdefghijklmnop";

    public static List<Message> GetMessages()
    {
        return _messagesRepository.GetMessages().Select(m => new Message
        {
            Id = m.Id,
            From = m.From,
            To = m.To,
            Content = Decrypt(m.Content)
        }).ToList();
    }

    public static Message Create_Message(string from, string to, string content, string provider)
    {
      	if (content == null || content.Length < 5) {
          throw new Exception("Content too short");
        }

      	try {
          return _messagesRepository.AddMessage(from, to, Encrypt(content), provider);
        } catch (Exception ex) {
          throw new Exception("Fail");
        }
    }

    private static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV  = Encoding.UTF8.GetBytes(IV);

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);

        sw.Write(plainText);
        sw.Close();

        return Convert.ToBase64String(ms.ToArray());
    }

    private static string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV  = Encoding.UTF8.GetBytes(IV);

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
