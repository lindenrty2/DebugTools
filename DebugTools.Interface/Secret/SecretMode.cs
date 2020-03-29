namespace DebugTools.Secret
{
    /// <summary>
    /// 情報安全設定タイプ
    /// </summary>
    public enum SecretMode : short
    {
        None = 0,          //公開情報
        RSA = 1,           //RSA
        Password = 2       //Password
    }

}