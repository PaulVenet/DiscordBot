//using Discord.Commands;
//using System.Threading.Tasks;
//using Discord.WebSocket;
//using Discord;
//using EynwaDiscordBot.Functions;
//using System.Linq;
//using System.IO;

//namespace EynwaDiscordBot.Modules
//{
//    public class PlaySong : ModuleBase<SocketCommandContext>
//    {
//        private SocketVoiceChannel _voiceChannel;
//        private SendAudio _sendAudio;

//        [Command("Play", RunMode = RunMode.Async)]
//        public async Task Play(string nameRequest = null, IVoiceChannel channel = null)
//        {
//            string path ="";
//            var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Song");
//            string[] songList = new string[files.Length];
//            for (int i=0; i < files.Length; i ++)
//            {
//                if(i == 0)
//                {
//                    songList[i] = Path.GetFileNameWithoutExtension(files[i]);
//                }
//                else
//                {
//                    songList[i] = ", " + Path.GetFileNameWithoutExtension(files[i]);
//                }
//                if (nameRequest != null && files[i].Contains(nameRequest))
//                {
//                    path = files[i];
//                    break;
//                }
//                if(i == files.Length -1)
//                {
//                    await Context.Message.Channel.SendMessageAsync("Liste des sons dispo : " + string.Concat(songList));
//                }
//            }
//            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
//            if (channel == null) { await Context.Message.Channel.SendMessageAsync("Connecte toi dans un chanel vocal pour m'invoquer !"); return; }
            
//            this._voiceChannel = this.Context.Guild.GetVoiceChannel(channel.Id);
//            var audioClient = await _voiceChannel.ConnectAsync();

//            _sendAudio = new SendAudio();
//            await _sendAudio.SendAsync(audioClient, path);
//            audioClient.Dispose();
//        }
//    }
//}