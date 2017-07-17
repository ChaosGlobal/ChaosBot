namespace ChaosGlobal.ChaosBot.Components
{
    using static Program;

    /// <summary>
    /// 内部 URN を管理するクラスです。
    /// </summary>

    public static class UrnManager
    {
        /// <summary>
        /// Bot のサムネイルを表す URN です。
        /// </summary>
        public const string BotThumbnail = "urn:chaosglobal:chaosbot-currentuser-thumbnail";

        /// <summary>
        /// 内部 URN を 汎用 URL に変換します。
        /// </summary>
        /// <param name="uri">検証する URI</param>
        /// <returns>URI が内部 URN に一致する場合は汎用 URL を返し、一致しない URN であれば null を返します。それ以外の場合、入力された文字列をそのまま返します。</returns>
        public static string ConvertUrn(this string uri)
        {
            if (uri?.StartsWith("urn:") ?? false) // パラメーターが URN であるかどうかを検証します。
            { // パラメーターが URN である場合
                switch (uri.ToLower())
                {
                    case BotThumbnail: // パラメーターが Bot のサムネイルを表す URN である場合
                        return Client.CurrentUser.GetAvatarUrl(); // Bot のサムネイルが存在する URL を取得して返します。
                    default: // どの内部 URN にも一致しない場合
                        return null; // null を返します。
                }
            }
            else
            { // パラメーターが URN でない場合
                return uri; // パラメーターをそのまま返します。
            }
        }
    }
}
