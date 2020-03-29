namespace DebugTools.Secret
{

    public interface ISecretService
    {

        string KeyName { get; }
        SecretMode SecretMode { get; }
        byte[] Encrypt(byte[] bytes);
        byte[] Dencrypt(byte[] bytes);

    }

}