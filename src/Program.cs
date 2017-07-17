using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChaosGlobal.ChaosBot
{
    /// <summary>
    /// <see cref="ChaosGlobal.ChaosBot.Program"/> のメインルーチンです。
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Discord API のトークンです。
        /// </summary>
        private const string Token = "";

        /// <summary>
        /// <see cref="ChaosGlobal.ChaosBot.Program"/> の静的コンストラクターです。
        /// </summary>
        static Program()
        {
            Client = new DiscordSocketClient();
        }

        /// <summary>
        /// 既定の Discord API クライアントを取得します。
        /// </summary>
        /// <returns><see cref="ChaosGlobal.ChaosBot"/> が用いる既定の Discord API クライアント</returns>
        internal static DiscordSocketClient Client { get; }

        /// <summary>
        /// <see cref="ChaosGlobal.ChaosBot"/> のエントリポイントです。
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        private static void Main(string[] args)
        {
            new Program().MainAsync(args).GetAwaiter().GetResult();
        }

        /// <summary>
        /// <see cref="ChaosGlobal.ChaosBot"/> の非同期エントリポイントです。
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        private async Task MainAsync(string[] args)
        {
            try
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // 現時点のUNIX時間を記録します。

                Directory.CreateDirectory("log"); // ログフォルダが存在しない場合、ログフォルダを作成します。

                using (var logStream = new FileStream(Path.Combine("log", $"{now}_out.log"), FileMode.CreateNew, FileAccess.Write, FileShare.Read)) // ログファイルを生成します。
                using (var errorStream = new FileStream(Path.Combine("log", $"{now}_error.log"), FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                using (var logWriter = new StreamWriter(logStream)) // ログライターを作成します。
                using (var errorWriter = new StreamWriter(errorStream))
                {
                    logWriter.AutoFlush = true; // 書き込みが行われる度バッファーをフラッシュします。
                    errorWriter.AutoFlush = true;

                    Console.SetOut(logWriter); // コンソールの書き込み先をログファイルに向けます。
                    Console.SetError(errorWriter);

                    Client.Log += Log;

                    await Client.LoginAsync(TokenType.Bot, Token); // トークンを利用し、Discord API に Bot を認証します。
                    await Client.StartAsync(); // Discord API の使用を開始します。

                    await Task.Delay(-1); // 無期限に継続するタスクを待機し、タスクの終了を抑止します。
                }
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message); // 暫定的な例外処理です。エラーメッセージを書き込みます。
            }
        }
        private Task Log(LogMessage log)
        {
            var context = $"{DateTimeOffset.Now:s} [{log.Severity}]\t{log.Message}"; // 書き込むログを生成します。
            switch (log.Severity)
            { // ログのタイプによって書き込み先を振り分けます。
                case LogSeverity.Critical: // クリティカルエラーの場合
                case LogSeverity.Error: // エラーの場合
                case LogSeverity.Warning: // 警告の場合
                    return Console.Error.WriteLineAsync(context); // 上記のいずれかである場合、エラーログファイルに書き込みます。
                default:
                    return Console.Out.WriteLineAsync(context); // それ以外のログは通常ログファイルに書き込みます。
            }
        }
    }
}
