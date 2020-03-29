namespace DebugTools.Secret
{
    /// <summary>
    /// 情報安全設定タイプ
    /// </summary>
    public enum SecretType : short
    {
        None = 0,          //公開情報
        Level1 = 1,        //暗号化、パスワードを持つ方なら真の内容が見れます、もってない方は***と表示する。
        Level2 = 2,        //一部仮データ化、真の内容の一部を仮の内容に変えることで内容を隠す。
        Level3 = 3         //完全仮データ化、真の内容を仮の内容に変えることで内容を隠す。
    }

}