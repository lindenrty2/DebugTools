using System;
using System.Runtime.InteropServices;

namespace DebugTools.DBO
{
    [Serializable()]
    public struct PackageMate
    {

        /// <summary>
    /// バージョン
    /// </summary>
    /// <remarks></remarks>
        public int Version;

        /// <summary>
    /// ブロック一覧の開始位置
    /// </summary>
    /// <remarks></remarks>
        public long BlockStartPosition;

        /// <summary>
    /// ブロックの数
    /// </summary>
    /// <remarks></remarks>
        public int BlockCount;

        /// <summary>
    /// セキュリティモード(0:セキュリティなし、1:RSA認証、2:Password認証)
    /// </summary>
    /// <remarks></remarks>
        public short SecretMode;

        /// <summary>
    /// 認証用キー名
    /// </summary>
    /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string KeyName;

        /// <summary>
    /// 提示
    /// </summary>
    /// <remarks></remarks>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Tip;
    }
}
