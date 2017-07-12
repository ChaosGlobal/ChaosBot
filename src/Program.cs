using Discord;
using Discord.Net;
using Discord.WebSocket;
using System;
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
        /// <see cref="ChaosGlobal.ChaosBot.Program"/> のコンストラクターです。
        /// </summary>
        private Program()
        {
            Client = new DiscordSocketClient();
        }

        /// <summary>
        /// 既定の Discord API クライアントを取得します。
        /// </summary>
        /// <returns><see cref="ChaosGlobal.ChaosBot"/> が用いる既定の Discord API クライアント</returns>
        internal DiscordSocketClient Client { get; }

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
                await Client.LoginAsync(TokenType.Bot, Token); // トークンを利用し、Discord API に Bot を認証します。
                await Client.StartAsync(); // Discord API の使用を開始します。

                await Task.Delay(-1); // 無期限に継続するタスクを待機し、タスクの終了を抑止します。
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message); // 暫定的な例外処理です。エラーメッセージを書き込みます。
            }
        }
    }
}
